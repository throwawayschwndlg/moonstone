using moonstone.core.models;
using moonstone.resources;
using moonstone.ui.web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class CurrencyController : BaseController
    {
        public CurrencyController(Current current)
            : base(current)
        {
        }

        [HttpGet]
        public ActionResult GetAllCurrencies()
        {
            try
            {
                var res = this.Current.Services.CurrencyService
                    .GetAllCurrencies()
                    .Select(c =>
                        new
                        {
                            name = $"{c.LocalizedName} ({c.Code})",
                            //description = c.Symbol,
                            value = c.Code
                        });

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult GetExchangeRate(string baseCurrency, string targetCurrency, string conversionDate)
        {
            try
            {
                var res = this.Current.Services.CurrencyService.GetExchangeRate(
                    baseCurrency, targetCurrency, DateTime.ParseExact(conversionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None));

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(
                    data: null,
                    message: string.Format(ValidationResources.Currency_GetExchangeRate_Error, targetCurrency, baseCurrency));
            }
        }

        [HttpGet]
        public ActionResult GetExchangeRateForExpense(Guid sourceBankAccoundId, string targetCurrency, string conversionDate)
        {
            try
            {
                // TODO: refector and put in server. also check if the current user can access this information
                var bankAccount = this.Current.Services.BankAccountService.GetBankAccountById(sourceBankAccoundId);
                return this.GetExchangeRate(targetCurrency, bankAccount.Currency, conversionDate);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(
                    data: null,
                    message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult GetExchangeRateForIncome(Guid destinationBankAccountId, string currency, string conversionDate)
        {
            try
            {
                // TODO: refector and put in server. also check if the current user can access this information
                var bankAccount = this.Current.Services.BankAccountService.GetBankAccountById(destinationBankAccountId);
                return this.GetExchangeRate(bankAccount.Currency, currency, conversionDate);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(
                    data: null,
                    message: ValidationResources.Generic_Error);
            }
        }
    }
}