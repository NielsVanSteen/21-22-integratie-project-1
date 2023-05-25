import * as popup from "./popup.js";
window.addEventListener("load", init)


function init(){
    const buttons = document.querySelectorAll(".btn-accpt-cookie")
    
    if(localStorage.getItem('CookiePopup') !== "true"){
        popup.init()
        popup.showPopup(".article-popup-cookie")
    }

    
    buttons.forEach( button =>{
        button.addEventListener('click', ()=>{
            localStorage.setItem('CookiePopup', "true");
        })
    })
    
}
