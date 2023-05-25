window.addEventListener("load", init);
import * as fileUpload from "./../shared/fileUpload.js";
import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

/**
 * init function executed on page load, adds all event listeners.
 * @author Niels Van Steen
 * */
function init() {
    popup.init();

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

    // add event listener to open the popup to archive a doc-review.
    const buttonsArchive = document.querySelectorAll(".btn-archive-doc-review");
    for (let i = 0; i < buttonsArchive.length; i++) {
        const btnArchive = buttonsArchive[i];
        const docReviewId = btnArchive.dataset.docReviewId;
        const isArchived = btnArchive.dataset.isArchived;

        // Create an event listener.
        btnArchive.addEventListener("click", btnArchiveClicked);
        btnArchive.docReviewId = docReviewId;
        btnArchive.isArchived = isArchived;
    } // For.

    // Add event listeners to delete a doc-review.
    const buttonsDelete = document.querySelectorAll(".btn-delete-doc-review");
    for (let i = 0; i < buttonsDelete.length; i++) {
        const btnDelete = buttonsDelete[i];
        const docReviewId = btnDelete.dataset.docReviewId;

        // Create an event listener.
        btnDelete.addEventListener("click", btnDeleteClicked);
        btnDelete.docReviewId = docReviewId;
    } // For.

    // Add event listeners to close a doc-review for coments.
    const buttonsCloseComments = document.querySelectorAll(".btn-close-comments-doc-review");
    for (let i = 0; i < buttonsCloseComments.length; i++) {
        const btnClose = buttonsCloseComments[i];
        const docReviewId = btnClose.dataset.docReviewId;

        // Create an event listener.
        btnClose.addEventListener("click", btnCloseClicked);
        btnClose.docReviewId = docReviewId;
    } // For.
    
      // Add event listeners to close a doc-review for coments.
    const buttonsPublish = document.querySelectorAll(".btn-publish-doc-review");
    for (let i = 0; i < buttonsPublish.length; i++) {
        const btnPublish = buttonsPublish[i];
        const docReviewId = btnPublish.dataset.docReviewId;

        // Create an event listener.
        btnPublish.addEventListener("click", btnPublishClicked);
        btnPublish.docReviewId = docReviewId;
    } // For.

} // init.

/**
 * shows the popup window to archive a doc-review. and adds the event listener for the confirm archive.
 * @author Niels Van Steen
 * */
function btnArchiveClicked(evt) {
    const docReviewId = evt.currentTarget.docReviewId;
    const isArchived = evt.currentTarget.isArchived;

    if (isArchived === 'true') {
        const popupEl = document.querySelector(".article-popup-archive-doc-review");
        popupEl.querySelector("h2").innerHTML = "Un-Archive Project";
        popupEl.querySelector(".btn-confirm-archive-doc-review").innerHTML = "Un-archive";
        popupEl.querySelector(".alert-warning").innerHTML += "Are you sure you want to un-archive? It will be visible again for normal users!"
    }

    // Create an event listener for the deleteTag button.
    const btnArchive = document.querySelector(".btn-confirm-archive-doc-review");
    btnArchive.addEventListener("click", archiveDocReview);
    btnArchive.docReviewId = docReviewId;

    popup.showPopup(".article-popup-archive-doc-review");
} // iconDeleteClicked.

/**
 * shows the popup window to delete a doc-review. and adds the event listener for the confirm delete.
 * @author Niels Van Steen
 * */
function btnDeleteClicked(evt) {
    const docReviewId = evt.currentTarget.docReviewId;

    // Create an event listener for the deleteTag button.
    const btnDelete = document.querySelector(".btn-confirm-delete-doc-review");
    btnDelete.addEventListener("click", deleteDocReview);
    btnDelete.docReviewId = docReviewId;

    popup.showPopup(".article-popup-delete-doc-review");
} // btnDeleteClicked.

function btnCloseClicked(evt) {
    const docReviewId = evt.currentTarget.docReviewId;

    // Create an event listener for the deleteTag button.
    const btnClose = document.querySelector(".btn-confirm-close-comments-doc-review");
    btnClose.addEventListener("click", closeDocReviewForComments);
    btnClose.docReviewId = docReviewId;

    popup.showPopup(".article-popup-close-comments-doc-review");
} // btnDeleteClicked.

function btnPublishClicked(evt) {
    const docReviewId = evt.currentTarget.docReviewId;

    // Create an event listener for the publish button.
    const btnPublish = document.querySelector(".btn-confirm-publish-doc-review");
    btnPublish.addEventListener("click", publishDocReview);
    btnPublish.docReviewId = docReviewId;

    popup.showPopup(".article-popup-publish-doc-review");
} // btnDeleteClicked.

/**
 * archives a doc-review
 * @author Niels Van Steen, Michiel Verschueren.
 * */
async function archiveDocReview(evt) {
    const docReviewId = evt.currentTarget.docReviewId;

    let response = await fetch(url.url() + url.getProjectName() + "/DocReviews/ArchiveDocReview/" + docReviewId, {
        method: "POST",
    });
    
    popup.closePopup(".article-popup-archive-doc-review");

    if (response.ok){
        popup.showMessage("DocReview archived", "success", // type is either: danger or success!
            ".status-messages-container", 5000);
        setTimeout("location.reload(true);", 3000);
    } else {
        popup.showMessage(await response.text(), "danger", // type is either: danger or success!
            ".status-messages-container", 5000);
    }
    
} // btnArchive.

/**
 * Deletes a doc-review
 * @author Niels Van Steen, Michiel Verschueren.
 * */
async function deleteDocReview(evt) {
    const docReviewId = evt.currentTarget.docReviewId;

    let response = await fetch(url.url() + url.getProjectName() + "/DocReviews/DeleteDocReview/" + docReviewId,{
        method: "POST",
    });
    
    popup.closePopup(".article-popup-delete-doc-review");

    document.querySelector(".list-item-wrapper[data-doc-review-id='"+docReviewId+"']").remove();

    if (response.ok){
        popup.showMessage("DocReview deleted", "success", // type is either: danger or success!
            ".status-messages-container", 5000);
        setTimeout("location.reload(true);", 3000);
    } else{
        popup.showMessage(await response.text(), "danger", // type is either: danger or success!
            ".status-messages-container", 5000);
    }

    let data = "Delete" + docReviewId;
    document.querySelector(".btn-link-hover-container[data-type='"+data+"']").remove();
    
} // deleteDocReview.

/**
 * closes a doc-review for comments.
 * @author Niels Van Steen, Michiel Verschueren.
 * */
async function closeDocReviewForComments(evt) {
    const docReviewId = evt.currentTarget.docReviewId;
    
    let response = await fetch(url.url() + url.getProjectName() + "/DocReviews/UpdateDocReviewClosed/" + docReviewId,{
        method: "PUT",
    });
    
    popup.closePopup(".article-popup-close-comments-doc-review");
    
    if (response.ok){
        popup.showMessage("DocReview closed for comments", "success", // type is either: danger or success!
            ".status-messages-container", 5000);
        setTimeout("location.reload(true);", 3000);
    } else{
        popup.showMessage(await response.text(), "danger", // type is either: danger or success!
            ".status-messages-container", 5000);
    }
    let data = "Close" + docReviewId;
    document.querySelector(".btn-link-hover-container[data-type='"+data+"']").remove();
    
} // deleteDocReview.

/**
 * publishes a docReview
 * @author Niels Van Steen, Michiel Verschueren
 */
async function publishDocReview(evt){
    const docReviewId = evt.currentTarget.docReviewId;
    
    let response = await fetch(url.url() + url.getProjectName() + "/DocReviews/PublishDocReview/" + docReviewId,{
        method: "PUT",
    });

    popup.closePopup(".article-popup-publish-doc-review");

    if (response.ok){
        popup.showMessage("DocReview published", "success", // type is either: danger or success!
            ".status-messages-container", 5000);
        setTimeout("location.reload(true);", 3000);
    } else{
        popup.showMessage(await response.text(), "danger", // type is either: danger or success!
            ".status-messages-container", 5000);
    }
    let data = "Publish" + docReviewId;
    document.querySelector(".btn-link-hover-container[data-type='"+data+"']").remove();
}


