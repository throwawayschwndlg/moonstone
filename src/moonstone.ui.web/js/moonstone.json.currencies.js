moonstone.json.currencies = {};
moonstone.json.currencies.getExchangeRate =
    function (baseCurrency, targetCurrency, conversionDate, successCallback, errorCallback, alwaysCallback) {
        var url = sprintf(moonstone.json.urls.getExchangeRate, baseCurrency, targetCurrency, conversionDate);

        moonstone.json.get(
            url,
            successCallback,
            errorCallback,
            alwaysCallback);
    }
moonstone.json.currencies.getExchangeRateForExpense =
    function (sourceBankAccoundId, targetCurrency, conversionDate, successCallback, errorCallback, alwaysCallback) {
        var url = sprintf(moonstone.json.urls.getExchangeRateForExpense, sourceBankAccoundId, targetCurrency, conversionDate);

        moonstone.log(url);

        moonstone.json.get(
            url,
            successCallback,
            errorCallback,
            alwaysCallback);
    }
moonstone.json.currencies.getExchangeRateForIncome =
    function (destinationBankAccountId, currency, conversionDate, successCallback, errorCallback, alwaysCallback) {
        var url = sprintf(moonstone.json.urls.getExchangeRateForIncome, destinationBankAccountId, currency, conversionDate);

        moonstone.json.get(
            url,
            successCallback,
            errorCallback,
            alwaysCallback);
    }