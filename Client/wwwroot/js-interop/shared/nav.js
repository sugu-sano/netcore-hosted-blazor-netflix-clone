/**
 * @param {HTMLElement} elm
 */
function addScrollEvent(elm) {
    window.addEventListener('scroll', function () {
        elm.classList.toggle('Nav-black', window.scrollY > 100)
    });
}