/*
 * dependecies:
 * - jquery
 * - sprintf
 */

var moonstone = {
    isDebug: true
};

moonstone.log = function (message) {
    if (moonstone.isDebug) {
        console.log(message);
    }
};