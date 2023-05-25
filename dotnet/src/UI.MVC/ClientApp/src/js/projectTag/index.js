import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";


// Execute on page load.
window.addEventListener("load", init);

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
function init() {

    // Add event listeners that will close the popup window.
    popup.init();
    
    // Add event listener to show to popup to create a new tag..
    const btnCreateTag = document.querySelector(".btn-create-tag");
    btnCreateTag.addEventListener("click", function () {
        popup.showPopup(".article-popup-create-tag");
    });
    
    // Add event listener to create a new tag.
    const btnConfirmCreateTag = document.querySelector(".btn-confirm-create-tag");
    btnConfirmCreateTag.addEventListener("click", createTag);
    
    // Add event listener that deletes the user's profile picture.
    const deleteTagIcons = document.querySelectorAll(".delete-tag-icon");
    for (let i = 0; i< deleteTagIcons.length; i++) {
        // Get the icon that was clicked with it's dataset value.
        const tagIcon = deleteTagIcons[i];
        const dataAttributeValue = tagIcon.dataset.tagId;
        
        // Create an event listener.
        tagIcon.addEventListener("click", iconDeleteClicked);
        tagIcon.tagId = dataAttributeValue;
    } // For.
    
    // Add event listeners for saving the edits.
    const saveEditsIcons = document.querySelectorAll(".save-edit-tag-icon");
    for (let i=0; i<saveEditsIcons.length; i++) {
        const saveEditsIcon = saveEditsIcons[i];
        const id = saveEditsIcon.dataset.tagId;
        
        // Create an event listener.
        saveEditsIcon.addEventListener("click", saveEdits);
        saveEditsIcon.tagId = id;
    } // For.
    
    // Add event listeners that will fire when the color input changes.
    const tagContainers = document.querySelectorAll(".project-tag-item-list");
   
    for (let i=0; i<tagContainers.length; i++) {
        const tagContainer = tagContainers[i];
        
        // Create an event listener.
        const inputPContainer = tagContainer.querySelector(".project-list-name");
        const selectWhiteText = tagContainer.querySelector(".select-text-color");
        let inputColor = tagContainer.querySelector("input[type=color]");
        let inputText = tagContainer.querySelector("input[type=text]");
        
        selectWhiteText.addEventListener("change", function () {
            inputPContainer.style.color = selectWhiteText.value;
            inputText.style.color = selectWhiteText.value;
        });
        
        inputColor.addEventListener("change", function () {
            inputPContainer.style.backgroundColor = inputColor.value;
        });
    } // For.
} // init.

/**
 * Function is executed when the user pressed the create button in the popup windows. 
 * This function will take all the input field data, create a tag object and send a POST request to create the tag.
 * @author Niels Van Steen
 * */
function createTag() {

    // Create the tag.
    const tag = {
        projectExternalName: url.getProjectName(),
        name: document.querySelector("#createTagName").value,
        color: document.querySelector("#createTagColor").value,
        isPublic: document.querySelector("#createTagPublic").value === "public",
        isTextWhite: document.querySelector("#createTagTextColor").value === "white"
    }

    fetch(url.url() + url.getProjectName() + "/ProjectTags", {
        method: "POST",
        body: JSON.stringify(tag),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        popup.closePopup(".article-popup-create-tag");
        if (response.ok)
            return window.location.reload();
        else if (response.status === 409)
            return popup.showMessage("Tag name must be unique", "danger", ".error-messages-container-list", 5000);
        else 
            return popup.showMessage("Tag could not be created!", "danger", ".error-messages-container-list", 5000);
    })
    .catch(() => {
        popup.showMessage("Tag could not be created!", "danger", ".error-messages-container-list", 5000);
    });
    
} // createTag

/**
 * Is invoked when the user clicks the 'save edits' icon, and updates the project tag with a http put request.
 * @author Niels Van Steen
 * */
function saveEdits(evt) {
    const tagId = evt.currentTarget.tagId;
    
    // Select the section containing all the inputs via the tag id.
    const containerElement = document.querySelector("section[data-tag-id='"+tagId+"']")
   
    // Create the tag.
    const tag = {
        projectTagId: tagId,
        name: containerElement.querySelector("input[type=text]").value,
        color: containerElement.querySelector("input[type=color]").value,
        isPublic: containerElement.querySelector(".select-public").value === "public",
        isTextWhite: containerElement.querySelector(".select-text-color").value === "white"
    }
    
    // Create the tag.
    fetch(url.url() + url.getProjectName() + "/ProjectTags/Update/"+tagId, {
        method: "PUT",
        body: JSON.stringify(tag),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
      if (response.status === 200)
          return popup.showMessage("Tag has been updated!", "success", ".error-messages-container-list", 5000);
      else if (response.status === 409)
          return popup.showMessage("Tag name must be unique!", "danger", ".error-messages-container-list", 5000);
      else
          return popup.showMessage("Failed to update the tag!", "danger", ".error-messages-container-list", 5000);
    }).catch(reason => popup.showMessage("Failed to update the tag!", "danger", ".error-messages-container-list", 5000));
    
} // saveEdits.



/**
 * This function is executed when the user clicks the delete icon, it will show a popup asking if the user is sure.
 * @author Niels Van Steen
 * */
function iconDeleteClicked(evt) {
    const tagId = evt.currentTarget.tagId;
    
    // Create an event listener for the deleteTag button.
    const btnDeleteIcon = document.querySelector(".btn-confirm-delete-tag");
    btnDeleteIcon.addEventListener("click", deleteTag);
    btnDeleteIcon.tagId = tagId;
   
    popup.showPopup(".article-popup-delete-tag");
} // iconDeleteClicked.

/**
 * // This function is invoked when the user presses 'delete' -> the tag will actually be deleted.
 * @author Niels Van Steen
 * */
function deleteTag(evt) {
    const tagId = evt.currentTarget.tagId;
    
    // Delete the tag.
    fetch(url.url() + url.getProjectName() + "/ProjectTags/Delete/"+tagId, {
        method: "DELETE"
    })
    .then(response => {
        if (response.status !== 204) {
            popup.errorDeletingRecord("Not Found", ".error-messages-container-list", ".article-popup-delete-tag", "The Tag could not be deleted", 5000);
        } else {
            popup.recordDeletedSuccessfully(tagId, "data-tag-id", ".article-popup-delete-tag", "The tag has been deleted!", ".error-messages-container-list", 5000);
        }
    })
    .catch(reason => popup.errorDeletingRecord(reason, ".error-messages-container-list", ".article-popup-delete-tag", "The Tag could not be deleted", 5000));
} // deleteTag.