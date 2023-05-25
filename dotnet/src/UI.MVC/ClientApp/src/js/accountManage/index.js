// Execute on page load.
window.addEventListener("load", init);

import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
async function init() {

    popup.init();

    // Add event listener that will be executed on page resize.
    setProfilePictureContainerAspectRatio();
    window.addEventListener('resize', setProfilePictureContainerAspectRatio);

    // Add event listener that will be executed when an image is selected.
    const imageInput = document.querySelector("#profilePictureInput");
    imageInput.addEventListener("change", addPreviewImage);

    // Add event listener that will show to change profile picture popup window.
    const btnShowProfilePicturePopup = document.querySelector(".btn-open-profile-popup");
    btnShowProfilePicturePopup.addEventListener("click", function () {
        popup.showPopup(".article-change-profile-picture");
    });

    // Add the event listener to upload a profile picture.
    const btnSaveProfilePicture = document.querySelector(".button-save-profile-picture");
    btnSaveProfilePicture.addEventListener("click", uploadProfilePicture);

    const btnDeleteProfilePopup = document.querySelector(".btn-delete-profile");
    if (btnDeleteProfilePopup !== null) {
        btnDeleteProfilePopup.addEventListener("click", () => {
            popup.showPopup(".article-delete-profile");
        });

        const btnDeleteProfile = document.querySelector(".button-delete-profile");
        btnDeleteProfile.addEventListener("click", await deleteProfile);
    }

    document.querySelector(".btn-toggle-input-file").addEventListener("click", function () {
        document.querySelector("#profilePictureInput").click();
    });

    // Add event listener that deletes the user's profile picture.
    const iconDeleteProfilePicture = document.querySelector("#delete-profile-picture");
    iconDeleteProfilePicture.addEventListener("click", deleteProfilePicture);

} // init.

/**
 * Function that sends an http request to update the user's profile picture.
 * @author Niels Van Steen
 * */
function uploadProfilePicture() {
    // Define the header.
    let h = new Headers();
    h.append('Accept', 'application/json');

    // Construct the form data.
    let formData = new FormData();
    let image = document.querySelector("#profilePictureInput").files[0];
    const errorMessagePopupContainer = document.querySelector("#profilePictureValidation");

    if (image === null || image === undefined) {
        errorMessagePopupContainer.innerHTML = "Select an image first!";
        return;
    }

    formData.append("image", image, image.name);

    // Create the request.
    let req = new Request(url.url() + url.getProjectName() + "/Accounts/Upload", {
        method: "POST",
        headers: h,
        mode: 'no-cors',
        body: formData
    });

    // Send the request.
    fetch(req)
        .then(async response => {
            if (response.ok) {
                popup.closePopup(".article-change-profile-picture");
                const link = await response.json();
                let headerProfileIcon = document.querySelector(".header-profile-icon");
                headerProfileIcon.src = link;
                let profilePicture = document.querySelector(".profile-picture");
                profilePicture.src = link;
            } else {
                errorMessagePopupContainer.innerHTML = await response.json();
            }
        })
        .catch(r => {
            errorMessagePopupContainer.innerHTML = "This File extension is not allowed!";
        });
} // uploadProfilePicture

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
function setProfilePictureContainerAspectRatio() {
    let container = document.querySelector('.profile-picture-preview-figure');
    const width = container.clientWidth;

    container.style.height = width + "px";
} // setProfilePictureContainerAspectRatio.

// This function shows the image that has been uploaded as a preview.
function addPreviewImage() {

    let preview = document.querySelector('#change-profile-picture-preview');
    let file = document.querySelector('#profilePictureInput').files[0];
    let reader = new FileReader();

    preview.style.display = "block";

    reader.onloadend = function () {
        preview.src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.src = "";
    }
} // addPreviewImage.

/**
 * Send an HTTP DELETE request to remove the user's profile picture.
 * @author Niels Van Steen
 * */
async function deleteProfilePicture() {
    await fetch(url.url() + url.getProjectName() + "/Accounts/DeleteProfilePicture", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    })
    // Close the popup.
    popup.closePopup(".article-popup");


    let headerProfileIcon = document.querySelector(".header-profile-icon");
    headerProfileIcon.src = "/images/icons/profile.png"
    let profilePicture = document.querySelector(".profile-picture");
    profilePicture.src = "/images/icons/profile.png"
} // deleteProfilePicture.

async function deleteProfile() {
     const request = await fetch(url.url() + url.getProjectName() + "/Accounts/DeleteProfile", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        }
    });

    const response = await request;

    if (response.ok) {
          popup.closePopup(".article-delete-profile");
          window.location.href = `/${url.getProjectName()}/Account/Register`;
    } else {
        document.querySelector(".article-delete-profile .popup-body").innerHTML = `<p class="alert alert-danger">Uw profiel kon niet verwijderd worden! Probeer het later opnieuw.</p>`;
    }
} // deleteProfile.