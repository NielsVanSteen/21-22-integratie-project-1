window.addEventListener("load", init);

import scroll  from "./../shared/scroll.js";

/**
 * The time in milliseconds to scroll from 1 phase to the next.
 * This is multiplied by the amount of phases in between.
 *
 * E.g., from phase 1 to 2 = (2-1 = 1) -> time * 1
 * E.g., from phase 1 to 3 = (3-1 = 2) -> time * 2;
 *
 * @author Niels Van Steen
 * */
const time = 500;

/**
 * The previous index of the selected nav item. -> this is by default 0 (the first nav item).
 * This is used to calculate the scroll time when scrolling to the new item (when scrolling past a lot of 'intermediate' phases
 * e.g., 1 -> 5 the time should be longer then when scrolling for example from 1 -> 2.
 *
 * @author Niels Van Steen
 * */
let prevIndex = 0;

/**
 * When scrolling to the other phase, there is a little animation that fist scrolls a bit in the other direction.
 * and after scrolling to the new item, it also goes a bit to far and then comes back.
 * This constant is the time these 2 animations take.
 *
 * @author Niels Van Steen
 * */
const intermediateTime = 300;

/**
 * This constant defines by how much {@link intermediateTime} is affected.
 * @author Niels Van Steen
 * */
let intermediateAmount = 15;

/**
 * Spam clicking the nav items kinda breaks the nice animation -> this value 'locks' the function untill
 * the animation has finished.
 *
 * @author Niels Van Steen
 * */
let isLocked = false;

/**
 * Class that when given to a nav item container element, that element will have difference css. (that of the active nav element).
 *
 * @author Niels Van Steen
 * */
const activeNavClass = "timeline-nav-active";


/**
 * Init function executed on page load.
 *
 * @author Niels Van Steen
 * */
function init() {

    // all the event listeners (the timeline phases excluded).
    addEventListeners();

    // All the functionality for scrolling through the different timeline phases.
    initTimeLine();
} // Init.

/**
 * Add the event listeners.
 *
 * @author Niels Van Steen
 * */
function addEventListeners() {
    const btnExplore = document.querySelector(".explore-landing-page");
    
    if (btnExplore == null)
        return;
    
    btnExplore.addEventListener("click", () => {
        //const height = window.innerHeight;
        //window.scrollTo(0, height);
        scroll("timelineContainer");
    });
}

/**
 * All the initialization to make clicking through all the timeline phases work.
 *
 * @author Niels Van Steen
 * */
function initTimeLine() {
    // Get the timeline navigation items & the corresponding timeline phase items.
    const timelineNavButtons = document.querySelectorAll(".btn-timeline-navigation-item");
    const timelineItems = document.querySelectorAll(".timeline-phase-wrapper");

    // There are no timeline phases -> return.
    if (timelineNavButtons === undefined || timelineItems === undefined)
        return;
    if (timelineNavButtons.length < 1 || timelineItems.length < 0)
        return;

    animateActiveTimeLine(0, 0, 0);

    // Give the first item the active class.
    const navs = document.querySelectorAll(".timeline-navigation-item-container");
    navs[0].classList.add(activeNavClass);

    // The length should be the same, so we could take either length, this takes one, and might prevent an error in the console if for some reason the lengths aren't the same.
    const min = Math.min(timelineNavButtons.length, timelineItems.length);

    // Add the event listeners for each item.
    for (let i = 0; i < min; i++) {
        const timelineNav = timelineNavButtons[i];
        const phaseItem = timelineItems[i];

        timelineNav.addEventListener("click", timelineNavClicked);
        timelineNav.navItem = timelineNav;
        timelineNav.index = i;
        timelineNav.phase = phaseItem;
    } // For.
}

/**
 * This function is invoked when a nav item has been clicked.
 *
 * @author Niels Van Steen
 * */
function timelineNavClicked(evt) {
    // Get the index of the clicked item & the container element containing all the phases.
    const index = evt.currentTarget.index;
    const containerElement = document.querySelector(".timeline-list-wrapper");

    // Previous animation hasn't finished -> return.
    if (isLocked)
        return;
    isLocked = true;

    // Calculate animation variables.
    const decrease = time / (0.5 * Math.abs(index - prevIndex) ** 1.2); // Exponential function that will decrease the amount of time when the difference between index en prevIndex gets bigger.
    const t = (decrease * Math.abs(index - prevIndex));  // t = animation time.
    const amount = -index * 100; //  amount is the new amount the css property 'top' will get.
    const prevAmount = -prevIndex * 100; // prevAmount is the current value of the css property 'top'.

    // When clicking the same item return -> no animation needed. the item is already active.
    if (amount === prevAmount)
        return;

    // Change the active navigation item with an animation.
    animateActiveTimeLine(prevIndex, index, intermediateTime * 2 + t);
    animateContent(prevIndex, index, t);

    // The animation scrolls in the opposite direction at the beginning to give a nice animation
    // intermediate determines the amount -> should be negative when scrolling scrolling down, and positive when scrolling up.
    let intermediate = intermediateAmount;
    if (amount > prevAmount)
        intermediate = -intermediateAmount;

    // Scroll in the opposite direction at the beginning.
    containerElement.style.transition = `top ${intermediateTime}ms ease`;
    containerElement.style.top = (prevAmount + intermediate) + "%";

    // The actual scroll down. (this scrolls a bit to far -> on purpose)
    setTimeout(() => {

        containerElement.style.transition = `top ${t}ms ease`;
        containerElement.style.top = (amount - intermediate) + "%";
    }, intermediateTime);

    // the animation scrolled a bit too far -> this animation scrolls it back up at the position is should be at.
    setTimeout(() => {

        containerElement.style.transition = `top ${intermediateTime}ms ease`;
        containerElement.style.top = (amount) + "%";

        // Update the previous index.
        prevIndex = index;
    }, intermediateTime + t);

    // Unlock the function.
    setTimeout(() => {
        isLocked = false;
    }, intermediateTime * 2 + t)

} // timelineNavClicked.

/**
 * Change the active navigation item with an animation.
 *
 * @author Niels Van Steen
 * */
function animateActiveTimeLine(prevIndex, index, time) {
    // Change the active link.
    const navs = document.querySelectorAll(".timeline-navigation-item-container");
    const prevNav = navs[prevIndex];
    const newNav = navs[index];

    // Remove the active class from the current navigation item.
    navs[prevIndex].classList.remove(activeNavClass);

    const wrapper = document.querySelector(".timeline-navigation-wrapper");
    wrapper.style.overflowY = "hidden";

    const container = document.querySelector(".timeline-navigation");
    const containerDistance = container.getBoundingClientRect().top;
    const distance = newNav.getBoundingClientRect().top;
    const difference = distance - containerDistance - 3;

    const circle = document.querySelector(".active-circle");
    circle.style.opacity = 1;
    circle.style.transition = `top ${time}ms ease`;
    circle.style.top = difference + "px";

    setTimeout(() => {
        navs[index].classList.add(activeNavClass);
        circle.style.opacity = 0;
        wrapper.style.overflowY = "auto";
    }, time)
} // animateActiveTimeLine.

/**
 * When scrolling through the timeline phases the content of each phases is faded in and out.
 *
 * @author Niels Van Steen
 * */
function animateContent(prevIndex, curIndex, time) {
    const phases = document.querySelectorAll(".timeline-phase-wrapper");
    const prevPhase = phases[prevIndex];
    const phase = phases[curIndex];

    fade(prevPhase, true, time);
    fade(phase, true,time + intermediateTime);
} // animateContent.

/**
 * Fade the information about a specific timeline phase in/out.
 *
 * @author Niels Van Steen
 * */

function fade(phase, isFadeOut, time) {
    const h3 = phase.querySelector("h3");
    const p = phase.querySelector("p");
    const h2 = phase.querySelector("h2");
    const div = phase.querySelector(".explore-phase-container a");

    let i1 = "1";
    let i2 = "0";

    if (isFadeOut) {
        i1 = "0";
        i2 = "1";
    }

    h3.style.opacity = i1;
    setTimeout(() => {
        h2.style.opacity = i1;
    }, 100);

    setTimeout(() => {
        p.style.opacity = i1;
    }, 200);

    if (div != null) {
        setTimeout(() => {
            div.style.opacity = i1;
        }, 300);
    }


    if (isFadeOut) {
        setTimeout(() => {
            h2.style.opacity = i2;
            h3.style.opacity = i2;
            p.style.opacity = i2;
            if (div != null) {
                div.style.opacity = i2;
            }

        }, time);
    }
}














