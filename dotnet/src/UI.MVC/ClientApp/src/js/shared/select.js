/**
 * This function gets the highlighted text and places it in the quote field.
 * @author Sander Verheyen
 * @returns html string of the highlighted text
 */
export function selectText() {
    /* Get field for quote. */
    let returnValue = {text:"", html:""};
    /* Set the quote field */
    returnValue["text"] = window.getSelection().toString();
    /* Gets the html of the highlighted text. */
    if (typeof window.getSelection != "undefined") {
        const sel = window.getSelection();
        if (sel.rangeCount) {
            const container = document.createElement("div");
            for (let i = 0, len = sel.rangeCount; i < len; ++i) {
                container.appendChild(sel.getRangeAt(i).cloneContents());
            }
            returnValue['html'] = container.innerHTML;
        }
    } else if (typeof document.selection != "undefined") {
        if (document.selection.type === "Text") {
            returnValue['html'] = document.selection.createRange().html;
        }
    }
    return returnValue;
}