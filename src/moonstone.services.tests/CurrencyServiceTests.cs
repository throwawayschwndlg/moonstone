using FluentAssertions;
using moonstone.tests.common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services.tests
{
    public class CurrencyServiceTests
    {
        protected CurrencyService CurrencyService { get; set; }

        [SetUp]
        public void _SetUp()
        {
            this.CurrencyService = new CurrencyService(
                TestProvider.GetRepositoryHub(
                    TestProvider.GetSqlContext()));
        }

        [Test]
        public void GetExchangeRate_CanGetExchangeRateFromWeb()
        {
            var baseCurrency = "CHF";
            var targetCurrency = "USD";
            var conversionDate = DateTime.UtcNow.AddDays(-7);

            var res = this.CurrencyService.GetExchangeRate(baseCurrency, targetCurrency, conversionDate);

            res.Base.ShouldBeEquivalentTo(baseCurrency);
            res.Target.ShouldBeEquivalentTo(targetCurrency);
        }
    }
}