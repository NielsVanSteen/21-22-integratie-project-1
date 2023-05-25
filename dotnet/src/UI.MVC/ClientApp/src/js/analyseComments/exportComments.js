import * as popup from "../shared/popup.js";
import * as url from "./../shared/url.js";


window.addEventListener("load", init)

function init() {

    // Export.
    const btnExportJson = document.querySelector(".btn-export-json");
    const btnExportCsv = document.querySelector(".btn-export-csv");
    const btnExportXml = document.querySelector(".btn-export-xml");

    btnExportJson.addEventListener("click", exportCommentsJson);
    btnExportCsv.addEventListener("click", exportCommentsCsv);
    btnExportXml.addEventListener("click", exportCommentsXml);
}

function getFilter() {
    const searchText = document.querySelector("#filterName").value;

    const commentStatus = []
    const checkboxes = document.querySelectorAll('.check-box-comment-status:checked');
    checkboxes.forEach(checkbox => {
        commentStatus.push(checkbox.value);
    });

    const docReviewStatus = [];
    const checkboxesDocReviewStatus = document.querySelectorAll('.check-box-doc-reviews:checked');
    checkboxesDocReviewStatus.forEach(checkbox => {
        docReviewStatus.push(checkbox.value);
    });

    const projectTags = [];
    const checkboxesProjectTags = document.querySelectorAll('.check-box-project-tags:checked');
    checkboxesProjectTags.forEach(checkbox => {
        projectTags.push(checkbox.value);
    });

    const pageNumber = document.querySelector("#currentPageInput").value;
    const pageSize = document.querySelector("#entriesPageInput").value;

    const sortOrder = document.querySelector("#sortOrder").value;
    const sortOn = document.querySelector("#sort").value;

    const hasFilterChanged = document.querySelector("#hasFilterChanged").value;

    return {
        searchText: searchText,
        commentStatus: commentStatus,
        docReviews: docReviewStatus,
        projectTags: projectTags,
        pageNumber: pageNumber,
        pageSize: pageSize,
        sortOrder: sortOrder,
        sortOn: sortOn,
        hasFilterChanged: hasFilterChanged
    };
} // getFilter.

async function exportCommentsJson() {

    // Get the selected filter as a query string.
    const filter = getFilter();
    const query = url.objectToQueryString(filter);

    // Send the request.
    const response = await fetch(url.url() + url.getProjectName() + "/AnalyseComments/ExportCommentsJson?" + query, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    // Generate the file and download.
    await exportComments(response, "json");
}

async function exportCommentsXml() {
    // Get the selected filter as a query string.
    const filter = getFilter();
    const query = objectToQueryString(filter);

    // Send the request.
    const response = await fetch(url.url() + url.getProjectName() + "/AnalyseComments/ExportCommentsXml?" + query, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    // Generate the file and download.
    await exportComments(response, "xml");
} // exportCommentsXml.

async function exportCommentsCsv() {
    
    // Get the selected filter as a query string.
    const filter = getFilter();
    const query = objectToQueryString(filter);

    // Send the request.
    const response = await fetch(url.url() + url.getProjectName() + "/AnalyseComments/ExportCommentsCsv?" + query, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });

    // Generate the file and download.
    await exportComments(response, "csv");
    
} // exportCommentsCsv.

async function exportComments(response, type) {
    // Function that converts text to a blob.
    const str2blob = txt => new Blob([txt]);

    // Create blob.
    const blob = await response.blob();
    //const newBlob = new Blob([blob]);

    // Get text from blob and render the emoji's.
    const reader = new FileReader();
    reader.onload = function () {

        // Render all the emoji's with regex.
        const regexJson = `"Emoji": "([0-9]+)",`;
        const regexXml = `<Emoji>([0-9]+)</Emoji>`;
        const regexCsv = `Emoji:([0-9]+)`;
        let text = reader.result;
    
        // Replace based on the type.
        if (type === "json") {
            text = text.replace(new RegExp(regexJson, 'g'), function (match, p1) {
                return `"Emoji": "${String.fromCodePoint(p1)}",`;
            });
        } else if (type === "xml") {
            text = text.replace(new RegExp(regexXml, 'g'), function (match, p1) {
                return `<Emoji>${String.fromCodePoint(p1)}</Emoji>`;
            });
        } else if (type === "csv") {
            text = text.replace(new RegExp(regexCsv, 'g'), function (match, p1) {
                return `Emoji:${String.fromCodePoint(p1)}`;
            });
        }

        // Create new blob from
        const newBlob = str2blob(text);
        const blobUrl = window.URL.createObjectURL(newBlob);

        const aTag = document.querySelector(".download-link");
        aTag.href = blobUrl;
        aTag.download = `comments.${type}`;
        aTag.click();

        //popup.showMessage(`<a href="${blobUrl}" download="comments.json">Download file</a>`, "success", ".comment-section-wrapper", 5000);

    }
    reader.readAsText(blob);
} // exportComments.