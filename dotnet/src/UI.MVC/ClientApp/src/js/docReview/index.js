import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";
import {selectText} from "./../shared/select.js";
import smoothScroll from "./../shared/scroll.js";

let lastText = "";
let html = "";
init();

/**
 * Initialize all buttons and eventlisteners
 * @author Sander Verheyen
 */
function init() {
    /* Getting all the buttons that need to have an eventListener. */
    const selectTextButton = document.getElementById('SelectText');
    const placeComment = document.getElementById('placeComment');
    const applyFilter = document.getElementById('buttonApplyFilter');
    const postSubComments = document.getElementsByClassName('sub-comment-post');
    const cancelSubComments = document.getElementsByClassName('sub-comment-cancel');
    const reportButton = document.getElementsByClassName('report-button');
    const removeButton = document.getElementsByClassName('remove-button');
    const editButton = document.getElementsByClassName('edit-button');
    const markButton = document.getElementsByClassName('mark-button');
    const addEmojiButton = document.getElementsByClassName('add-emoji-button');
    const moreEmojiButton = document.getElementsByClassName('more-emoji-button');
    const loadMoreButton = document.getElementById('loadMoreButton');

    /* Giving all the buttons an eventlistener and adding a property for commentId. */
    // Post sub-comments
    for (let i = 0; i < postSubComments.length; i++) {
        const subcomment = postSubComments[i];
        subcomment.addEventListener('click', publishSubComment);
        subcomment.commentId = subcomment.dataset.commentId;
    }
    // Cancel sub-comments
    for (let i = 0; i < cancelSubComments.length; i++) {
        const cancel = cancelSubComments[i];
        cancel.addEventListener('click', cancelSubComment);
        cancel.commentId = cancel.dataset.commentId;
    }

    // Report buttons
    for (let i = 0; i < reportButton.length; i++) {
        const report = reportButton[i];
        report.addEventListener('click', reportComment);
        report.commentId = report.dataset.commentId;
    }
    // Remove buttons
    for (let i = 0; i < removeButton.length; i++) {
        const remove = removeButton[i];
        remove.addEventListener('click', deleteComment);
        remove.commentId = remove.dataset.commentId;
    }
    // Mark buttons
    for (let i = 0; i < markButton.length; i++) {
        const mark = markButton[i];
        mark.addEventListener('click', markInText);
        mark.html = mark.dataset.html;
    }
    // Edit buttons
    for (let i = 0; i < editButton.length; i++) {
        const edit = editButton[i];
        edit.addEventListener('click', editComment);
        edit.commentId = edit.dataset.commentId;
    }
    // Adding emoji's
    for (let i = 0; i < addEmojiButton.length; i++) {
        const emojiButton = addEmojiButton[i];
        emojiButton.addEventListener('click', addEmoji);
        emojiButton.commentId = emojiButton.dataset.commentId;
        emojiButton.emojiId = emojiButton.dataset.emojiId;
    }
    for (let i = 0; i < moreEmojiButton.length; i++) {
        const moreEmoji = moreEmojiButton[i];
        moreEmoji.addEventListener('click', showEmojis);
        moreEmoji.commentId = moreEmoji.dataset.commentId;
    }
    // For posting a new main comment
    if (selectTextButton !== null && placeComment !== null) {
        selectTextButton.addEventListener('click', select);
        placeComment.addEventListener('click', publishComment);
    }
    if (loadMoreButton !== null) {
        loadMoreButton.addEventListener('click', loadMoreComments);
        loadMoreButton.pageSize = loadMoreButton.dataset.pageSize;
        loadMoreButton.pageNumber = loadMoreButton.dataset.pageNumber;
    }
    
    
}

function select(){
    let selection = selectText();
    html = selection["html"];
    const field = document.getElementById('selectedText');
    field.innerHTML = selection["text"];
}

/**
 * publishes a new comment
 * @author Sander Verheyen
 */
function publishComment() {
    /* Get the comment text. */
    html = removeMarkedTags(html);
    const commentText = document.getElementById('commentText').value;
    if (commentText.trim() === "") {
        popup.showMessage("Gelieve een reactie in the geven.", "danger", ".error-message-box", 5000);
        return;
    }
    /* Make a new comment object. */
    const comment = {
        CommentText: commentText,
        PlacedOnReactionId: null,
        DocReviewId: parseInt(window.location.href.split('/').reverse()[0]),
        Quote: html
    };
    fetch(url.url() + url.getProjectName() + "/Comments/CreateComment", {
        method: "POST",
        body: JSON.stringify(comment),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.ok) {
                postComment(response.json());
            } else if (response.status === 400) {
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * Publishes a sub comment
 * @param evt
 */
function publishSubComment(evt) {
    const commentId = evt.currentTarget.commentId;
    const commentText = document.querySelector(`.sub-comment-text[data-comment-id="${commentId}"]`).value;
    if (commentText.trim() === "") {
        popup.showMessage("Gelieve een reactie in the geven.", "danger", ".error-message-box", 5000);
        return;
    }
    const comment = {
        CommentText: commentText,
        PlacedOnReactionId: parseInt(commentId),
        DocReviewId: parseInt(window.location.href.split('/').reverse()[0]),
        Quote: ""
    };
    fetch(url.url() + url.getProjectName() + "/Comments/CreateComment", {
        method: "POST",
        body: JSON.stringify(comment),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.ok) {
                postSubComment(response.json(), commentText, commentId);
            } else if (response.status === 400) {
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * Empties the input fields of a sub comment.
 * @param evt
 */
function cancelSubComment(evt) {
    const commentId = evt.currentTarget.commentId;
    const commentText = document.querySelector(`.sub-comment-text[data-comment-id="${commentId}"]`);
    commentText.valueOf = "";
}

/**
 * @author Sander Verheyen
 * prints the placed comment
 * @param promise
 * @returns {Promise<void>}
 */
async function postComment(promise) {

    // Loads the return of the response
    let comment = await promise;

    // Gets all the values
    let commentText = document.getElementById('commentText').value;
    const postedComments = document.getElementById('commentSection');
    let mainComment = '';
    const profilePic = document.querySelector(`.header-profile-icon`);
    let quoteHtml = removeStartEndHtml(html);
    let shortQuote = quoteHtml.substring(0, quoteHtml.length > 18 ? 18 : quoteHtml.length);

    if (comment.placedOn == null && comment.subCommenting) {

        // Adding the option to place subcomments
        mainComment += `
            <div class="sub-comment-container">
                        <div class="row">
                            <figure class="sub-comment-profile-picture-figure">
                                ${profilePic.outerHTML}
                            </figure>
                            <div class="sub-comment-text-container">
                                <textarea class="sub-comment-text" data-comment-id="${comment.id}" placeholder="Reageer"></textarea>
                            </div>
                        </div>
                        <div class="sub-comment-actions-container">
                            <button class="sub-comment-cancel btn-sub-comment-action btn btn-secondary btn-sm" data-comment-id="${comment.id}">Annuleer</button>
                            <button class="sub-comment-post btn-sub-comment-action btn btn-success btn-sm" data-comment-id="${comment.id}">Reageer</button>
                        </div>
                    </div>
        `;
    }
    let emojiSection = "";
    if (comment.emojis) {
        emojiSection = `
            <section class="comment-emoji-wrapper">
                <ul class="emojis-list" data-comment-id="${comment.id}">
                    <li class="li-more-emoji">
                        <button class="more-emoji-button btn-icon" data-comment-id="${comment.id}">
                            <i class="fa-solid fa-face-smile-beam"></i>
                        </button>
                    </li>
                </ul>
            </section>
        `;
    }

    /* placing a new comment. */
    postedComments.innerHTML +=`
        <section class="comment-block comment-wrapper comment-user-page-block-wrapper" data-comment-id="${comment.id}">
             <section class="single-comment comment-container comment-user-page-container" data-comment-id="${comment.id}">
                <div class="comment-row comment-row-user-page">
                    <figure>${profilePic.outerHTML}</figure>
                    
                    <!-- Name and date. -->
                    <div class="comment-writer-date-wrapper">
                        <h2>${comment.fullname}</h2>
                        <p class="comment-date">Een paar seconde geleden</p>
                    </div>
                </div>
                <section class="comment-quote-container">
                <p>
                    Commented on
                    <button class="mark-button btn-icon" data-comment-id="${comment.id}"><span>'${shortQuote}'</span></button>
                </p>
                </section>
                <section class="comment-text-container" data-comment-id="${comment.id}">
                    <p class="comment-text" data-comment-id="${comment.id}">${commentText}</p>
                </section>
            
                <div>
                    ${emojiSection}
                </div>

                <section class="comment-tags-wrapper comment-actions-user-page-wrapper comment-hover-wrapper">
                    <div class="icon-container">
                        <i class="fa-solid fa-gear"></i>
                    </div>
                    <div class="comment-tags-container comment-hover-active-container">
                        <button class="edit-button btn-icon btn-edit" data-comment-id="${comment.id}"><i class="fa-solid fa-pen-to-square"></i></button>
                        <button class="save-button btn-icon btn-save" data-comment-id="${comment.id}"><i class="fa-solid fa-circle-check"></i></button>
                        <button class="remove-button btn-icon" data-comment-id="${comment.id}"><i class="fa-solid fa-trash-can"></i></button>
                    </div>
                </section> 
            </section>
            <section class="sub-comment-wrapper" data-comment-id="${comment.id}">
                <div class="sub-comment-block" data-comment-id="${comment.id}"></div>
                 ${mainComment}
            </section>
        
         </section>`;
    let saveButton = document.querySelector(`.save-button[data-comment-id="${comment.id}"]`);
    saveButton.style.display = "none";
    let quoteButton = document.querySelector(`.mark-button[data-comment-id="${comment.id}"]`);
    quoteButton.setAttribute("data-html", quoteHtml);
    /* Empty the input field*/
    cancelComment();
    /* Give all the event listeners to new buttons*/
    init();
}

/**
 * @author Sander Verheyen
 * prints the placed sub comment
 * @param promise
 * @returns {Promise<void>}
 */
async function postSubComment(user, commentText, placedOn) {
    // Gets the user.
    let comment = await user;
    const commentId = comment.id;
    const profilePic = document.querySelector(`.header-profile-icon`);
    const quote = document.querySelector(`.single-comment[data-comment-id="${placedOn}"]`).querySelector(`.comment-writer-date-wrapper h2`).innerText;

    // Gets the right sub comment block.
    const subCommentSection = document.querySelector(`.sub-comment-block[data-comment-id="${placedOn}"]`);// let subCommentSection;

    // Placing the sub comment. 
    let emojiSection = "";

    if (comment.emojis) {
        emojiSection = `
            <section class="comment-emoji-wrapper">
                <ul class="emojis-list" data-comment-id="${commentId}">
                    <li class="li-more-emoji">
                        <button class="more-emoji-button btn-icon" data-comment-id="${commentId}">
                            <i class="fa-solid fa-face-smile-beam"></i>
                        </button>
                    </li>
                </ul>
            </section>
        `;
    }

    subCommentSection.innerHTML += `
        <section class="single-comment comment-container comment-user-page-container" data-comment-id="${comment.id}">
                <div class="comment-row comment-row-user-page">
                    <figure>${profilePic.outerHTML}</figure>
                    
                    <!-- Name and date. -->
                    <div class="comment-writer-date-wrapper">
                        <h2>${comment.fullname}</h2>
                        <p class="comment-date">Een paar seconde geleden</p>
                    </div>
                </div>
                
                <section class="comment-text-container" data-comment-id="${comment.id}">
                    <span>@${quote}</span><br>
                     <p class="comment-text" data-comment-id="${comment.id}">${commentText}</p>
                </section>
                <div>
                    ${emojiSection}
                </div>
                <section class="comment-tags-wrapper comment-actions-user-page-wrapper comment-hover-wrapper">
                    <div class="icon-container">
                        <i class="fa-solid fa-gear"></i>
                    </div>
                    <div class="comment-tags-container comment-hover-active-container">
                        <button class="edit-button btn-icon btn-edit" data-comment-id="${comment.id}"><i class="fa-solid fa-pen-to-square"></i></button>
                        <button class="save-button btn-icon btn-save" data-comment-id="${comment.id}"><i class="fa-solid fa-circle-check"></i></button>
                        <button class="remove-button btn-icon" data-comment-id="${comment.id}"><i class="fa-solid fa-trash-can"></i></button>
                    </div>
                </section> 
            </section>
    `;

    let saveButton = document.querySelector(`.save-button[data-comment-id="${commentId}"]`);
    const text = document.querySelector(`.sub-comment-text[data-comment-id="${placedOn}"]`);
    text.value = "";
    saveButton.style.display = "none";

    // Give all the event listeners to new buttons
    init();
}

/**
 * @author Sander Verheyen
 * Empties the input fields of a new comment
 */
function cancelComment() {
    document.getElementById('selectedText').innerHTML = "";
    document.getElementById('commentText').value = "";
}

/**
 * @author Sander Verheyen
 * Marks a comment as inappropriate.
 * @param evt
 */
function reportComment(evt) {
    const commentId = evt.currentTarget.commentId;
    fetch(url.url() + url.getProjectName() + "/Comments/ReportComment", {
        method: "POST",
        body: JSON.stringify(commentId),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.status === 404) {
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * Deletes a comment placed by current user.
 * @param evt
 */
function deleteComment(evt) {
    const commentId = evt.currentTarget.commentId;
    fetch(url.url() + url.getProjectName() + "/Comments/CreateCommentDeletedHistory", {
        method: "POST",
        body: JSON.stringify(commentId),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.status === 200) {
                commentRemove(commentId);
            } else if (response.status === 400 || response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * places an emoji on a comment.
 * @param evt
 */
function addEmoji(evt) {
    // Getting the values
    const commentId = evt.currentTarget.commentId;
    const emojiId = evt.currentTarget.emojiId;
    const emojiReaction = {
        PlacedOnReactionId: commentId,
        EmojiId: emojiId,
        DocReviewId: parseInt(window.location.href.split('/').reverse()[0])
    };
    fetch(url.url() + url.getProjectName() + "/Comments/CreateEmojiReaction", {
        method: "POST",
        body: JSON.stringify(emojiReaction),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    updateEmojis(commentId, data);
                })
            } else if (response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * Removes the comment from the page
 * @param commentId
 */
function commentRemove(commentId) {
    let comment = document.querySelector(`.single-comment[data-comment-id="${commentId}"]`);
    if (comment.closest('.sub-comment-block')){
        comment.remove();
        return;
    }
    comment = document.querySelector(`.comment-block[data-comment-id="${commentId}"]`);
    comment.remove();
}

function removeMarkedTags(text) {
    const regexMarked = /((<span class="marked">)(.*?)(<\/span>))/;
    while (text.match(regexMarked)){
        text = text.replace(text.match(regexMarked)[1], text.match(regexMarked)[3]);
    }
    return text;
}

/**
 * @author Sander Verheyen
 * Marks the quote in the text.
 * @param evt
 */
function markInText(evt) {
    
    /* Gets the docreview text area. */
    const docText = document.getElementsByClassName('doc-review-actual-text-container');
    let text = docText[0].innerHTML;
    const markRegex = /(<[^\/].*?>)+(.*?)(<\/.*?>)+/;
    const markStart = /^(.*?)<\/.*?>/;
    const markEnd = /([<].*[>])(.*?)$/;
    const regexGoogleDocs = /=""/;
    const regexFont = /family:" /;
    let replaceQuote = "";
    if (text !== "") {
        /* Removes all the marks from the text. */
        text = removeMarkedTags(text);
        if (text.match(regexGoogleDocs)){
            text = text.replace(new RegExp(regexGoogleDocs,'g'), "");
        }
        if (text.match(regexFont)){
            text = text.replace(new RegExp(regexFont, 'g'), "family:\"");
        }
        /* Remove all the white spaces from text. */
        text = text.trim();
        text = text.replace(new RegExp(/ {2,}/, "g"), "");
        text = text.replace(new RegExp(/\r?\n|\r/, "g"), "");
        text = text.toLowerCase();
        /* Remove all the html entities. */
        const textarea = document.createElement("textarea");
        textarea.innerHTML = text;
        text = textarea.value;
        let quoteHtml = evt.currentTarget.html;
        quoteHtml = removeStartEndHtml(quoteHtml);
        replaceQuote = quoteHtml;
        let result = replaceQuote;
        /* place spans between other html */
        if (replaceQuote.match(markRegex)){
            while(replaceQuote.match(markRegex)){
                let group = markRegex.exec(replaceQuote);
                result = result.replace(group[0], group[0].replace(group[2],'<span class="marked">'+group[2] +'</span>'));
                replaceQuote = replaceQuote.replace(group[0],"");
            }
        }
        replaceQuote = result;
        if (replaceQuote.match(markStart)){
            replaceQuote = replaceQuote.replace(replaceQuote.match(markStart)[1], replaceQuote.match(markStart)[1] + '</span>');
        }
        if (replaceQuote.match(markEnd)){
            replaceQuote = replaceQuote.replace(replaceQuote.match(markEnd)[2], '<span class="marked">'+replaceQuote.match(markEnd)[2]);
        }
        /* place starting and end span */
        replaceQuote = '<span class="marked">'+ replaceQuote +'</span>';
        /* replace doc-review text with marked text. */
        docText[0].innerHTML = text.replace(quoteHtml.toLowerCase(), replaceQuote);        
        scrollToHighlightedText();
    }
}

function scrollToHighlightedText() {
   const element = document.querySelector('.marked');
   
   let screenHeight = window.innerHeight;
   const offset = screenHeight / 3;
   
   smoothScroll(".marked", offset);
}

/**
 * @author Sander Verheyen
 * Editing a comment.
 * @param evt
 */
function editComment(evt) {
    /* Get the current comment. */
    const commentId = evt.currentTarget.commentId;
    let commentText = document.querySelector(`.comment-text[data-comment-id="${commentId}"]`);
    /* Make a new textarea with the commentText. */
    const textArea = `<textarea class="update-text" data-comment-id="${commentId}">${commentText.innerText}</textarea>`;
    lastText = commentText.innerText;
    /* Gets the save button and enables it. */
    let saveButton = document.querySelector(`.save-button[data-comment-id="${commentId}"]`);
    saveButton.style.display = "inline";
    /* Disable the edit button. */
    evt.currentTarget.style.display = "none";
    /* Set the commentText to the textarea to allow the user to edit it. */
    commentText.innerHTML = textArea;
    /* Give the save button an event. */
    saveButton.addEventListener('click', saveComment);
    saveButton.commentId = commentId;
}

/**
 * @author Sander Verheyen
 * Saves the new commentText
 * @param evt
 */
function saveComment(evt) {
    const commentId = evt.currentTarget.commentId;
    let editButton = document.querySelector(`.edit-button[data-comment-id="${commentId}"]`);
    const commentText = document.querySelector(`.update-text[data-comment-id="${commentId}"]`);
    const comment = {
        commentId: parseInt(commentId),
        EditedText: commentText.value
    };
    evt.currentTarget.style.display = "none";
    editButton.style.display = "inline";
    document.querySelector(`.comment-text[data-comment-id="${commentId}"]`).innerHTML = `<p class="comment-text" data-comment-id="@Model.CommentId}">${lastText}</p>`;
    if (comment.EditedText.trim() === ""){
        popup.showMessage("De text kan niet leeg zijn.", "danger", ".error-message-box", 5000);
        return;
    }
    
    fetch(url.url() + url.getProjectName() + "/Comments/UpdateCommentEditHistory", {
        method: "PUT",
        body: JSON.stringify(comment),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.status === 204 || response.status === 200) {
                commentUpdate(comment);
            } else if (response.status === 400 || response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data, "danger", ".error-message-box", 5000);
                })
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan", "danger", ".error-message-box", 5000));
}

/**
 * @author Sander Verheyen
 * Updates the with it's new text.
 * @param comment
 */
function commentUpdate(comment) {
    // Get the commentID.
    const commentId = comment.commentId;
    // Update the commentText to new text.
    let commentText = document.querySelector(`.comment-text[data-comment-id="${commentId}"]`);
    commentText.innerHTML = `<p class="comment-text" data-comment-id="@Model.CommentId}">${comment.EditedText}</p>`;
}

/**
 * @author Sander Verheyen
 * Remove the starting and end html tags in a quote.
 * @param quoteHtml
 */
function removeStartEndHtml(quoteHtml) {
    let regexStart;
    let regexEnd;
    regexStart = /(^(<[a-z].*?>)+)(.*)/;
    regexEnd = /((>.*?\/<)+)(.*)/;
    // Check if there are any matches with the regex.
    let quoteReverse = quoteHtml.split("").reverse().join("");
    // Apply the regex.
    if (quoteHtml.match(regexStart)){
        quoteHtml = quoteHtml.match(regexStart)[3];
        quoteReverse = quoteHtml.split("").reverse().join("");
    }
    if (quoteReverse.match(regexEnd)){
        quoteHtml = quoteReverse.match(regexEnd)[3];
        quoteHtml = quoteHtml.split("").reverse().join("");
    }
    return quoteHtml;
}

/**
 * @author Sander Verheyen
 * Updates the emoji counters and shows/removes a new emoji when it has been placed.
 * @param commentId
 * @param promise
 */
function updateEmojis(commentId, promise) {
    const addCode = promise.addCode;
    const removeCode = promise.removeCode;
    const emojiId = promise.emojiId;
    const removeEmojiId = promise.removeEmojiId;
    // Check if the emoji is already shown
    const liAdd = document.querySelector(`.shown-emoji[data-comment-id="${commentId}"][data-emoji-code="${addCode}"]`);
    const ulAdd = document.querySelector(`.emojis-list[data-comment-id="${commentId}"]`);
    if (liAdd != null) {
        // Add 1 to the counter
        let text = liAdd.innerText.split(' ')[0];
        text = parseInt(text);
        if (!isNaN(text)) {
            liAdd.innerHTML = `${++text} <button class="add-emoji-button btn-icon" data-comment-id="${commentId}" data-emoji-id="${emojiId}">${String.fromCodePoint(addCode)}</button>`;
            liAdd.classList.add("marked-emoji");
        }
    } else if (addCode !== "") {
        // Show the new emoji and set counter to 1
        ulAdd.innerHTML += `
            <li class="shown-emoji marked-emoji" data-comment-id="${commentId}" data-emoji-code="${addCode}">1 ${String.fromCodePoint(addCode)}
            </li>`;
    }
    if (removeCode !== "" && removeEmojiId !== 0) {
        const liRemove = document.querySelector(`.shown-emoji[data-comment-id="${commentId}"][data-emoji-code="${removeCode}"]`);
        let text = liRemove.innerText.split(' ')[0];
        text = parseInt(text);
        if (!isNaN(text) && --text === 0) {
            liRemove.remove();
        } else {
            liRemove.innerHTML = "";
            liRemove.innerHTML = `${text} <button class="add-emoji-button btn-icon" data-comment-id="${commentId}" data-emoji-id="${removeEmojiId}">${String.fromCodePoint(removeCode)}</button>`;
            liRemove.classList.remove("marked-emoji");
        }
    }
    init();
}

/**
 * @author Sander Verheyen
 * Shows all the emoji's available to place for this doc-review
 * @param evt
 */
function showEmojis(evt) {
    const commentId = evt.currentTarget.commentId;
    const globalEmojis = document.querySelectorAll(`.global-emoji-button`);
    for (let i = 0; i < globalEmojis.length; i++) {
        globalEmojis[i].dataset.commentId = commentId;
        globalEmojis[i].addEventListener("click", () => {
            popup.closePopup(".article-popup-react-emoji")
        });
    }
    popup.showPopup(".article-popup-react-emoji");
    init();
}

/**
 * @author Sander Verheyen
 * Loads more comments
 */
function loadMoreComments(){
    let pagesize = document.getElementById('pageSize').value
    pagesize+=5;
    document.getElementById('pageSize').value = pagesize;
    document.querySelector("#formFilter").submit();
}