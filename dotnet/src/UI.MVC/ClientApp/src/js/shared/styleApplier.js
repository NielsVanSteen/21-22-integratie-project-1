import * as url from "./../shared/url.js";

/**
 * Functions in this file are used to check which project style to apply.
 *
 * This function checks the sessionStorage. When the external project name in the url and in sessionStorage differ
 * the api fetch request is executed and the new style is loaded in local storage.
 *
 * @param forceReload, is only set to true when changing the style on a project. -> otherwise it'll return since the cached & url project are the same.
 *
 * @author Niels Van Steen
 * */
export async function checkReloadStyles(forceReload = false) {

    const cachedProject = sessionStorage.getItem("project");
    const urlProject = url.getProjectName();

    if (cachedProject != null && cachedProject.toLowerCase() === urlProject.toLowerCase() && !forceReload)
        return;

    const request = await fetch(url.url() + url.getProjectName() + "/ProjectStylings/GetProjectStyle/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    });

    if (request.status === 204) {
        //sessionStorage.removeItem("project");
        sessionStorage.removeItem("projectStyle");
    }

    sessionStorage.setItem("project", urlProject); // update the local storage project.

    if (request.status !== 200)
        return;

    try {
        const response = await request.json();
        
        // Json.stringify doesn't work on an object so store it in an array and user json.stringify.
        let responses = [];
        responses.push(response);
        sessionStorage.setItem("projectStyle", JSON.stringify(responses));
    } catch(error) {
         sessionStorage.removeItem("projectStyle");
    }

}

/**
 * This function takes the stored style from sessionStorage and overrides the css variables
 * before the DOM is completely loaded.
 *
 * @author Niels Van Steen
 * */
export function applyStyles() {

    // Get project style from local storage.
    const styles = JSON.parse(sessionStorage.getItem("projectStyle"));

    if (styles == null)
        return;

    // object is stores in an array because JSON.parse wouldn't work when using stringify on the object itself.
    const projectStyle = styles[0];

    if (projectStyle == null)
        return;
    
    // Override the css variables.
    document.documentElement.style.cssText = `
        --light-green: #${projectStyle.colorLight};
        --normal-green: #${projectStyle.colorMedium};
        --dark-green: #${projectStyle.colorDark};
        --darkest-green: #${projectStyle.colorDark};
    `;
} // applyStyles.