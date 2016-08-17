using moonstone.core.exceptions.serviceExceptions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using moonstone.core.services.results;
using moonstone.core.utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace moonstone.services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        // we are currently using fixer.io for the exchange rates. see: https://fixer.io

        // eg. https://api.fixer.io/2000-01-03?base=CHF
        protected const string FIXER_RATE_URL_BASE = "https://api.fixer.io/{0}?base={1}";

        public CurrencyService(RepositoryHub repoHub) : base(repoHub)
        {
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            var nodaCurrencies = NodaMoney.Currency.GetAllCurrencies().Where(c => !c.IsObsolete);
            var currencies = nodaCurrencies.Select(c =>
                new Currency
                {
                    Code = c.Code,
                    LocalizedName = c.EnglishName, // TODO: Get localized name
                    Symbol = c.Symbol
                })
                .OrderBy(c => c.Code);

            return currencies;
        }

        public ExchangeRateResult GetExchangeRate(string baseCurrencyCode, string targetCurrencyCode, DateTime conversionDateUtc)
        {
            if (string.IsNullOrWhiteSpace(baseCurrencyCode) || string.IsNullOrWhiteSpace(targetCurrencyCode))
            {
                throw new ArgumentException("The base and target currency codes must be provided");
            }
            if (conversionDateUtc == default(DateTime))
            {
                throw new ArgumentException("Please prove a valid conversion date.");
            }
            if (baseCurrencyCode.Equals(targetCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return new ExchangeRateResult
                {
                    Base = baseCurrencyCode,
                    Date = DateTime.UtcNow,
                    Rate = 1.0f,
                    Target = targetCurrencyCode
                };
            }

            // if the conversion date is in the future we will use today
            if (conversionDateUtc > DateTime.UtcNow)
            {
                conversionDateUtc = DateTime.UtcNow;
            }

            try
            {
                var result = MSWebRequest.RequestJson(
                    string.Format(FIXER_RATE_URL_BASE,
                        conversionDateUtc.ToString("yyyy-MM-dd"),
                            baseCurrencyCode));

                var rate = result.rates[targetCurrencyCode];

                var exchangeRate = new ExchangeRateResult
                {
                    Base = result.@base,
                    Date = DateTime.ParseExact(result.date.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    Target = targetCurrencyCode,
                    Rate = (float)result.rates[targetCurrencyCode]
                };

                return exchangeRate;
            }
            catch (Exception e)
            {
                throw new RequestExchangeRateException(
                     $"Failed to request exchange rate. From {baseCurrencyCode} to {targetCurrencyCode} on {conversionDateUtc.ToShortDateString()}.", e);
            }
        }
    }
}