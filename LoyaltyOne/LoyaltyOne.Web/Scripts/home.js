var apiBaseUrl = "http://localhost:81/api/";

function getTextRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var value = args.value;
    
    if (value === undefined || value === '') {
        responseCallback('');
    } else {
        ajaxRequest(apiBaseUrl + 'v1/text/' + value, function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj.text);
        });
    }
}

function ButtonDone_Click(args) {
    var inputSrc = document.getElementById('inputText');
    var htmlDest = document.getElementById('textResponse');
    var loadingImg = document.getElementById("imgLoadingText");

    loadingImg.style.display = "";
    getTextRequest({ apiBaseUrl: apiBaseUrl, value: inputSrc.value }, function (response) {
        htmlDest.innerHTML = response;
        loadingImg.style.display = "none";
    });

    return false;
}