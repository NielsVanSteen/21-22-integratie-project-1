import * as popup from "../shared/popup.js";
import * as url from "../shared/url.js"

window.addEventListener('load', init)

/**
 * @author Michiel Verschueren
 * entry point function that is called when the page loads
 */
function init() {
    popup.init()

    //Add all the eventlisteners
    document.querySelector("#upload-pdf").addEventListener("click", uploadPdf)
    document.querySelector(".import-pdf-popup-btn").addEventListener("click", () => {
        popup.showPopup(".article-popup-import-pdf")
    })
    document.querySelector("#import-google-docs").addEventListener("click", importFromGoogleDocs)
    document.querySelector(".import-tutorial").addEventListener("click", () => {
        popup.showPopup(".article-popup-import-help")
    })
}

/**
 * @author Michiel Verschueren
 * Uploads a pdf file to the server
 *
 * @returns {Promise<void>}
 */
async function uploadPdf() {
    //Close the upload popup
    popup.closePopup(".article-popup-import-pdf")

    //Show a status message
    popup.showMessage("Uploading PDF...", "success", ".upload-status-container", -1)
    // Define the header.
    let h = new Headers();
    h.append("Accept", "text/html");

    // Construct the form data.
    let formData = new FormData();
    let pdf = document.querySelector("#pdf").files[0];
    if (pdf === null || pdf === undefined) {
        popup.showMessage("Select an pdf first!", "danger", ".upload-status-container", 5000)
        return;
    }

    //Add the pdf to the form data
    formData.append("pdf", pdf, pdf.name);

    // Create the request.
    let req = new Request(
        url.url() + url.getProjectName() + "/DocReviews/uploadPdf",
        {
            method: "POST",
            headers: h,
            body: formData,
        }
    );

    //execute the request
    let response
    try {
        response = await fetch(req);
        if (!response.ok) {
            //If the request failed show the error message on the page
            popup.showMessage("Something went wrong", "danger", ".upload-status-container", 5000)
            return
        }
    } catch (e){
        popup.showMessage(e, "danger", ".upload-status-container", 5000)
        return
    }
    

    //Show a status messgae
    popup.showMessage("PDF uploaded successfully", "success", ".upload-status-container", 5000)

    //Show the parsed content on the page
    const text = await response.text();
    setEditorHtml(text)
}

/**
 * @author Michiel Verschueren
 * Function to add the imported text to the page
 * @param {string} newHtml
 */
function setEditorHtml(newHtml) {
    //Get the container div
    const container = document.querySelector(".doc-review-content-wrapper")

    //Create a new Div element and set its attributes
    const newDiv = document.createElement("div")
    newDiv.id = "imported-content"
    newDiv.innerHTML = newHtml;

    //Add a hidden textArea to the new div to allow the text to be sent to the server using asp.net
    newDiv.innerHTML += `<textarea class="d-none" name="DocReviewText" id="DocReviewText">${newHtml}</textarea>`

    //Get the ckEditor instance and text Area
    const ckeditorTextArea = document.querySelector("#DocReviewText")
    const ckeditorInstance = CKEDITOR.instances["DocReviewText"];

    //If the ckeditor instance is not already destroyed, destroy it
    if (ckeditorInstance !== null && ckeditorInstance !== undefined) {
        ckeditorInstance.destroy(true);
        document.querySelector(".docreview-image-upload-wrapper").remove()
    } else {
        //Otherwise remove the container element of the previous imported content
        document.querySelector("#imported-content").remove()
    }

    //Add the new div to the container
    ckeditorTextArea.value = newHtml
    ckeditorTextArea.classList.add("d-none")
    container.appendChild(newDiv)
}

/**
 * @author Michiel Verschueren
 * Import from google docs
 * @returns {Promise<void>}
 */
async function importFromGoogleDocs() {
    //Close the popup
    popup.closePopup(".article-popup-import-help")

    //Show status message
    popup.showMessage("Importing...", "success", ".upload-status-container", -1)

    //Get the fileId
    const fileId = document.querySelector("#fileId").value

    //If the fileId is empty, show error message
    if (fileId === "" || fileId === null || fileId === undefined) {
        popup.showMessage("Input a fileId first!", "danger", ".upload-status-container", 5000)
        return;
    }

    //Make the request
    const response = await fetch(url.url() + url.getProjectName() + "/docreviews/import/" + fileId, {
        method: 'POST'
    })

    //If the response is not ok, show error message
    if (!response.ok) {
        popup.showMessage(await response.text(), "danger", ".upload-status-container", 5000)
        return;
    }

    //Get the response json
    const json = await response.json();

    //Show the status message
    popup.showMessage("Document imported successfully", "success", ".upload-status-container", 5000)

    //Set the editor html
    setEditorHtml(json.value)

}