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
    const btnDeleteMods = document.querySelectorAll(".btn-delete-moderator");
    for (let i = 0; i< btnDeleteMods.length; i++) {
        
        const btnDeleteMod = btnDeleteMods[i];
        const modId = btnDeleteMod.dataset.moderatorId;

        // Create an event listener.
        btnDeleteMod.addEventListener("click", btnDeleteModClicked);
        btnDeleteMod.moderatorId = modId;
    } // For.
    
} // init.


/**
 * Show the popup to delete a moderator. and attach the event listener to the delete button.
 * @author Niels Van Steen
 * */
function btnDeleteModClicked(evt) {
    const moderatorId = evt.currentTarget.moderatorId;
    
    // Create an event listener for the deleteTag button.
    const btnDeleteMod = document.querySelector(".btn-confirm-delete-moderator");
    btnDeleteMod.addEventListener("click", deleteModerator);
    btnDeleteMod.moderatorId = moderatorId;
    
    popup.showPopup(".article-popup-delete-moderator");
} // iconDeleteClicked.

/**
 * Method is invoked when the user presses the delete button in the popup and confirms the removal of a moderator.
 * This function sends an http delete request which will remove the moderator from the database.
 * @author Niels Van Steen
 * */
function deleteModerator(evt) {
    const moderatorId = evt.currentTarget.moderatorId;

    // Delete the tag.
    fetch(url.url() + url.getProjectName() + "/ProjectsModeration/DeleteModerator/"+moderatorId, {
        method: "DELETE"
    })
        .then(response => {
            if (response.status !== 204) {
                popup.errorDeletingRecord("Not Found", ".error-messages-container-list", ".article-popup-delete-moderator", "The User could not be deleted", 5000);
            } else {
                popup.recordDeletedSuccessfully(moderatorId, "data-moderator-id", ".article-popup-delete-moderator", "The User has been deleted!", ".error-messages-container-list", 5000);
            }
        })
        .catch(reason => popup.errorDeletingRecord(reason, ".error-messages-container-list", ".article-popup-delete-moderator", "The User could not be deleted", 5000));
} // deleteModerator.