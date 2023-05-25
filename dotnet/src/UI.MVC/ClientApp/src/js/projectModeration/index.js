// Execute on page load.
window.addEventListener("load", init);

import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

/**
 * Function invoked at page load, used to add event listeners.
 * @author Niels Van Steen
 * */
function init() {
    // Event listener invoked when the user clicks apply on the filter.
    const buttonApplyFilter = document.querySelector("#buttonApplyFilter");
    buttonApplyFilter.addEventListener("click", getProjectsByFilter);
} // init.

function objectToQueryString(obj) {
    const esc = encodeURIComponent;
    return Object.keys(obj).map(k => `${esc(k)}=${esc(obj[k])}`).join('&')
}

/**
 * Load the projects given the filter criteria.
 * Is executed when the user presses the filter button.
 * @author Niels Van Steen
 * */
async function getProjectsByFilter() {
    const sortSelect = document.querySelector("#sortOrder").value;
    const name = document.querySelector("#filterName").value;
    const sortOrder = sortSelect === "asc" ? "0" : "1";

    const object = {
        "name": name,
        "sortOrder": sortOrder
    };
    const query = objectToQueryString(object);
    
    const request = await fetch(
        url.url() + url.getProjectName() + "/ProjectsModeration/GetProjectByFilter?" + query, {
            method: "GET",
        }
    );

    const response = await request.json();

    showProjects(response);
}

/**
 * Function is executed after (next line):
 * @see getProjectsByFilter
 * And shows all the projects that meet the filter criteria.
 * @author Niels Van Steen
 * */
function showProjects(projects) {
    let container = document.querySelector("#projectListContainer");
    container.innerHTML = "";

    // @(CustomIdentityConstants.CloudStorageBasicUrl + project.GetProjectBannerImageName())
    projects.forEach((project) => {
        let html = `
            <section class="list-item-wrapper">
                <figure class="list-item-banner-image-figure">
                    <img src="${project.bannerImageUrl}" alt="Project Banner Image">
                </figure>
        
                <section class="list-item-information-container">
                    <ul>
                        <li>
                            <img src="${project.logoUrl}" alt="Project Logo">
                        </li>
                        <li>${project.externalName}</li>
                        <li>${project.internalName}</li>
                        <li>
                            <a href="${project.projectBackEndUrl}" class="default-link">Details</a>
                        </li>
                    </ul>
                    <p> ${project.introduction}</p>
                </section>
            </section>
        `;
        container.innerHTML += html;
    });
} // showProjects.
