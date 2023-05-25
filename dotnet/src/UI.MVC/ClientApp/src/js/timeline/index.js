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

    let deleteButtons = document.querySelectorAll(".btn-delete-phase");
    for (let i = 0; i < deleteButtons.length; i++) {
        const btnDelete = deleteButtons[i];
        const id = btnDelete.dataset.phaseId;
        btnDelete.addEventListener("click", askDeletePhase);
        btnDelete.id = id;
    }
} // init.

function askDeletePhase(evt) {
    const id = evt.currentTarget.id;
    popup.showPopup(".article-popup-delete-phase");

    const btnDelete = document.querySelector(".btn-confirm-delete-phase");
    btnDelete.addEventListener("click", deletePhase);
    btnDelete.id = id;

} // askDeletePhase.

function deletePhase(evt) {
    const id = evt.currentTarget.id;

    // Delete the tag.
    fetch(url.url() + url.getProjectName() + "/TimeLines/DeletePhase/" + id, {
        method: "DELETE"
    })
        .then(response => {
            if (response.status !== 204) {
                popup.showMessage("The phase could not be deleted!", "danger", ".status-messages-container", 5000);
            } else {
                popup.showMessage("The phase has been deleted!", "success", ".status-messages-container", 5000);
            }
        })
        .catch(() => popup.showMessage("The phase could not be deleted!", "danger", ".status-messages-container", 5000));

    popup.closePopup(".article-popup-delete-phase");

    // Delete the existing.
    const container = document.querySelector(".timeline-phase-edit-container[data-phase-id='" + id + "']");
    if (container != null)
        container.remove();

    const containerElement = document.querySelector(".timeline-phases-edits-wrapper");

    if (!containerElement.firstChild) {
        document.querySelector(".no-phases-yet").innerHTML = `<p class="alert alert-warning">There are no phases yet. Create the first!</p>`;
    }
} // deletePhase.