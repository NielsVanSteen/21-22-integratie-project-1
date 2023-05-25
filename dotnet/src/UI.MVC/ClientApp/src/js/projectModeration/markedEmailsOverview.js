import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

// Execute on page load.
window.addEventListener("load", init);

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
function init() {

    // Add event listeners for the popup window.
    popup.init();
    
    // Add event listeners to delete moderators.
    const btnDeleteMarkedEmails = document.querySelectorAll(".btn-delete-marked-email");
    for (let i = 0; i< btnDeleteMarkedEmails.length; i++) {
        
        const btnDeleteMarkedEmail = btnDeleteMarkedEmails[i];
        const id = btnDeleteMarkedEmail.dataset.markedEmailId;

        // Create an event listener.
        btnDeleteMarkedEmail.addEventListener("click", btnDeleteMarkedEmailClicked);
        btnDeleteMarkedEmail.markedEmailId = id;
    } // For.
    
} // init.

/**
 * Show the popup to delete a moderator. and attach the event listener to the delete button.
 * @author Niels Van Steen
 * */
function btnDeleteMarkedEmailClicked(evt) {
    const id = evt.currentTarget.markedEmailId;
    
    // Create an event listener for the deleteTag button.
    const btnDeleteMarkedEmail = document.querySelector(".btn-confirm-delete-marked-email");
    btnDeleteMarkedEmail.addEventListener("click", deleteMarkedEmail);
    btnDeleteMarkedEmail.markedEmailId = id;
    
    popup.showPopup(".article-popup-delete-marked-email");
} // iconDeleteClicked.

/**
 * Method is invoked when the user presses the delete button in the popup and confirms the removal of a marked-email.
 * This function sends an http delete request which will remove the marked-email from the database.
 * @author Niels Van Steen
 * */
function deleteMarkedEmail(evt) {
    const id = evt.currentTarget.markedEmailId;

    // Delete the tag.
    fetch(url.url() + url.getProjectName() + "/ProjectsModeration/DeleteMarkedEmail/"+id, {
        method: "DELETE"
    })
        .then(response => {
            if (response.status !== 204) {
                popup.errorDeletingRecord("Not Found", ".error-messages-container-list-marked-emails", ".article-popup-delete-marked-email", "The marked email could not be deleted", 5000);
            } else {
                popup.recordDeletedSuccessfully(id, "data-marked-email-id", ".article-popup-delete-marked-email", "The marked email has been deleted!", ".error-messages-container-list-marked-emails", 5000);
            }
        })
        .catch(reason => popup.errorDeletingRecord(reason, ".error-messages-container-list-marked-emails", ".article-popup-delete-marked-email", "The marked-email could not be deleted", 5000));
} // deleteMarkedEmail.