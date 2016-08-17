using moonstone.core.models;
using moonstone.core.services.results;
using System;
using System.Collections.Generic;

namespace moonstone.core.services
{
    public interface ICurrencyService
    {
        IEnumerable<Currency> GetAllCurrencies();

        ExchangeRateResult GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode, DateTime utcDate);
    }
}