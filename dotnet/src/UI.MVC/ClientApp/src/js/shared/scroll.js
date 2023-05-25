let prevTime;

/**
 * Function that scrolls to a y-position with an animation.
 * -> not all browser support the css property: 'scroll-behavior: smooth;'
 * -> and with scroll-behavior I can't choose the time either.
 *
 * @author Niels Van Steen
 * */
export function scrollSmooth(y, time) {

    prevTime = Date.now();

    let curScroll = window.scrollY;
    const difference = curScroll - y;

    const increment = 1;
    test(curScroll, increment, y, difference, time);
}

let oldScroll = 0;

function test(curScroll, increment, y, difference, time) {

    const curTime = Date.now();
    const timeDifference = (curTime - prevTime + 1) * 10;
    
    const moveAmount = Math.abs(difference) / time * timeDifference;
    curScroll += moveAmount;
    window.scrollTo(window.scrollX, curScroll);

    // Recall function.
    setTimeout(() => {
      
        if (curScroll < y) {
            prevTime = Date.now();
            oldScroll = curScroll;
            test(curScroll, increment, y, difference, time);
        }
    }, 1);

}


function currentYPosition() {
    // Firefox, Chrome, Opera, Safari
    if (self.pageYOffset) return self.pageYOffset;
    // Internet Explorer 6 - standards mode
    if (document.documentElement && document.documentElement.scrollTop)
        return document.documentElement.scrollTop;
    // Internet Explorer 6, 7 and 8
    if (document.body.scrollTop) return document.body.scrollTop;
    return 0;
}


function elmYPosition(eID) {
    let elm = document.getElementById(eID);
    if (elm == null)
        elm = document.querySelector(eID);
    
    let y = elm.offsetTop;
    let node = elm;
    while (node.offsetParent && node.offsetParent !== document.body) {
        node = node.offsetParent;
        y += node.offsetTop;
    } return y;
}


export default function smoothScroll(eID, offset) {
    let startY = currentYPosition();
    let stopY = elmYPosition(eID) - offset;
    let distance = stopY > startY ? stopY - startY : startY - stopY;
    if (distance < 100) {
        scrollTo(0, stopY); return;
    }
    let speed = Math.round(distance / 100);
    if (speed >= 20) speed = 20;
    let step = Math.round(distance / 25);
    let leapY = stopY > startY ? startY + step : startY - step;
    let timer = 0;
    if (stopY > startY) {
        for ( let i=startY; i<stopY; i+=step ) {
            setTimeout("window.scrollTo(0, "+leapY+")", timer * speed);
            leapY += step; if (leapY > stopY) leapY = stopY; timer++;
        } return;
    }
    for ( let i=startY; i>stopY; i-=step ) {
        setTimeout("window.scrollTo(0, "+leapY+")", timer * speed);
        leapY -= step; if (leapY < stopY) leapY = stopY; timer++;
    }
}
