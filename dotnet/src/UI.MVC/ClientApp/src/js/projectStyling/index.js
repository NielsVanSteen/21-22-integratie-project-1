import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";
import * as styleApplier from "./../shared/styleApplier.js";

window.addEventListener("load", init);

/**
 * Function executed on page load.
 *
 * @author Niels Van Steen
 * */
function init() {

    initializeCurSelectedStyle();

    const colorStyleButtons = document.querySelectorAll(".btn-color-style-selector");
    for (let i = 0; i < colorStyleButtons.length; i++) {
        const styleButton = colorStyleButtons[i];
        const id = styleButton.dataset.themeStyleId;
        styleButton.addEventListener("click", applyColorStyle);
        styleButton.id = id;
    }

    const btnSave = document.querySelector(".btn-save-style");
    btnSave.addEventListener("click", saveEdits);

    const btnCreate = document.querySelector(".btn-create-style");
    btnCreate.addEventListener("click", createStyle);

    initializeColorPreviews();

} // init.

const activeClass = "active-style-item";
let prevSelectedStyle;
let prevSelectedContainer;
let curId;

function initializeCurSelectedStyle() {
    // Get the previous selected id.
    prevSelectedContainer = document.querySelector("." + activeClass);
    prevSelectedStyle = prevSelectedContainer.dataset.themeStyleId;
}

/**
 * Event listener is executed when the user clicks a color style.
 *
 * @author Niels Van Steen
 * */
function applyColorStyle(evt) {
    const id = evt.currentTarget.id;

    initializeCurSelectedStyle();
    curId = id;

    // Remove the active class from the previous selected item and add it to the new one.
    prevSelectedContainer.classList.remove(activeClass);

    const newSelectedContainer = document.querySelector(".project-styles-item-container[data-theme-style-id='" + id + "']");
    newSelectedContainer.classList.add(activeClass);

}

/**
 * Saves the newly selected theme style.
 *
 * @author Niels Van Steen
 * */
async function saveEdits() {
    if (curId === undefined || curId == null) {
        popup.showMessage("This style is already the active one!", "warning", ".status-message-container", 5000);
        return;
    }

    let response = await fetch(url.url() + url.getProjectName() + "/ProjectStylings/Update/" + curId, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        }
    });

    if (response.status === 200) {
        popup.showMessage("Theme has been changed!", "success", ".status-message-container", 5000);
        await styleApplier.checkReloadStyles(true);
        styleApplier.applyStyles();

    } else {
        popup.showMessage("Theme could not be changed!", "danger", ".status-message-container", 5000);
    }
} // saveEdits.

function initializeColorPreviews() {
    const inputs = document.querySelectorAll(".input-color-style");
    for (let i = 0; i < inputs.length; i++) {
        const input = inputs[i];
        input.addEventListener("change", function () {
            const button = document.querySelector(".project-styles-color-item-button[data-index='" + i + "']");
            button.style.backgroundColor = input.value;
        });
    }
}

// create a new theme style.
async function createStyle() {

    const style = {
        genericName: document.querySelector(".input-generic-name").value,
        displayName: document.querySelector(".input-display-name").value,
        colorLight: document.querySelector(".color-1").value,
        colorMedium: document.querySelector(".color-2").value,
        colorDark: document.querySelector(".color-3").value,
        colorDarkest: document.querySelector(".color-4").value,
    }
    
    if (style.genericName.trim() === "" || style.displayName.trim() === ""){
        return popup.showMessage("Names can not be empty!", "danger", ".status-message-container", 5000);
    }

    const response = await fetch(url.url() + url.getProjectName() + "/ProjectStylings/CreateStyle/", {
        method: "POST",
        body: JSON.stringify(style),
        headers: {
            "Content-Type": "application/json"
        }
    });
    
    if (response.ok) {
        popup.showMessage("Theme has been created!", "success", ".status-message-container", 5000);
        await styleApplier.checkReloadStyles(true);
        styleApplier.applyStyles();
        setTimeout(() => {
            window.location.reload();
        }, 1000);
    } else {
        popup.showMessage("Theme could not be created!", "danger", ".status-message-container", 5000);
    }
    


} // createStyle.