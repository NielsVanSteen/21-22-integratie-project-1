import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

import autosize from "autosize/dist/autosize.js";

window.addEventListener("load", init);

/**
 * @author Sander Verheyen
 * Give all the buttons an event listener.
 */
function init() {
    // Get all the buttons.
    const addTagButtons = document.getElementsByClassName('add-tag');
    const deleteTagButtons = document.getElementsByClassName('delete-tag');
    const deleteCommentButtons = document.getElementsByClassName('delete-comment');
    const saveButtons = document.getElementsByClassName('save-comment');
    const publishButtons = document.getElementsByClassName('publish-comment');
    const liSelects = document.getElementsByClassName('full-select');
    const selects = document.getElementsByClassName('tag-selector');
    const status = document.getElementsByClassName('comment-status');

    // Add all the event listeners to the buttons and add properties.
    // All select lists.
    for (let i = 0; i < selects.length; i++) {
        if (selects[i].options.length === 0) {
            liSelects[i].style.display = "none";
        }
    }
    // Add Tag buttons.
    for (let i = 0; i < addTagButtons.length; i++) {
        const addTagButton = addTagButtons[i];
        const commentId = addTagButton.dataset.commentId;
        addTagButton.addEventListener('click', addTag);
        addTagButton.commentId = commentId;
    }
    // Delete Tag buttons.
    for (let i = 0; i < deleteTagButtons.length; i++) {
        const deleteTagButton = deleteTagButtons[i];
        const commentId = deleteTagButton.dataset.commentId;
        const tagId = deleteTagButton.dataset.tagId;
        deleteTagButton.addEventListener('click', deleteTag);
        deleteTagButton.commentId = commentId;
        deleteTagButton.tagId = tagId;
    }
    // Delete comment buttons.
    for (let i = 0; i < deleteCommentButtons.length; i++) {
        const deleteCommentButton = deleteCommentButtons[i];
        const commentId = deleteCommentButton.dataset.commentId;
        deleteCommentButton.addEventListener('click', deleteComment);
        deleteCommentButton.commentId = commentId;
    }
    // Save buttons.
    for (let i = 0; i < saveButtons.length; i++) {
        const saveButton = saveButtons[i];
        const commentId = saveButton.dataset.commentId;
        saveButton.addEventListener('click', saveComment);
        saveButton.commentId = commentId;
    }
    // Publish buttons.
    for (let i = 0; i < publishButtons.length; i++) {
        const publishButton = publishButtons[i];
        const commentId = publishButton.dataset.commentId;
        publishButton.addEventListener('click', publishComment);
        publishButton.commentId = commentId;
    }
    // Check if status is removed.
    for (let i = 0; i < status.length; i++) {
        if (status[i].innerHTML === "Removed") {
             disableRemoved(status[i].dataset.commentId);
        }
    }
    
    autosize(document.querySelectorAll('textarea'));

    addEventListenersFilters();

    addEventListenersPagination();
}

/**
 * @author Sander Verheyen
 * Adds a new tag to the comment.
 * @param evt
 */
function addTag(evt) {
    const commentId = evt.currentTarget.commentId;
    const updateSelector = document.querySelector(`.tag-selector[data-comment-id="${commentId}"]`);
    const tagId = updateSelector.value;
    const commentTag = {
        ProjectTagId: tagId,
        ReactionGroupId: commentId
    };
    fetch(url.url() + url.getProjectName() + "/Comments/UpdateTag", {
        method: "POST",
        body: JSON.stringify(commentTag),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
                if (response.status === 204) {
                    tagUpdate(commentId, tagId, false);
                } else if (response.status === 400 || response.status === 404){
                    response.text().then(data => {
                        popup.showMessage(data,"danger",".error-message-box",5000);
                    })
                }
            })
        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
}

/**
 * @author Sander Verheyen
 * deletes a tag from the comment.
 * @param evt
 */
function deleteTag(evt) {
    const commentId = evt.currentTarget.commentId;
    const tagId = evt.currentTarget.tagId;
    const commentTag = {
        ProjectTagId: parseInt(tagId),
        ReactionGroupId: parseInt(commentId)
    };
    fetch(url.url() + url.getProjectName() + "/Comments/DeleteTag", {
        method: "DELETE",
        body: JSON.stringify(commentTag),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
                if (response.status === 204) {
                        tagUpdate(commentId, tagId, true);
                } else if (response.status === 400 || response.status === 404){
                    response.text().then(data => {
                        popup.showMessage(data,"danger",".error-message-box",5000);
                    })
                }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
}

/**
 * @author Sander Verheyen
 * deletes a comment.
 * @param evt
 */
function deleteComment(evt) {
    const commentId = evt.currentTarget.commentId;
    const comment = {
        commentId: parseInt(commentId)
    };
    fetch(url.url() + url.getProjectName() + "/Comments/CreateCommentDeletedHistory", {
        method: "POST",
        body: JSON.stringify(comment.commentId),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            if (response.status === 200) {
                response.text().then(data => {
                    commentUpdate(comment, data);
                });
                
            } else if (response.status === 400 || response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data,"danger",".error-message-box",5000);
                })            
            }
            
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
}

/**
 * @author Sander Verheyen
 * Saves the edited commentText.
 * @param evt
 */
function saveComment(evt) {
    const commentId = evt.currentTarget.commentId;
    const comment = {
        commentId: commentId,
        EditedText: document.querySelector(`.text-area[data-comment-id="${commentId}"]`).value
    };
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
            if (response.status === 200) {
                response.text().then(data => {
                    commentUpdate(comment, data);
                });
                popup.showMessage("Comment updated successfully!","success",".error-message-box",5000);
                
            } else if (response.status === 400 || response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data,"danger",".error-message-box",5000);
                })            
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
}

/**
 * @author Sander Verheyen
 * publishes the comment.
 * @param evt
 */
function publishComment(evt) {
    const commentId = evt.currentTarget.commentId;
    const comment = {
        commentId: commentId,
    };
    fetch(url.url() + url.getProjectName() + "/Comments/CreateCommentPublishHistory/", {
        method: "POST",
        body: JSON.stringify(comment.commentId),
        headers: {
            "Content-Type": "application/json",
            'Accept': 'application/json'
        }
    })
        .then(response => {
            if (response.status === 200) {
                response.text().then(data => {
                    commentUpdate(comment, data);
                    const commentText = document.querySelector(`.text-area[data-comment-id="${commentId}"]`).value;
                    const editComment = {
                        commentId: commentId,
                        EditedText: commentText
                    }
                    fetch(url.url() + url.getProjectName() + "/Comments/UpdateCommentEditHistory", {
                        method: "PUT",
                        body: JSON.stringify(editComment),
                        headers: {
                            "Content-Type": "application/json"
                        }
                    })
                        .then(response2 => {
                            if (response2.status === 200) {
                                response2.text().then(data => {
                                    commentUpdate(comment, data);
                                });
                            }
                            else if (response2.status === 400 || response2.status === 404){
                                response2.text().then(data => {
                                    if (!(data.toLowerCase() === "De text was niet veranderd.".toLowerCase())) {
                                        popup.showMessage(data, "danger", ".error-message-box", 5000);
                                    }
                                })                            
                            }
                        })
                        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
            })
            }
            else if(response.status === 400 || response.status === 404){
                response.text().then(data => {
                    popup.showMessage(data,"danger",".error-message-box",5000);
                })            
            }
        })
        .catch(() => popup.showMessage("Er is iets fout gegaan","danger",".error-message-box",5000));
}

/**
 * @author Sander Verheyen
 * Update the page with the new tag or delete the tag
 * @param evt
 */
function tagUpdate(commentId, tagId, isDelete) {
    // Get the select list
    const liSelect = document.querySelector(`.full-select[data-comment-id="${commentId}"]`);
    const updateSelector = document.querySelector(`.tag-selector[data-comment-id="${commentId}"]`);
    const updateUl = document.querySelector(`.comment-tags[data-comment-id="${commentId}"]`);
    // If you have to delete the tag or add the tag.
    if (isDelete) {
        // Gets the tag that has to be deleted.
        const deleteLi = document.querySelector(`.tag[data-comment-id="${commentId}"][data-tag-id="${tagId}"]`);
        const opt = `<option value="${tagId}" data-name="${deleteLi.innerText}">${deleteLi.innerHTML.trim()}</option>`;
        // If the selector is empty enable it.
        if (updateSelector.options.length === 0) {
            liSelect.style.display = "block";
        }
        // Add the deleted tag to the selector.
        updateSelector.innerHTML += opt;
        // Remove the tag from the list.
        updateUl.removeChild(deleteLi);
    } else {
        // Add the tag to the list
        const html = ` 
            <li class="tag" data-comment-id="${commentId}" data-tag-id="${tagId}">
                <p>${updateSelector.options[updateSelector.selectedIndex].dataset.name} </p>
                <button class="delete-tag btn-icon" type="button" data-comment-id="${commentId}" data-tag-id="${tagId}"><i class="fa-solid fa-trash-can"></i></button>
            </li>`;
        updateUl.innerHTML += html;
        updateSelector.remove(updateSelector.selectedIndex);
        // If the selector is now empty don't show it.
        if (updateSelector.options.length === 0) {
            liSelect.style.display = "none";
        }
    }
    init();
}

/**
 * @author Sander Verheyen
 * Edit the comments status based on what was done.
 * @param comment
 * @param promise
 */
async function commentUpdate(comment, promise) {
    // Getting the promise
    let status = promise;
    // Finding the correct field to update
    const updateStatus = document.querySelector(`.comment-status[data-comment-id="${comment.commentId}"]`);
    status = status.replaceAll("\"", '');
    updateStatus.innerText = status;
    // Remove the publish button if the comment has been published.
    if (status.toLowerCase() === "Published".toLowerCase()) {
        const actions = document.querySelector(`.comment-actions-container[data-comment-id="${comment.commentId}"]`);
        const publishButton = document.querySelector(`.publish-comment[data-comment-id="${comment.commentId}"]`);
        publishButton.remove();
        actions.innerHTML += `<button type="button" class="save-comment btn btn-success" data-comment-id="${comment.commentId}">Save</button>`;
    }
    // Reload all the button functionality
    init();
}

/**
 * @author Sander Verheyen
 * If a comment has been removed set all the fields to disabled.
 * @param commentId
 */
function disableRemoved(commentId) {
    // Get all the field that need to be disabled
    const textArea = document.querySelector(`.text-area[data-comment-id="${commentId}"]`);
    const deleteTags = document.querySelectorAll(`.delete-tag[data-comment-id="${commentId}"]`);
    const addTag = document.querySelector(`.add-tag[data-comment-id="${commentId}"]`);
    const deleteButton = document.querySelector(`.delete-comment[data-comment-id="${commentId}"]`);
    const saveButton = document.querySelector(`.save-comment[data-comment-id="${commentId}"]`);
    const publishButton = document.querySelector(`.publish-comment[data-comment-id="${commentId}"]`);
    textArea.disabled = true;
    // Set everything to disabled.
    for (let i = 0; i < deleteTags.length; i++) {
        deleteTags[i].disabled = true;
    }
    if (saveButton === null) {
        publishButton.disabled = true;
    }
    if (publishButton === null) {
        saveButton.disabled = true;
    }
    addTag.disabled = true;
    deleteButton.disabled = true;
}

/**
 * Adds the eventlisteners for the filters.
 */
function addEventListenersFilters() {
    
    // On any input change. set the hidden input to true.
    const filterInputs = document.querySelectorAll(".filter-input");
    filterInputs.forEach(input => {
        input.addEventListener("input", () => {
            document.querySelector("#hasFilterChanged").value = "true";
        });
    });
    
} // addEventListenersFilters.


/**
 * Adds event listeners for the pagination.
 */
function addEventListenersPagination() {

    const pageButtons = document.querySelectorAll(".btn-page-change");

    pageButtons.forEach(function (button) {
        button.addEventListener("click", changePage);
        button.btn = button;
    });
}

/**
 * Changes the page
 * @param evt
 */
function changePage(evt) {
    evt.preventDefault();
    
    // get button
    const button = evt.currentTarget.btn;
    const page = button.dataset.page;
    
    // get 'currentPageInput' by id
    const currentPageInput = document.querySelector("#currentPageInput");
    
    switch (page) {
        case "prev":
            currentPageInput.value = parseInt(currentPageInput.value) - 1;
            break;
        case "next":
            currentPageInput.value = parseInt(currentPageInput.value) + 1;
            break;
        default:
            currentPageInput.value = parseInt(page);
            break;
    }
    
    // Submit form with id 'formFilter'
    document.querySelector("#formFilter").submit();
}