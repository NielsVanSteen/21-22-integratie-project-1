/**
 * the root url of the backend application. with 'api' added.
 * @author Niels Van Steen
 * */
export function url() {
    return "/api/";
}

/**
 * Each project has a unique url since the external project name is added to the url.
 * This function returns the name of the project.
 * @author Niels Van Steen
 * @return external project name of the current project.
 * */
export function getProjectName() {
    const array = window.location.href.replace("https://", "").replace("http://","").split("/");
    const length = array.length;
    
    if (length < 2)
        return undefined;
    
    return array[1];
}

/**
 * @author Niels Van Steen
 * 
 * Converts javascript object to a query string.
 * */
export function objectToQueryString(obj) {
    const esc = encodeURIComponent;
    return Object.keys(obj).map(k => `${esc(k)}=${esc(obj[k])}`).join('&')
}


