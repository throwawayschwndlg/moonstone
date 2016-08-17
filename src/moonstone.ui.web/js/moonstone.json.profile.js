moonstone.json.profile = {};

moonstone.json.profile.getProfileInformation = function (successCallback, errorCallback, alwaysCallback) {
    moonstone.json.get(
        moonstone.json.urls.getProfileInformation,
        successCallback,
        errorCallback,
        alwaysCallback);
};

moonstone.json.profile.setTimeZone = function (timeZone, successCallback, errorCallback, alwaysCallback) {
    moonstone.json.post(
        moonstone.json.urls.setTimeZone,
        {
            timeZone: timeZone
        },
        successCallback,
        errorCallback,
        alwaysCallback
        );
};