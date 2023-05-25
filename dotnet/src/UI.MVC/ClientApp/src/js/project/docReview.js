import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";
import {selectText} from "./../shared/select.js";

window.addEventListener("load", init)
let html = "";

/**
 * @author Michiel Verschueren
 * Entry function that gets called on page load
 * @returns {Promise<void>}
 */
async function init() {
    popup.init();
    
    //Get the createButton and add a eventlistener to it
    const createButton = document.querySelector("#btn-create-survey");
    if (createButton != null) {
        const docReviewId = createButton.dataset.docReviewId;
        createButton.addEventListener("click", () => {
            buttonCreateSurveyClicked(docReviewId)
        })
    }

    //Get the SelectTextButton and add a eventListener to it
    const selectTextButton = document.querySelector("#btn-select-text-survey");
    if (selectTextButton != null) {

        selectTextButton.addEventListener("click", select);
    }

    //Get all the submitButtons for all the surveys and add eventlisteners to them
    const surveyAnswerButtons = document.querySelectorAll(".btn-survey-answer");
    for (let i = 0; i < surveyAnswerButtons.length; i++) {
        surveyAnswerButtons[i].addEventListener("click", () => {
             submitSurvey(surveyAnswerButtons[i].dataset.surveyId)
            surveyAnswerButtons[i].disabled = true;
        })
    }

    initToggleCommentsSurvey();

} // init.

/**
 * @author Michiel Verschueren
 * Function to select the quote
 */
function select(){
    //get the selection
    let selection = selectText();
    html = selection["html"];
    
    //Display the selectedText in the sidebar
    const field = document.getElementById('selected-text-survey');
    field.innerHTML = selection["text"];
}

/**
 * @author Michiel Verschueren
 * Function that gets called when the button create survey is clicked
 * @param docReviewId the id of the current docReview
 */
async function buttonCreateSurveyClicked(docReviewId) {
    if (html === "") {
        popup.showMessage("Please select some text first", "danger", ".survey-response", 5000);
        return;
    }
    
    // Add eventlistener for the confirm button
    const btnCreateSurvey = document.querySelector(".btn-confirm-create-survey");
    btnCreateSurvey.replaceWith(btnCreateSurvey.cloneNode(true));

    document.querySelector(".btn-confirm-create-survey").addEventListener("click", () => {
        createSurvey(docReviewId);
    });

    //Get the add options button
    const addOptionButton = document.querySelector("#add-option");

    //Get all the options
    const options = document.querySelectorAll(".option");

    //get the last option
    const lastOption = options[options.length - 1];

    //get the id of the last option
    const optionsId = parseInt(lastOption.dataset.optionId);
    
    //Replace the add option button with a clone to all the event listeners
    addOptionButton.replaceWith(addOptionButton.cloneNode(true));
    
    //Add eventlistener to the add option button
    document.querySelector("#add-option").addEventListener("click", () => {
        addExtraOption(optionsId)
    })

    //Show the popup
    popup.showPopup(".article-popup-create-survey");
}

/**
 * @author Michiel Verschueren
 * Function that gets called when a user clicks the confirm button
 */
async function createSurvey(docReviewId) {
    // Get all the inputs from the popup
    const title = document.querySelector("#title");
    const description = document.querySelector("#description");
    const multipleOptions = document.querySelector("#multipleChoiceAnswer");
    let possibleOptions = "{";
    const options = document.querySelectorAll(".option");

    // parse the options to json
    for (let i = 0; i < options.length; i++) {
        possibleOptions += `"${options[i].querySelector("#option-text").value}":"${options[i].querySelector("#option-description").value}"`;
        if (i + 1 < options.length) {
            possibleOptions += ","
        }
    }
    possibleOptions += "}";

    //make the body of the request
    const MultipleAnswer = parseInt(multipleOptions.value) === 2;
    const body = {
        DocReviewId: docReviewId,
        Title: title.value,
        Description: description.value,
        AreMultipleOptionsAllowed: MultipleAnswer,
        Options: JSON.parse(possibleOptions),
        Quote: html
    }

    // Make the request
    const response = await fetch(url.url() + url.getProjectName() + "/docreviews/AddSurvey", {
        method: "POST",
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json"
        }
    })
    popup.closePopup(".article-popup-create-survey");

    //error handeling
    if (response.ok) {
        popup.showMessage("Survey created", "success", ".survey-response", 5000)
        setTimeout(() => {
            window.location.reload();
        }, 3000);
    } else {
        let errors;
        let errorResponse = await response.text();
        try {
            const errorsResponse = JSON.parse(errorResponse).errors;
            errors = parseErrors(errorsResponse);
        } catch {
            errors = errorResponse
        }
        popup.showMessage(errors, "danger", ".survey-response", -1);
    }
    const btnCreateSurvey = document.querySelector(".btn-confirm-create-survey");
}

/**
 * @author Michiel Verschueren
 * Function to return the error content from the response
 * @param errorResponse the errors returned as response from the api call
 * @returns {*[]} an array of strings which represent the errors
 */
function parseErrors(errorResponse) {
    let props = Object.getOwnPropertyNames(errorResponse);
    let errors = [];
    for (let i = 0; i < props.length; i++) {
        errors.push(Reflect.get(errorResponse, props[i]));
    }
    return errors;
}


/**
 * @author Michiel Verschueren
 * Add an extra option div to the page
 */
function addExtraOption() {
    // Get last option
    const options = document.querySelectorAll(".option");
    const lastOption = options[options.length - 1];

    // Get the latest id if there is no option leave the id 0
    let optionsId = 0
    if (lastOption !== undefined) {
        optionsId = parseInt(lastOption.dataset.optionId);
    }

    //Get the container element for all the options
    const optionContainer = document.querySelector("#options-div");
    const currentId = optionsId + 1;

    // Create a new div element for a new option
    const newOptionDiv = document.createElement("div");
    newOptionDiv.classList.add("option");
    newOptionDiv.dataset.optionId = currentId;
    newOptionDiv.innerHTML = `
        <h3>Option ${currentId+1}</h3>
        <input type='text' name='option-text' id='option-text' placeholder='Option text' class='form-control'>
        <input type='text' name='option-description' id='option-description' placeholder='Description' class='form-control'>

        <button class='btn-icon btns-delete' data-option-id='${currentId}'>
            <i class='fa-solid fa-trash-can'></i>
        </button>
    `;
    // Display the new option on the page
    optionContainer.appendChild(newOptionDiv);

    //add the event listeners
    addEventListeners();
}


/**
 * @author Michiel Verschueren
 * Function that adds the event listeners to the delete buttons of all the options
 */
function addEventListeners() {
    const options = document.querySelectorAll(".btns-delete");
    for (let i = 0; i < options.length; i++) {
        options[i].removeEventListener("click", removeOption)
        options[i].addEventListener("click", removeOption)
    }
}

/**
 * @author Michiel Verschueren
 * listener functions that removes the option where the remove button is clicked from
 * @param event
 */
function removeOption(event) {
    //Get the dom element that is clicked
    let target = event.target;

    // If the element is a PATH make the target the parent node of the element
    if (target.tagName.toLowerCase() === 'path') {
        target = target.parentNode;
    }

    // If the element is a SVG make the target the parent node of the element
    if (target.tagName.toLowerCase() === 'svg') {
        target = target.parentNode;
    }
    // Get the data-attribute which holds the 'id' of the option
    const id = target.dataset.optionId;

    // Remove the option with the same 'id' as the clicked element
    const optionsDiv = document.querySelector(".option[data-option-id='" + id + "']");
    optionsDiv.remove();
}

/**
 * @author Michiel Verschueren
 * function that uses webApi to add a user response to a survey to the database
 * @param surveyId the id of the survey
 */
async function submitSurvey(surveyId) {
    const errorMessageContainerClass =`.survey-answer-response[data-survey-id="${surveyId}"]`

    // Get the right SurveyDiv element
    const surveyDiv = document.querySelector(`.survey[data-survey-id="${surveyId}"]`);

    //Get the checked options
    const checkedOptionIds = Array.from(surveyDiv.querySelectorAll(".survey-option-input:checked"));

    //Map the options on their id's
    const ids2 = checkedOptionIds.map(option => option.dataset.optionId);

    //make api call to add the user response to the database
    const response = await fetch(`${url.url() + url.getProjectName()}/docReviews/surveyResponse/${surveyId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            optionIds: ids2
        })
    });
    // error handling
    if (response.ok) {
        popup.showMessage("Thank you for responding", "success", errorMessageContainerClass, 5000)
    } else {
        let errors;
        let errorResponse = await response.text();
        try {
            const errorsResponse = JSON.parse(errorResponse).errors;
            errors = parseErrors(errorsResponse);
        } catch {
            errors = errorResponse
        }
        popup.showMessage(errors, "danger", errorMessageContainerClass, -1)
    }
}

function initToggleCommentsSurvey() {
    const toggleButtons = document.querySelectorAll(".btn-toggle-comments-survey");
    toggleButtons.forEach(button => {
        button.addEventListener("click", toggleCommentSurvey);
        button.active = button.dataset.toggleType;
    });
}

function toggleCommentSurvey(evt) {
    const type = evt.currentTarget.active;

    let curActive, newActive;

    if (type === "comments") {
        newActive = document.querySelector(".doc-review-toggle-comments");
        curActive = document.querySelector(".doc-review-toggle-surveys");
    } else {
        newActive = document.querySelector(".doc-review-toggle-surveys");
        curActive = document.querySelector(".doc-review-toggle-comments");
    }

    curActive.classList.remove("active");
    newActive.classList.add("active");

}















