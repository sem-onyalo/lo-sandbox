var httpRequest;

function ajaxRequest(url, method, content, callback) {
    httpRequest = new XMLHttpRequest();

    if (!httpRequest) {
        alert('Failed to create an AJAX request');
        return false;
    }
    httpRequest.onreadystatechange = function () { ajaxResponse(callback); };
    httpRequest.open(method, url);
    httpRequest.setRequestHeader("Content-Type", "application/json");
    httpRequest.send(content);
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