// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Get the parent that has parentClass from currentElement
// Return the parent element
function getParent(parentClass, currentElement) {

    // Keep getting the parent until element has the targetClass
    while (currentElement.getAttribute("class") == null || !currentElement.getAttribute("class").includes(parentClass)) {
        currentElement = currentElement.parentNode;
    }

    return currentElement;
}