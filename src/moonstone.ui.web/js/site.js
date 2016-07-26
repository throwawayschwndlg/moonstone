var user_profile_info = {};

$(function () {
    init();
});

function init() {
    // profile agnostic inits
    initSemanticUi();
    initToastr();
    initOffline();
    initMoonstone();

    // profile based inits / settings
    loadUserProfile(function () {
        updateTimeZone();
        initPikaday();
    });
}

function initSemanticUi() {
    // api
    $.fn.api.settings.api = {
        'api-login': '/user/login',
        'api-create-bankaccount': '/bankaccount/create',
        'api-create-category': '/category/create',
        'api-create-group': '/group/create',
        'api-create-transaction': '/transaction/create',
        'api-create-expense': '/transaction/expense',
        'api-get-profile-info': '/User/GetProfileInformation',
        'api-get-categories-for-current-group': '/category/GetAllCategoriesForCurrentGroup',
        'api-get-bankaccounts-for-current-user': '/bankaccount/GetAllBankAccountsForCurrentUser',
        'api-get-timezones': '/user/getTimeZones',
        'api-get-currencies': '/currency/GetAllCurrencies',
        'api-update-profile-settings': '/user/settings',
    };
    $.fn.api.settings.successTest = function (response) {
        if (response && response.success) {
            return response.success;
        }
        return false;
    };
    $.fn.api.settings.onSuccess = handleApiSuccess;
    $.fn.api.settings.onFailure = handleApiFailure;

    // nav dropdowns
    $('.ms-nav-profile-dropdown').dropdown();
    $('.ms-nav-group-dropdown').dropdown();

    // checkboxes
    $('.ui.checkbox').checkbox({
        onChecked: function () { $(this).val(true); },
        onUnchecked: function () { $(this).val(false); }
    });
    // checkboxes - this is some true shit. fix this ASAP
    $.each($('.ui.checkbox').find('input'), function () {
        if ($(this).val() == "True") {
            $(this).parent().checkbox('check');
        }
        else {
            $(this).parent().checkbox('uncheck');
        }
    });

    // icon toolbar tooltips
    $(".ms-icon-bar .item").popup();
    moonstone.log('semantic initialized');
}

function initToastr() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function initOffline() {
    if (moonstone.isDebug) {
        Offline.options =
            {
                checks:
                    {
                        xhr: { url: 'https://httpbin.org/ip' }
                    },
                checkOnLoad: true
            };
    }

    Offline.on('down', function () {
        moonstone.log('connection lost');
        displayMessage('error', 'connection lost', 'Connection lost.');
    });
    Offline.on('up', function () {
        moonstone.log('reconnected');
        displayMessage('success', 'connection has been restored', 'Connection has been restored.');
    });

    moonstone.log('Offline initialized.');
}

function initMoonstone() {
    moonstone.json.antiForgeryToken = $('[name=__RequestVerificationToken]').first().val();
};

function initPikaday() {
    $('.ms-date-selector').each(function () {
        // this is the picker
        var $picker = $(this);
        // this field holds the actual value
        var $hidden = $(this).prev('input');
        // set the default date of the picker
        $picker.val(moment($hidden.val()).format(user_profile_info.dateFormat));
        // initialize pikaday
        $picker.pikaday({
            format: user_profile_info.dateFormat
        });
        // register a change handler on picker
        $picker.change(function () {
            // first, we need to get the pikaday instance...
            var pika = $(this).data('pikaday');
            // format the selected date to our default format.
            $hidden.val(pika.getMoment().format('YYYY-MM-DD')).trigger('change');
        });
    });

    moonstone.log('pikaday initialized');
};

function updateTimeZone() {
    if (user_profile_info.autoUpdateTimeZone) {
        var userTz = moment.tz.guess();
        moonstone.log(sprintf('Client timezone is %s', userTz));

        if (user_profile_info.timeZone == null || user_profile_info.timeZone != userTz) {
            moonstone.log('Difference in time zones. Updating profile timezone.');

            moonstone.json.profile.setTimeZone(userTz, function (response) {
                user_profile_info.timeZone = userTz;

                displaySuccess(null, response.message);
                moonstone.log(sprintf('User TimeZone has been updated to %s', user_profile_info.timeZone));
            });
        }
    }
    else {
        moonstone.log('TimeZone auto update is disabled.');
    }
}

function bindFormSubmit(formSelector, apiAction, callbackSuccess, callbackFailure) {
    var apiSettings = {
        action: apiAction,
        method: 'POST',
        serializeForm: true,
        stateContext: formSelector
    };

    if (callbackSuccess !== null) {
        apiSettings.onSuccess = callbackSuccess;
    }

    if (callbackFailure !== null) {
        apiSettings.onFailure = callbackFailure;
    }

    $(formSelector + ' .submit.button').api(apiSettings);

    $('.submit.button').on('click', function () {
        $(formSelector).valid();
    });
}

function handleApiFailure(response) {
    moonstone.log(response);
    displayMessage('error', 'Error', response.message);
}

function handleApiSuccess(response) {
    //displayMessage('success', 'Success', response.message);

    if (response.returnUrl !== null) {
        window.location = response.returnUrl;
    }
}

function displayError(title, message) {
    displayMessage('error', title, message);
}

function displayWarning(title, message) {
    displayMessage('warning', title, message);
}

function displaySuccess(title, message) {
    displayMessage('success', title, message);
}

function displayInfo(title, message) {
    displayMessage('info', title, message);
}

function displayMessage(type, title, message) {
    switch (type) {
        case 'error':
            //toastr.error(message, title);
            toastr.error(message);
            break;
        case 'warning':
            //toastr.warning(message, title);
            toastr.warning(message);
            break;
        case 'success':
            toastr.success(message);
            break;
        case 'info':
        default:
            //toastr.info(message, title);
            toastr.info(message);
    }
}

function initModal(modalSelector, triggerSelector) {
    $(modalSelector)
        .modal({
            transition: 'horizontal flip',
            autofocus: true
        })
        .modal('attach events', triggerSelector, 'show')
    ;
}

function bindDropdown(selector, apiAction, callbackSuccess) {
    var $element = $(selector);
    var $items = $element.find('.menu');

    if (callbackSuccess == null) {
        callbackSuccess = function (response) {
            $.each(response.data, function (index, v) {
                var $item = $('<div class="item" data-value="' + v.value + '">' + v.name + '</div>');
                if (v.description != null) {
                    var $description = $('<span class="description">' + v.description + '</span>');
                    $item.append($description);
                }

                $items.append($item);
            });

            $element.dropdown();
        };
    }

    $element.api({
        on: 'now',
        action: apiAction,
        onSuccess: callbackSuccess
    });
}

function hideElement($element) {
    $element.hide();
}

function showElement($element) {
    $element.show();
}

function registerChangeListener($element, callback) {
    moonstone.log('registering change listener for ' + $element.attr('id'));
    $element.change(callback);
}

function loadUserProfile(successCallback) {
    moonstone.json.profile.getProfileInformation(null, null, function (response) {
        user_profile_info = response.data;

        moonstone.log(user_profile_info);

        successCallback();
    });
}