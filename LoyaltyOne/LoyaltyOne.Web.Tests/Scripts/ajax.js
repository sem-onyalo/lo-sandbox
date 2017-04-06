var httpRequest;

function ajaxRequest(url, callback) {
    httpRequest = new XMLHttpRequest();

    if (!httpRequest) {
        alert('Failed to create an AJAX request');
        return false;
    }
    httpRequest.onreadystatechange = function () { ajaxResponse(callback); };
    httpRequest.open('GET', url);
    httpRequest.send();
}

function ajaxResponse(callbackFunction) {
    if (httpRequest.readyState === XMLHttpRequest.DONE) {
        if (httpRequest.status === 200) {
            callbackFunction(httpRequest.responseText);
        } else {
            callbackFunction(httpRequest.status + ': ' + httpRequest.responseText);
        }
    }
}