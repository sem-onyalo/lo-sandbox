var apiBaseUrl = "http://localhost:81/api/";

function getTextRequest(args, responseCallback) {
    var apiBaseUrl = args.apiBaseUrl;
    var value = args.value;
    
    if (name === null || value === undefined || value === '') {
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

    if (name === null || name === undefined || name === '') {
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
        if (response[i].parentid === '0') {
            if (isReply) {
                subs += "</ul>";
                isReply = false;
            }

            subs += ('<li id="' + subIdPrefix + response[i].id + '">'
                + response[i].text
                + '<span style="font-size:0.8em;color:#666;">&nbsp;&nbsp;'
                + response[i].city.charAt(0).toUpperCase() + response[i].city.slice(1)
                + ' (' + response[i].lat + ',' + response[i].lon + ') ' + response[i].temp + '\u00B0'
                + '</span>'
                + '<br /><input type="text" id="inputReply' + response[i].id + '" placeholder="reply" />'
                + '&nbsp;<input type="text" id="inputReplyCity' + response[i].id + '" placeholder="city" />'
                + '&nbsp;<button onclick="return ButtonReply_Click({ id: ' + response[i].id + ' })">Submit</button><br /><br />'
                + "</li>");
        } else {
            if (!isReply) {
                subs += '<ul id="' + repliesIdPrefix + response[i].parentid + '" style="padding: 0 20px 20px 20px;">'
                isReply = true;
            }

            subs += ("<li>"
                + response[i].text
                + '<span style="font-size:0.8em;color:#666;">&nbsp;&nbsp;'
                + response[i].city.charAt(0).toUpperCase() + response[i].city.slice(1)
                + ' (' + response[i].lat + ',' + response[i].lon + ') ' + response[i].temp + '\u00B0'
                + '</span>'
                + "</li>");
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
    var city = args.city;

    if (value === null || value === undefined || value === '') {
        responseCallback('');
    } else {
        var content = { Name: name, Text: value, City: city, ParentId: parentId };
        ajaxRequest(apiBaseUrl + 'v1/text', 'POST', JSON.stringify(content), function (response) {
            var jsonObj = JSON.parse(response.toLowerCase());
            responseCallback(jsonObj);
        });
    }
}

function postReplyRequestCallback(response) {
    var item = document.createElement("li");
    var text = document.createTextNode(response.data.text);

    var loc = document.createElement("span");
    var locText = document.createTextNode('\u00A0\u00A0' + response.data.city.charAt(0).toUpperCase() + response.data.city.slice(1)
        + ' (' + response.data.lat + ',' + response.data.lon + ') ' + response.data.temp + '\u00B0');

    loc.style = "font-size:0.8em;color:#666;";
    loc.appendChild(locText);

    item.appendChild(text);
    item.appendChild(loc);

    if (document.getElementById(repliesIdPrefix + response.data.parentid) === null) {
        var list = document.createElement("ul");
        list.id = repliesIdPrefix + response.data.parentid;
        list.style = "padding: 0 20px 20px 20px;";
        list.appendChild(item);

        document.getElementById(subIdPrefix + response.data.parentid).appendChild(list);
    } else {

        document.getElementById(repliesIdPrefix + response.data.parentid).appendChild(item);
    }
}

function validateForm(formObject) {
    var isError = false;

    for (var i = 0; i < formObject.inputComponents.length; i++) {
        if (formObject.inputComponents[i].component.value === '') {
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
    var inputCity = document.getElementById('inputReplyCity' + args.id);

    if (inputReply.value.trim() !== '' && inputCity.value.trim() !== '') {
        imgLoadingText.style.display = "";

        var postContent = { apiBaseUrl: apiBaseUrl, value: inputReply.value, name: '', city: inputCity.value, parentId: args.id };
        postTextRequest(postContent, function (response) {
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
    var inputCity = document.getElementById('inputCity');
    var htmlDest = document.getElementById('textResponse');
    var imgLoadingText = document.getElementById("imgLoadingText");
    var inputTextErrorMsg = document.getElementById("inputTextValidationMsg");
    var inputNameErrorMsg = document.getElementById("inputNameValidationMsg");
    var inputCityErrorMsg = document.getElementById("inputCityValidationMsg");

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
            },
            {
                "component": inputCity,
                "errorMsgComponent": inputCityErrorMsg,
                "errorMessage": "Please enter a city"
            }
        ]
    };

    if (!validateForm(formObject)) {

        imgLoadingText.style.display = "";

        var postContent = { apiBaseUrl: apiBaseUrl, value: inputText.value, name: inputName.value, city: inputCity.value, parentId: 0 };
        postTextRequest(postContent, function (response) {
            htmlDest.innerHTML = response.data.text;
            imgLoadingText.style.display = "none";

            getTextsRequest({ apiBaseUrl: apiBaseUrl, name: inputName.value }, function (response) {
                getTextsRequestCallback(response);
            });
        });
    }

    return false;
}