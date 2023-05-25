/**
 * This function is executed when the user uploads a file (input type=file), and adds a preview of that images in the browser.
 * @author Niels Van Steen
 * @param previewElement the html element that will display the image the user just uploaded.
 * @param fileInput the input type file that contains the uploaded image.
 * */
export function addPreview(previewElement, fileInput) {
    let preview = document.querySelector(previewElement);
    let file = document.querySelector(fileInput).files[0];
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
}

/**
 * This function deletes the preview of the image that the user uploaded.
 * @author Niels Van Steen
 * @param previewElement the html element that displays the preview of the image.
 * */
export function deletePreview(previewElement) {
    let preview = document.querySelector(previewElement);
    preview.src = "";
    preview.style.display = "none";
}

/**
 * The banner image container will remain to have an aspect ratio of 16:9. This function updates the height according to the width.
 * @author Niels Van Steen
 * */
export function changeLandScapeImageContainerRatios(containerElements, wrapperElements) {
    const containers = document.querySelectorAll(containerElements);
    const wrappers = document.querySelectorAll(wrapperElements);

    for (let i = 0; i < containers.length; i++) {
        const container = containers[i];
        const width = container.clientWidth;
        container.style.height = (width / 16 * 9) + "px";
    } // For.

    for (let i = 0; i < wrappers.length; i++) {
        const wrapper = wrappers[i];
        const width = wrapper.clientWidth;
        wrapper.style.height = (width / 16 * 9) + "px";
    } // For.
} // changeLandScapeImageContainerRatios.
