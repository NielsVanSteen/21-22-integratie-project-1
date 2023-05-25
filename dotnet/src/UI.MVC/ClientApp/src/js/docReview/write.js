window.addEventListener("load", init);
import * as fileUpload from "./../shared/fileUpload.js";
import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

/**
 * init function executed on page load.
 * Adds all the event listeners.
 * @author Niels Van Steen
 * */
function init() {
  popup.init();

  // Add the event listeners to toggle the visibility of the list that shows all the available emoji's.
  addEventListenersToToggleTheList();

  // Add the event listeners on all the emoji's so the user can click them and they'll be added.
  addEventListenersForAvailableEmoji();

  // Add the event listener that will be invoked when the state of the checkbox 'are emoji allowed' updates.
  // and will show/hide the emoji selector accordingly.
  updateEmojiSelector();
  const areEmojiAllowed = document.querySelector("#AreEmojiOnCommentsAllowed");
  areEmojiAllowed.addEventListener("change", function () {
    updateEmojiSelector();
  });

  // Adds the event listener that will upload the image to the cloud storage, after which it can be used in a doc-review.
  const btnUploadImage = document.querySelector(".btn-confirm-upload-image");
  btnUploadImage.addEventListener("click", uploadImage);

  // Add the event listener that shows the popup window to upload an image.
  const btnOpenPopupUpload = document.querySelector("#buttonShowUploadPopup");
  btnOpenPopupUpload.addEventListener("click", function () {
    popup.showPopup(".article-popup-upload-image");
  });

  addEventListenersToUseAndCopyAnImage();

  // Add event listeners that will keep the image upload container maintain it's aspect ratio.
  fileUpload.changeLandScapeImageContainerRatios(
    ".image-preview-figure",
    ".image-preview-container"
  );
  window.addEventListener("resize", () => {
    fileUpload.changeLandScapeImageContainerRatios(
      ".image-preview-figure",
      ".image-preview-container"
    );
  });

  // Event listener that will preview the banner image once it's selected.
  // Add event listener that will be executed when an image is selected.
  const imageInput = document.querySelector("#bannerImageFile");
  imageInput.addEventListener("change", function () {
    fileUpload.addPreview("#bannerImagePreview", "#bannerImageFile");
  });
  
  const btnImportPdfPopup = document.querySelector(".import-pdf-popup-btn");
  btnImportPdfPopup.addEventListener("click", function () {
    popup.showPopup(".article-popup-import-pdf");
  });
  
} // Init.

/**
 * Uploads a docReview image using web api.
 * The image can then be used to write a doc-review.
 * @author Niels Van Steen
 * */
function uploadImage() {
  // Define the header.
  let h = new Headers();
  h.append("Accept", "application/json");

  // Construct the form data.
  let formData = new FormData();
  let image = document.querySelector("#uploadDocReviewImageInput").files[0];
  const errorMessagePopupContainer = document.querySelector(
    ".upload-image-error-message-container"
  );

  if (image === null || image === undefined) {
    errorMessagePopupContainer.innerHTML = "Select an image first!";
    return;
  }

  formData.append("image", image, image.name);

  // Create the request.
  let req = new Request(
    url.url() + url.getProjectName() + "/DocReviews/Upload",
    {
      method: "POST",
      headers: h,
      body: formData,
    }
  );

  // Send the request.
  fetch(req)
    .then(async (response) => {
      if (response.ok) {
        popup.closePopup(".article-popup-upload-image");
        updateExplorerWindow(await response.json());
      } else {
        errorMessagePopupContainer.innerHTML = await response.json();
      }
    })
    .catch((r) => {
      errorMessagePopupContainer.innerHTML =
        "This File extension is not allowed!";
    });
} // addUploadEventListener.

/**
 * Shows all the images in the explorer window.
 * @author Niels Van Steen
 * */
function updateExplorerWindow(imageLink) {
  const explorerWindowBody = document.querySelector(".images-explorer-body");

  explorerWindowBody.innerHTML += `   
        <div class="explorer-image-item-container">
            <figure>
                <img src="${imageLink}" alt="">
                <button class="btn-icon btn-copy-image" type="button" onclick="event.preventDefault();" data-img-src="${imageLink}">
                    <i class="fa-solid fa-copy"></i>
                </button>
                <button class="btn-icon btn-use-image" type="button" onclick="event.preventDefault();" data-img-src="${imageLink}">
                    <i class="fa-solid fa-file-import"></i>
                </button>
            </figure>
        </div>`;

  const errorMessage = document.querySelector("#no-images-error");
  if (errorMessage){
    errorMessage.remove();
  }
  // Update the event listeners.
  addEventListenersToUseAndCopyAnImage();
} // updateExplorerWindow.

/**
 * Updates the visibility of the emoji chooser.
 * @author Niels Van Steen
 * */
function updateEmojiSelector() {
  const areEmojiAllowed = document.querySelector("#AreEmojiOnCommentsAllowed");
  const containerElement = document.querySelector(".emoji-preview-wrapper");
  const mode = areEmojiAllowed.checked ? "block" : "none";
  containerElement.style.display = mode;
}

/**
 * Add the event listeners to toggle the visibility of the list that shows all the available emoji's.
 * @author Niels Van Steen
 * */
function addEventListenersToToggleTheList() {
  // Add the event listeners to toggle the visibility of the list that shows all the available emoji's.
  const toggleAvailableEmoji = document.querySelectorAll(
    ".btn-toggle-available-emoji"
  );
  for (let i = 0; i < toggleAvailableEmoji.length; i++) {
    const btnToggle = toggleAvailableEmoji[i];
    btnToggle.addEventListener("click", toggleVisibilityOfEmoji);
  } // For.
} // addEventListenersToToggleTheList.

/**
 * Toggle the container that shows the list with all available emoji's.
 * @author Niels Van Steen
 * */
function toggleVisibilityOfEmoji() {
  const element = document.querySelector(".all-available-emoji-container");
  const mode = element.style.display === "block" ? "none" : "block";
  element.style.display = mode;
} // toggleVisibilityOfEmoji.

/**
 * Add the event listeners for the available emoji's so the user can click them and they'll be added.
 * @author Niels Van Steen
 * */
function addEventListenersForAvailableEmoji() {
  const buttons = document.querySelectorAll(".btn-available-emoji");
  for (let i = 0; i < buttons.length; i++) {
    const button = buttons[i];
    const emojiCode = button.dataset.emojiCode;
    const emojiId = button.dataset.emojiId;
    button.addEventListener("click", addEmoji);
    button.emojiCode = emojiCode;
    button.emojiId = emojiId;
  } // For.
} // addEventListenersForAvailableEmoji.

/**
 * Add the event listeners to remove the selected  emoji's.
 * @author Niels Van Steen
 * */
function addEventListenersToRemoveSelectedEmoji() {
  const buttons = document.querySelectorAll(".btn-delete-selected-emoji");
  for (let i = 0; i < buttons.length; i++) {
    const button = buttons[i];
    const emojiCode = button.dataset.emojiCode;
    const emojiId = button.dataset.emojiId;
    button.addEventListener("click", removeEmoji);
    button.emojiCode = emojiCode;
    button.emojiId = emojiId;
  } // For.
} // addEventListenersToRemoveSelectedEmoji.

/**
 * Adds an available emoji to the selected list and removes it from the available list.
 * @author Niels Van Steen
 * */
function addEmoji(evt) {
  const emojiCode = evt.currentTarget.emojiCode;
  const emojiId = evt.currentTarget.emojiId;

  const containerElement = document.querySelector("#emojiPreviewContainer");

  // Add the emoji to the list.
  containerElement.innerHTML += `
        <div class="emoji-preview-item" data-emoji-id="${emojiId}">
            <button type="button" onclick="event.preventDefault()" data-emoji-id="${emojiId}" data-emoji-code="${emojiCode}" class="btn-icon btn-delete-selected-emoji"><i class="fa-solid fa-trash-can"></i></button>
            <p>&#${emojiCode};</p>
            <input type="checkbox" class="SelectedEmojiIds" name="SelectedEmojiIds" value="${emojiId}" checked>
        </div>`;

  // Remove the emoji from the available list.
  const emojiToRemove = document.querySelector(
    ".btn-available-emoji[data-emoji-id='" + emojiId + "']"
  );
  emojiToRemove.remove();

  // Add the event listeners when the user wants to remove a selected emoji.
  addEventListenersToRemoveSelectedEmoji();
} // addEmoji.

/**
 * Removes an emoji from the selected list ands add it back to the list of all available emoji's.
 * @author Niels Van Steen
 * */
function removeEmoji(evt) {
  const emojiCode = evt.currentTarget.emojiCode;
  const emojiId = evt.currentTarget.emojiId;

  // Remove the selected emoji.
  const elementToRemove = document.querySelector(
    ".emoji-preview-item[data-emoji-id='" + emojiId + "']"
  );
  elementToRemove.remove();

  // Add it back to the list with available emoji's.
  const containerElement = document.querySelector(
    ".all-available-emoji-container"
  );
  containerElement.innerHTML += `   
                <button type="button" onclick="event.preventDefault()" data-emoji-code="${emojiCode}" data-emoji-id="${emojiId}" class="btn-icon btn-available-emoji">
                    <span>&#${emojiCode};</span>
                </button>`;

  // Update the event listeners in the available emoji list.
  addEventListenersForAvailableEmoji();

  // Add the event listeners to toggle the visibility of the list that shows all the available emoji's.
  addEventListenersToToggleTheList();
} // removeEmoji.

/**
 * Adds the event listeners so the user can click on an image in the explorer window and it'll be inserted in the docreview.
 * And the event listeners that copies the image to the user's clipboard.
 * @author Niels Van Steen
 * */
function addEventListenersToUseAndCopyAnImage() {
  const useButtons = document.querySelectorAll(".btn-use-image");
  for (let i = 0; i < useButtons.length; i++) {
    const useButton = useButtons[i];
    const imageSource = useButton.dataset.imgSrc;
    useButton.addEventListener("click", function () {
      const element = `<img src='${imageSource}' alt='image'>`;
      CKEDITOR.instances["DocReviewText"].insertHtml(element);
    });
  }

  const copyButtons = document.querySelectorAll(".btn-copy-image");
  for (let i = 0; i < copyButtons.length; i++) {
    const copyButton = copyButtons[i];
    const imageSource = copyButton.dataset.imgSrc;
    copyButton.addEventListener("click", function () {
      const element = imageSource;
      navigator.clipboard.writeText(element);
      popup.quickShowPopup(".article-popup-text-copied", 2500);
    });
  }
} // addEventListenersToUseAndCopyAnImage.
