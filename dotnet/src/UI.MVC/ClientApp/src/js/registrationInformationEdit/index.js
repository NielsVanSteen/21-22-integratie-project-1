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
    const btnCreate = document.querySelector(".btn-create-property");
    btnCreate.addEventListener("click", function () {
        popup.showPopup(".article-popup-create-property");
    });
    
    // Add event listener to create a new tag.
    const btnConfirmCreateTag = document.querySelector(".btn-confirm-create-property");
    btnConfirmCreateTag.addEventListener("click", createRecord);
    
    // Add event listener that deletes a specified record.
    const deleteIcons = document.querySelectorAll(".delete-property-icon");
    for (let i = 0; i< deleteIcons.length; i++) {
        // Get the icon that was clicked with it's dataset value.
        const deleteIcon = deleteIcons[i];
        const dataAttributeValue = deleteIcon.dataset.propertyId;
        
        // Create an event listener.
        deleteIcon.addEventListener("click", iconDeleteClicked);
        deleteIcon.propertyId = dataAttributeValue;
    } // For.
    
    // Add event listeners for saving the edits.
    const saveEditsIcons = document.querySelectorAll(".save-edit-property-icon-btn");
    for (let i=0; i<saveEditsIcons.length; i++) {
        const saveEditsIcon = saveEditsIcons[i];
        const id = saveEditsIcon.dataset.propertyId;
        
        // Create an event listener.
        saveEditsIcon.addEventListener("click", saveEdits);
        saveEditsIcon.propertyId = id;
    } // For.
    
} // init.

/**
 * Is invoked when the user presses the create button to create new registration field. This function sends a post request to create the field.
 * @author Niels Van Steen
 * */
function createRecord() {

    // Create the property.
    const userPropertyName = {
        userPropertyLabel: document.querySelector("#createPropertyName").value,
        description: document.querySelector("#createPropertyDescription").value,
        isRequired: document.querySelector("#selectCreateIsRequired").value === "required",
        userPropertyType: parseInt(document.querySelector("#selectCreateDataType").value),
        projectExternalName: url.getProjectName()
    };
    
    fetch(url.url() + url.getProjectName() + "/registrations/Create", {
        method: "POST",
        body: JSON.stringify(userPropertyName),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
        popup.closePopup(".article-popup-create-property");
        if (response.ok)
            return window.location.reload();
        else if (response.status === 409)
            return popup.showMessage("The field name must be unique", "danger", ".error-messages-container-list", 5000);
        else 
            return popup.showMessage("The field could not be created!", "danger", ".error-messages-container-list", 5000);
    })
    .catch(r => {
        popup.showMessage("The field could not be created!", "danger", ".error-messages-container-list", 5000);
    });
    
} // createTag

/**
 * Is invoked when the user clicks the 'save edits' icon.
 * @author Niels Van Steen
 * */
function saveEdits(evt) {
    const propertyId = evt.currentTarget.propertyId;
    
    // Select the section containing all the inputs via the tag id.
    const containerElement = document.querySelector("section[data-property-id='"+propertyId+"']")
   
    // Create the property.
    const userPropertyName = {
        userPropertyNameId: propertyId,
        userPropertyLabel: containerElement.querySelector("input[type=text]").value,
        description: containerElement.querySelector("textarea").value,
        isRequired: containerElement.querySelector(".select-is-required").value === "required",
        userPropertyType: parseInt(containerElement.querySelector(".select-property-data-type").value),
        projectExternalName: url.getProjectName()
    };
    
    // Create the tag.
    fetch(url.url() + url.getProjectName() + "/registrations/Update/"+propertyId, {
        method: "PUT",
        body: JSON.stringify(userPropertyName),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(response => {
      if (response.status === 200)
          return popup.showMessage("The field has been updated!", "success", ".error-messages-container-list", 5000);
      else if (response.status === 409)
          return popup.showMessage("The field name must be unique!", "danger", ".error-messages-container-list", 5000);
      else
          return popup.showMessage("Failed to update the field!", "danger", ".error-messages-container-list", 5000);
    }).catch(reason => popup.showMessage("Failed to update the field!", "danger", ".error-messages-container-list", 5000));
    
} // saveEdits.


/**
 * This function is executed when the user clicks the delete icon, it will show a popup asking if the user is sure.
 * @author Niels Van Steen
 * */
function iconDeleteClicked(evt) {
    const propertyId = evt.currentTarget.propertyId;
    
    // Create an event listener for the deleteTag button.
    const btnDeleteIcon = document.querySelector(".btn-confirm-delete-property");
    btnDeleteIcon.addEventListener("click", deleteTag);
    btnDeleteIcon.propertyId = propertyId;
   
    popup.showPopup(".article-popup-delete-property");
} // iconDeleteClicked.

/**
 * This function is invoked when the user presses 'delete' -> the tag will actually be deleted.
 * @author Niels Van Steen
 * */
function deleteTag(evt) {
    const propertyId = evt.currentTarget.propertyId;
    
    // Delete the tag.
    fetch(url.url() + url.getProjectName() + "/registrations/Delete/"+propertyId, {
        method: "DELETE"
    })
    .then(response => {
        if (response.status !== 204) {
            popup.errorDeletingRecord("Not Found", ".error-messages-container-list", ".article-popup-delete-property", "The Field could not be deleted", 5000);
        } else {
            popup.recordDeletedSuccessfully(propertyId, "data-property-id", ".article-popup-delete-property", "The Field has been deleted!", ".error-messages-container-list", 5000);
        }
    })
    .catch(reason => popup.errorDeletingRecord(reason, ".error-messages-container-list", ".article-popup-delete-property", "The Field could not be deleted", 5000));
    
} // deleteTag.




