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

var subIdPrefix = "liSub";
var repliesIdPrefix = "ulReplies";
var htmlSubs = document.getElementById('userSubmissions');
function getTextsRequestCallback(response) {
    var subs = "<ul>";
    var isReply = false;
    for (var i = 0; i < response.length; i++) {
        if (response[i].parentid == 0) {
            if (isReply) {
                subs += "</ul>";
                isReply = false;
            }

            subs += ('<li id="' + subIdPrefix + response[i].id + '">'
                + response[i].text
                + '<br /><input type="text" id="inputReply' + response[i].id
                + '" />&nbsp;<button onclick="return ButtonReply_Click({ id: ' + response[i].id + ' })">Reply</button><br />'
                + "</li>"
            );
        } else {
            if (!isReply) {
                subs += '<ul id="' + repliesIdPrefix + response[i].parentid + '">'
                isReply = true;
            }

            subs += "<li>" + response[i].text + "</li>";
        }
    }
    subs += "</ul>";

    htmlSubs.innerHTML = subs;
}

function postTextRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var parentId = args.parentId;
    var value = args.value;
    var name = args.name;

    if (value === undefined || value === '') {
        responseCallback('');
    } else {
        var content = { Name: name, Text: value, ParentId: parentId };
        ajaxRequest(apiBaseUrl + 'v1/text', 'POST', JSON.stringify(content), function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj);
        });
    }
}

function postReplyRequestCallback(response) {
    var item = document.createElement("li");
    var text = document.createTextNode(response.text);
    item.appendChild(text);

    if (document.getElementById(repliesIdPrefix + response.parentid) == undefined) {
        var list = document.createElement("ul");
        list.id = repliesIdPrefix + response.parentid;
        list.appendChild(item);

        document.getElementById(subIdPrefix + response.parentid).appendChild(list);
    } else {

        document.getElementById(repliesIdPrefix + response.parentid).appendChild(item);
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

function ButtonReply_Click(args) {
    var imgLoadingText = document.getElementById("imgLoadingText");
    var inputReply = document.getElementById('inputReply' + args.id);

    if (inputReply.value.trim() != '') {
        imgLoadingText.style.display = "";
        postTextRequest({ apiBaseUrl: apiBaseUrl, value: inputReply.value, name: '', parentId: args.id }, function (response) {
            imgLoadingText.style.display = "none";
            postReplyRequestCallback(response);
        });
    }
}

function ButtonGetSubs_Click() {
    var inputName = document.getElementById('inputName');
    var inputNameErrorMsg = document.getElementById("inputNameValidationMsg");

    var formObject = {
        "inputComponents": [
            {
                "component": inputName,
                "errorMsgComponent": inputNameErrorMsg,
                "errorMessage": "Please enter a name"
            }
        ]
    };
    
    if (!validateForm(formObject)) {
        imgLoadingText.style.display = "";
        getTextsRequest({ apiBaseUrl: apiBaseUrl, name: inputName.value }, function (response) {
            imgLoadingText.style.display = "none";
            getTextsRequestCallback(response);
        });
    }
}

function ButtonDone_Click(args) {
    var inputText = document.getElementById('inputText');
    var inputName = document.getElementById('inputName');
    var htmlDest = document.getElementById('textResponse');
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
                "errorMessage": "Please enter a name"
            }
        ]
    };

    if (!validateForm(formObject)) {

        imgLoadingText.style.display = "";
        postTextRequest({ apiBaseUrl: apiBaseUrl, value: inputText.value, name: inputName.value, parentId: 0 }, function (response) {
            htmlDest.innerHTML = response.text;
            imgLoadingText.style.display = "none";

            getTextsRequest({ apiBaseUrl: apiBaseUrl, name: inputName.value }, function (response) {
                getTextsRequestCallback(response);
            });
        });
    }

    return false;
}