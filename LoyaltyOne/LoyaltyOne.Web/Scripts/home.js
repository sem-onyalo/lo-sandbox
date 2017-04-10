var apiBaseUrl = "http://localhost:81/api/";

function getTextRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var value = args.value;
    
    if (value === undefined || value === '') {
        responseCallback('');
    } else {
        ajaxRequest(apiBaseUrl + 'v1/text/' + value, 'GET', undefined, function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj.text);
        });
    }
}

function getTextsRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var name = args.name;

    if (name === undefined || name === '') {
        responseCallback('');
    } else {
        ajaxRequest(apiBaseUrl + 'v1/texts/' + name, 'GET', undefined, function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj.texts);
        });
    }
}

function postTextRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var value = args.value;
    var name = args.name;

    if (value === undefined || value === '' || name === undefined || name === '') {
        responseCallback('');
    } else {
        var content = { Name: name, Text: value };
        ajaxRequest(apiBaseUrl + 'v1/text', 'POST', JSON.stringify(content), function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj.text);
        });
    }
}

function validateForm(formObject) {
    var isError = false;

    for (var i = 0; i < formObject.inputComponents.length; i++) {
        if (formObject.inputComponents[i].component.value == '') {
            isError = true;
            formObject.inputComponents[i].errorMsgComponent.style.display = "";
            formObject.inputComponents[i].errorMsgComponent.innerHTML = formObject.inputComponents[i].errorMessage;
        } else {
            formObject.inputComponents[i].errorMsgComponent.style.display = "none";
        }
    }

    return isError;
}

function ButtonDone_Click(args) {
    var inputText = document.getElementById('inputText');
    var inputName = document.getElementById('inputName');
    var htmlDest = document.getElementById('textResponse');
    var htmlSubs = document.getElementById('userSubmissions');
    var imgLoadingText = document.getElementById("imgLoadingText");
    var inputTextErrorMsg = document.getElementById("inputTextValidationMsg");
    var inputNameErrorMsg = document.getElementById("inputNameValidationMsg");

    var formObject = {
        "inputComponents": [
            {
                "component": inputText,
                "errorMsgComponent": inputTextErrorMsg,
                "errorMessage": "Please enter some text"
            },
            {
                "component": inputName,
                "errorMsgComponent": inputNameErrorMsg,
                "errorMessage": "Please enter your name"
            }
        ]
    };

    if (!validateForm(formObject)) {

        imgLoadingText.style.display = "";
        postTextRequest({ apiBaseUrl: apiBaseUrl, value: inputText.value, name: inputName.value }, function (response) {
            htmlDest.innerHTML = response;
            imgLoadingText.style.display = "none";

            getTextsRequest({ apiBaseUrl: apiBaseUrl, name: inputName.value }, function (response) {
                var subs = "<ul>";
                for (var i = 0; i < response.length; i++) {
                    subs += "<li>" + response[i] + "</li>";
                }
                subs += "</ul>";

                htmlSubs.innerHTML = subs;
            });
        });
    }

    return false;
}