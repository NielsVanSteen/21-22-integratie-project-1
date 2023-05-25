import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";
import {getProjectName} from "./../shared/url.js";

// Execute on page load.
window.addEventListener("load", init);

/**
* Function invoked at page load, used to add event listeners.
* @author Niels Van Steen
* */
function init() {
    // event listener for the search projects button.
    const btnSearchProject = document.querySelector("#btnSearchProject");
    btnSearchProject.addEventListener("click", function () {
        getProjects(false);
    });
    
    // Add event listener that will toggle the visibility of the assigned projects (admins are not assigned to projects).
    const selectRole = document.querySelector("#selectRole");
    if (selectRole !== undefined && selectRole !== null)
        selectRole.addEventListener("change", function() {checkToggleProjects(selectRole)})

    // The the event listeners for the delete icons.
    addDeleteButtonEventListeners();
    
    // Event listener to close the search box result.
    const btnCloseSearch = document.querySelector("#close-search-results");
    btnCloseSearch.addEventListener("click", function() {toggleSearchBox("none")});
} // init.

/**
 * Checks the selected value of the select for the roles. and check whether to hide or show the assigned projects article.
 * @author Niels Van Steen
 * */
function checkToggleProjects(selectRole) {
    const value = selectRole.value;
    let article = document.querySelector(".assigned-project-wrapper");
    article.style.display = parseInt(value) === 1 ? "block" : "none";
} // checkToggleProjects.

// Adds the event listeners for the buttons to add an assigned project. This method is invoked after the user searches for projects.

/**
 * Adds the event listeners for the add buttons inside the search result box. so the user can add projects to the account they're editing.
 * @author Niels Van Steen
 * */
function addEventListenersAddProject() {
    // Add the event listener for the add buttons.
    const addProjectButtons = document.querySelectorAll(".btn-add-project");
    
    for (let i=0; i<addProjectButtons.length; i++) {
        let addProjectButton = addProjectButtons[i];
        let projectId = addProjectButton.dataset.projectId;
        addProjectButton.addEventListener("click", function () {
            addAssignedProject(projectId);
        })
    }
} // addEventListenersAddProject.

/**
 * Get all the projects matching the search criteria.
 * @author Niels Van Steen
 * */
function getProjects() {
    const name = document.querySelector("#inputProjectName").value;
    const sortOrder = "Ascending";

    fetch(`${url.url()}${url.getProjectName()}/ProjectsModeration/GetProjectByFilter?name=${name}&sortOrder=${sortOrder}`, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
    })
    .then(response => response.json())
    .then(data => showProjects(data, name))
    .catch(error => popup.showMessage("Projects Could not be retrieved!", "danger", ".error-messages-container-list", 5000));
} // getProjects

// Closes the search box window.
function toggleSearchBox(mode) {
    // Close the search field.
    let searchContainer = document.querySelector(".projects-search-container");
    searchContainer.style.display = mode;
} // toggleSearchBox.

/**
 * Shows the projects in the search box dialog window.
 * @author Niels Van Steen
 * @param projects a list of projects the search yielded.
 * @param name the input of the search box the user typed.
 * */
function showProjects(projects, name) {
    let container = document.querySelector("#projectSearchResultContainer");
    container.innerHTML = "";
    
    // Make the results in the search box visible.
    toggleSearchBox("block");

    let assignedProjectsList = document.querySelector(".assigned-projects-list");
    const addedProjects = assignedProjectsList.querySelectorAll("input[type=text]")
    let addedProjectIds = [];
    for (let e = 0; e<addedProjects.length; e++)
        addedProjectIds.push(parseInt(addedProjects[e].value));
    
    let counter = 0;
    
    // Add all the projects.
    for (let i=0; i<projects.length; i++) {
        const project = projects[i];
        
        // Don't show the already added projects.
        if (addedProjectIds.includes(parseInt(project.projectId)))
            continue;
        counter++;
        
        container.innerHTML += ' ' +
        '           <li class="outer-li">' +
        '                <ul class="ul-single-project-search">' +
        '                    <li class="inner-li">'+ project.externalName +'</li>' +
        '                    <li>'+ project.internalName +'</li>' +
        '                    <li>' +
        '                        <button class="btn btn-success btn-add-project" data-project-id="'+project.projectId+'" type="button" onclick="event.preventDefault()">Add</button>' +
        '                   </li>' +
        '               </ul>\n' +
        '           </li>';
    } // For.
    
    const text = name === "" ? "All Projects have been added!" : "No results!";
    if (counter === 0) {
        container.innerHTML = "<p class='text-danger'>"+text+"</p>";
    }
    
    // Check the search yielded no results, if so display a correct messages.
    if (projects.length < 1) {
        container.innerHTML = '<li class="outer-li text-danger">No results</li>'
    }
    
    // Add the event listeners for the projects in the search box.
    addEventListenersAddProject();
} // showProjects.


/**
 * When the user clicks the add button in the search dialog this method is invoked and adds the projects.
 * @author Niels Van Steen
 * @param projectId The id of the project that the account will be registered for.
 * */
async function addAssignedProject(projectId) {
    let assignedProjectsList = document.querySelector(".assigned-projects-list");
    
    await fetch(`${url.url()}${url.getProjectName()}/Projects/${projectId}`, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
    })
    .then(response => response.json())
    .then(project => {
        assignedProjectsList.innerHTML += '' +
            '               <section class="assigned-project-item" data-project-id="'+parseInt(project.projectId)+'">' +
            '                    <figure>' +
            '                        <img src="'+project.bannerImageUrl+'" alt="Project Banner image">' +
            '                        <figcaption>'+ project.externalName +'</figcaption>' +
            '                    </figure>' +
            '                    <input type="text" name="AssignedProjectIds" class="hidden" value="'+project.projectId+'">' +
            '                    <button class="button-delete-icon" type="button" data-project-id="'+project.projectId+'"><i class="fa-solid fa-trash-can"></i></button>' +
            '                </section>';
    })
    .catch(error => popup.showMessage("Projects Could not be retrieved!", "danger", ".error-messages-container-list", 5000));
    
    getProjects(true);
    
    // The the event listeners for the delete icons.
    addDeleteButtonEventListeners();
    
} // addAssignedProject.

/**
 * Add the event listeners for the delete icons.
 * @author Niels Van Steen
 * */
function addDeleteButtonEventListeners() {
    // Add the remove event listeners.
    let deleteButtons = document.querySelectorAll(".button-delete-icon");
    let projectSections = document.querySelectorAll(".assigned-project-item");

    for (let i=0; i<deleteButtons.length; i++) {

        deleteButtons[i].addEventListener("click", function() {
            projectSections[i].remove();
        });
    }
}