moonstone.json.urls = {
    // profile
    getProfileInformation: '/user/getProfileInformation',
    setTimeZone: '/user/setTimeZone',
    // currencies
    getExchangeRate: '/currency/getExchangeRate?baseCurrency=%s&targetCurrency=%s&conversionDate=%s',
    getExchangeRateForExpense: '/currency/getExchangeRateForExpense?sourceBankAccoundId=%s&targetCurrency=%s&conversionDate=%s',
    getExchangeRateForIncome: '/currency/getExchangeRateForIncome?destinationBankAccountId=%s&currency=%s&conversionDate=%s'
};