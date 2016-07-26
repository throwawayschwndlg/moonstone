moonstone.json = {};
moonstone.json.antiForgeryToken = null;

moonstone.json.get = function (url, successCallback, errorCallback, alwaysCallback) {
    if (successCallback == null) {
        successCallback = moonstone.json.defaultSuccess;
    }
    if (errorCallback == null) {
        errorCallback = moonstone.json.defaultError;
    }
    if (alwaysCallback == null) {
        alwaysCallback = moonstone.json.defaultAlways;
    }

    moonstone.log(sprintf('Requesting JSON from %s', url));

    $.getJSON(url)
        .done(function (result) {
            if (result.success) {
                successCallback(result);
            }
            else {
                errorCallback(result);
            }
        })
        .fail(errorCallback)
        .always(alwaysCallback);
}

moonstone.json.post = function (url, data, successCallback, errorCallback, alwaysCallback) {
    if (successCallback == null) {
        successCallback = moonstone.json.defaultSuccess;
    }
    if (errorCallback == null) {
        errorCallback = moonstone.json.defaultError;
    }
    if (alwaysCallback == null) {
        alwaysCallback = moonstone.json.defaultAlways;
    }

    data.__RequestVerificationToken = moonstone.json.antiForgeryToken;

    moonstone.log(sprintf('Posting JSON to %s', url));
    moonstone.log(data);

    $.post(url, data)
        .done(function (result) {
            if (result.success) {
                successCallback(result);
            }
            else {
                errorCallback(result);
            }
        })
        .fail(errorCallback)
        .always(alwaysCallback);
};

moonstone.json.defaultSuccess = function (response) {
    moonstone.log('JSON Success.');
};

moonstone.json.defaultError = function (response) {
    moonstone.log('JSON Error.');
};

moonstone.json.defaultAlways = function (response) {
    moonstone.log(response);
}