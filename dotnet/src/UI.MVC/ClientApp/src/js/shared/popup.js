/**
 * Init function executed on page load, which adds the event listeners to close a popup window.
 * @author Niels Van Steen
 * */
export function init() {
    // Add event listeners that will close the popup window.
    const closeIcons = document.querySelectorAll(".close-popup-icon");
    const btnsCancel = document.querySelectorAll(".btn-cancel-popup");

    for (let i = 0; i < closeIcons.length; i++) {
        const closeIcon = closeIcons[i];
        const dataClass = closeIcon.dataset.closeClass;

        closeIcon.addEventListener("click", function () {
            closePopup(dataClass);
        });
    }
    for (let i = 0; i < btnsCancel.length; i++) {
        const btnCancel = btnsCancel[i];
        const dataClass = btnCancel.dataset.closeClass;

        btnCancel.addEventListener("click", function () {
            closePopup(dataClass)
        });
    }

}

/**
 * this function closes a popup window.
 * @author Niels Van Steen
 * @param nameOfElement the class WITH . in front of the html container element of the popup window.
 * */
export function closePopup(nameOfElement) {
    let popupContainer = document.querySelector(nameOfElement);
    
    // Allow scrolling again.
    document.querySelector("body").style.overflow = "auto";

    // Animation is done with css transitions!
    popupContainer.style.top = "0px";
    popupContainer.style.opacity = 0;

    // Remove the darker background.
    let popupBackground = document.querySelector(nameOfElement + "-background-wrapper");
    popupBackground.style.opacity = 0;

    // After the animation is done set display to none.
    setTimeout(function () {
        //popupContainer.style.display = "none";
        popupContainer.style.transition = "";
        popupContainer.style.left = "-1000px";
        popupContainer.style.visibility = "hidden";
        
        popupBackground.style.visibility = "hidden";
    }, 500);
}

/**
 * this function shows a popup window.
 * @author Niels Van Steen
 * @param nameOfElement the class WITH . in front of the html container element of the popup window.
 * */
export function showPopup(nameOfElement) {
    let popupContainer = document.querySelector(nameOfElement);
    
    // Disable scrolling when popup is open.
    document.querySelector("body").style.overflow = "hidden";

    // Animate the popup window.
    popupContainer.style.left = "50%";
    popupContainer.style.transition = "top 400ms ease, opacity 500ms ease";
    popupContainer.style.visibility = "visible";
    //popupContainer.style.display = "block";
    popupContainer.style.top = "100px";
    popupContainer.style.opacity = 1;

    // Make the background darker and don't allow the user to click anywhere else.
    let popupBackground = document.querySelector(nameOfElement + "-background-wrapper");
    popupBackground.style.visibility = "visible";
    popupBackground.style.opacity = 1;
}

/**
 * Shows the popup window for a given duration of time.
 * @author Niels Van Steen
 * */
export function quickShowPopup(nameOfElement, duration) {
    showPopup(nameOfElement);
    setTimeout(function () {
        closePopup(nameOfElement);
    }, duration);
}

// Invoked when the tag has been deleted.
export function recordDeletedSuccessfully(id, dataAttribute, popupContainerClass, message, errorMessageBodyContainerClass, messageLength) {

    // Remove the element that has just been deleted (less resource intensive than reloading all the tags) + when the page reloads the tags will load again from the db.
    const containerOfTag = document.querySelector('section[' + dataAttribute + '="' + id + '"]');
    containerOfTag.remove();
    closePopup(popupContainerClass);
    showMessage(message, "success", errorMessageBodyContainerClass, messageLength)
}

// Invoked when the tag couldn't be deleted.
export function errorDeletingRecord(reason, errorMessageBodyContainerClass, popupContainerClass, message, messageLength) {

    const errorContainer = document.querySelector(".popup-body-error-message-container");
    errorContainer.innerHTML = "<p class='alert alert-danger'>message</p>";
    showMessage(message, "danger", errorMessageBodyContainerClass, messageLength)

    setTimeout(function () {
        errorContainer.innerHTML = "";
        popup.closePopup(popupContainerClass);
    }, messageLength);
}

// Shows a success/error message.
export function showMessage(message, type, errorMessageBodyContainerClass, length) {
    const container = document.querySelector(errorMessageBodyContainerClass);
    container.innerHTML = "<p class='alert alert-" + type + "'>" + message + "</p>";

    if (length < 0)
        return;

    setTimeout(function () {
        container.innerHTML = "";
    }, length);
} // showMessage.

export function deleteMessage(errorMessageBodyContainerClass) {
    const container = document.querySelector(errorMessageBodyContainerClass);
    container.innerHTML = "";
}
