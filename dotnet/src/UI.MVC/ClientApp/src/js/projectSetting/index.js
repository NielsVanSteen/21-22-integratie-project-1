import * as fileUpload from "./../shared/fileUpload.js";
import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

// execute on page load
window.addEventListener("load", init);

/**
 * function that gets called on page load
 * @returns {Promise<void>}
 * @author Michiel Verschueren
 */
async function init() {
  await fillTextAreas();
  addEventListeners();
}

/**
 * fill text areas of the statements with data from the database
 * @returns {Promise<void>}
 * @author Michiel Verschueren
 */
async function fillTextAreas() {
  let response = await fetch(
    url.url() +
      url.getProjectName() +
      "/ProjectSettings/GetAccessibilityAndPrivacyStatement"
  );
  let responseJson = JSON.parse(await response.text());

  if (responseJson.privacy !== undefined && responseJson.privacy !== "") {
    document.querySelector("#privacy-statement").value = responseJson.privacy;
  }
  if (
    responseJson.accessibility !== undefined &&
    responseJson.accessibility !== ""
  ) {
    document.querySelector("#accessibility-statement").value =
      responseJson.accessibility;
  }
}

/**
 * function that adds eventlisteners to all the buttons
 * @author Michiel Verschueren
 */
function addEventListeners() {
  // get button to submit statements
  let statementsButton = document.querySelector("#add-statements");
  //add an event listener to the button to submit data to API
  statementsButton.addEventListener("click", submitStatements);

  // get button to archive a project
  let archiveButton = document.querySelector("#archive");
  //add an event listener to the button to submit data to API
  archiveButton.addEventListener("click", archiveProject);

  // get button to publish a project
  let publishButton = document.querySelector("#publish");
  //add an event listener to the button to submit data to API
  publishButton.addEventListener("click", publishProject);

  addEventListenersFooterLogos();
} // addEventListeners.

function addEventListenersFooterLogos() {
  // Add event listener to delete the project footer logo.
  const deleteLogosButtons = document.querySelectorAll(
    ".btn-delete-footer-logos"
  );
  for (let i = 0; i < deleteLogosButtons.length; i++) {
    const btnDelete = deleteLogosButtons[i];
    btnDelete.addEventListener("click", deleteFooterLogo);
    btnDelete.id = btnDelete.dataset.footerLogoId;
  } // For.

  // Add event listener that will be executed when an a project logo image is selected
  const projectLogoInput = document.querySelector("#footerLogo");
  projectLogoInput.addEventListener("change", function () {
    fileUpload.addPreview("#projectLogoImageDisplay", "#footerLogo");
  });

  // Add event listeners to add a footer logo.
  const addFooterLogo = document.querySelector("#addFooterLogo");
  addFooterLogo.addEventListener("click", uploadFooterLogo);
} // addEventListenersFooterLogos.

function deleteFooterLogo(evt) {
  const id = evt.currentTarget.id;

  fetch(
    url.url() + url.getProjectName() + "/ProjectSettings/DeleteLogo/" + id,
    {
      method: "DELETE",
    }
  )
    .then((response) => {
      if (response.ok) {
        popup.showMessage(
          "Logo has been deleted!",
          "success",
          ".error-messages-container",
          4000
        );
      } else {
        popup.showMessage(
          "Logo cloud not be deleted!",
          "danger",
          ".error-messages-container",
          4000
        );
      }
    })
    .catch((reason) => {
      popup.showMessage(
        "Logo Could not be deleted!",
        "danger",
        ".error-messages-container",
        4000
      );
    });

  document
    .querySelector(".footer-logo-container[data-footer-logo-id='" + id + "']")
    .remove();
} // deleteFooterLogo.

/**
 * get text from text areas and add them to the database
 * @returns {Promise<void>}
 * @author Michiel Verschueren
 */
async function submitStatements() {
  // Get values of the text areas
  let privacy = document.querySelector("#privacy-statement").value;
  let accessibility = document.querySelector("#accessibility-statement").value;
  if (privacy.trim() === "" && accessibility.trim() === "") {
    return popup.showMessage(
      "The privacy and accessibility statement must not be empty.",
      "danger",
      ".status-messages-container",
      5000
    );
  }
  if (privacy.trim() === "") {
    return popup.showMessage(
      "The privacy statement must not be empty.",
      "danger",
      ".status-messages-container",
      5000
    );
  }
  if (accessibility.trim() === "") {
    return popup.showMessage(
      "The accessibility statement must not be empty.",
      "danger",
      ".status-messages-container",
      5000
    );
  }

  // create json object from data
  const body = {
    Accessibility: accessibility,
    Privacy: privacy,
  };

  //post data to API
  let response = await fetch(
    url.url() +
      url.getProjectName() +
      "/ProjectSettings/AddPrivacyAndAccessibilityStatement",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    }
  );

  let message, type;
  if (!response.ok) {
    message = await response.text();
    type = "danger";
  } else {
    message = "Statements have been updated!";
    type = "success";
  }

  popup.showMessage(message, type, ".status-messages-container", 5000);
}

/**
 * Function to perform a web API request to archive a project
 * @author Michiel Verschueren
 */
async function archiveProject() {
  const errorSpan = document.querySelector("#archive-error");
  let response = await fetch(
    url.url() + url.getProjectName() + "/ProjectSettings/ArchiveProject",
    {
      method: "POST",
    }
  );

  if (response.ok) {
    const archiveButton = document.querySelector("#archive");
    archiveButton.disabled = true;
    errorSpan.innerHTML = `<p class="alert alert-success">Project Has Been archived!</p>`;
  } else {
    errorSpan.innerHTML = `<p class="alert alert-danger">${await response.text()}</p>`;
  }
}

/**
 * Function to perform a web API request to archive a project
 * @author Michiel Verschueren
 */
async function publishProject() {
  const errorSpan = document.querySelector("#publish-error");
  let response = await fetch(
    url.url() + url.getProjectName() + "/ProjectSettings/PublishProject",
    {
      method: "POST",
    }
  );

  if (response.ok) {
    const publishButton = document.querySelector("#publish");
    publishButton.disabled = true;
    const archiveButton = document.querySelector("#archive");
    archiveButton.disabled = false;
    errorSpan.innerHTML = `<p class="alert alert-success">Project Has Been published!</p>`;
  } else {
    errorSpan.innerHTML = `<p class="alert alert-danger">${await response.text()}</p>`;
  }
}

async function uploadFooterLogo() {
  // Define the header.
  let h = new Headers();
  h.append("Accept", "application/json");

  // Construct the form data.
  let formData = new FormData();
  let image = document.querySelector("#footerLogo").files[0];
  const errorMessagePopupContainer = document.querySelector(
    ".error-messages-container"
  );

  if (image === null || image === undefined) {
    popup.showMessage(
      "Select an image first!",
      "danger",
      ".error-messages-container",
      3000
    );
    return;
  }

  formData.append("image", image, image.name);

  // Create the request.
  let req = new Request(
    url.url() + url.getProjectName() + "/ProjectSettings/UploadFooterLogo",
    {
      method: "POST",
      headers: h,
      body: formData,
    }
  );

  try {
    
    const res = await fetch(req);
    
    if (res.ok) {
      const json = await res.json();
      setTimeout(() => {
        updateLogoViews(json);
      }, 1000);
    
    } else {
      errorMessagePopupContainer.innerHTML = await res.json();
    }
    
  } catch (err) {
    popup.showMessage(
      "This File extension is not allowed!",
      "danger",
      ".error-messages-container",
      3000
    );
  }
} // uploadFooterLogo.

function updateLogoViews(response) {
  const containerElement = document.querySelector(".project-footer-wrapper");
  const html = `
        <section class="footer-logo-container" data-footer-logo-id="${response.id}">
            <figure>
                <img src="${response.imageLink}" alt="Logo">
                <button class="btn btn-icon btn-delete-footer-logos" data-footer-logo-id="${response.id}">
                    <i class="fa-solid fa-trash-can"></i>
                </button>
            </figure>
        </section>`;
  containerElement.innerHTML += html;

  // Delete the image in the preview.
  document.querySelector("#projectLogoImageDisplay").src = "";

  // Update delete event listeners.
  addEventListenersFooterLogos();
} // updateLogoViews.
