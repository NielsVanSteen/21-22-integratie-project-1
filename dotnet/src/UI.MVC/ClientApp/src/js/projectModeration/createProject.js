// Execute on page load.
window.addEventListener("load", init);

import * as fileUpload from "./../shared/fileUpload.js";

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
function init() {
    // Add event listener that will be executed when an a project logo image is selected
    const projectLogoInput = document.querySelector("#projectLogo");
    projectLogoInput.addEventListener("change", function () {
        fileUpload.addPreview("#projectLogoImageDisplay", "#projectLogo")
    });

    // Add event listener that will be executed when an a project logo image is selected
    const projectBannerImageInput = document.querySelector("#projectBannerImage");
    projectBannerImageInput.addEventListener("change", function () {
        fileUpload.addPreview("#projectBannerImageDisplay", "#projectBannerImage")
    });
    
} // init.
