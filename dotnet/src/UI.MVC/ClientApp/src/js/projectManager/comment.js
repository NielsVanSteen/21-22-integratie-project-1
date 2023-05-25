import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

popup.init()

const buttons = document.querySelectorAll(".view-comment")
const profilePicture = document.getElementById("view-comment-profilePicture")
const userName = document.getElementById("view-comment-fullname")
const date = document.getElementById("view-comment-date")
const quote = document.getElementById("view-comment-quote")
const commentText = document.getElementById("view-comment-comment-text")
const emojisPopUp = document.getElementById("view-comment-comment-emoji")

for (let i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener('click',()=>
    {
        showComment(buttons[i].dataset.commentsId)
    })
}

async function showComment(commentId) {
    //Get comment
    const response = await fetch(url.url() + url.getProjectName() + "/Comments/GetDetailComment/" + commentId, {
        method: "GET"
    })
    const responseText = await response.json()
    profilePicture.src = responseText.profilePicture
    userName.innerText = responseText.userName
    date.innerText = responseText.date
    quote.innerText = responseText.quote
    commentText.innerText = responseText.commentText
    showEmojis(responseText.emojis)
    popup.showPopup(".article-popup-view-comment")
    
}

function showEmojis(emojis){
    //Convert the object to an map
    const map = objectToMap(emojis)
    if(map.size > 0){
        //Add the html to see an emoji
        map.forEach((values,keys)=>{
            emojisPopUp.innerHTML +=`
                <li class="shown-emoji">
                       ${values} <button class="add-emoji-button btn-icon">${String.fromCodePoint(keys)}</button>
                </li>`
        })
    }
}

function objectToMap (obj){
    const keys = Object.keys(obj);
    const map = new Map();
    for(let i = 0; i < keys.length; i++){
        map.set(keys[i], obj[keys[i]]);
    }
    return map;
}
