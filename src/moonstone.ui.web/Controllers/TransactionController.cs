using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.Transaction;
using System;
using System.Linq;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class TransactionController : BaseController
    {
        public TransactionController(Current current)
            : base(current)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateTransactionViewModel
            {
                ValueDate = DateTime.UtcNow.ToLocalTime()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTransactionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.Current.Services.TransactionService.CreateTransaction(
                        new core.models.Transaction()
                        {
                            Amount = model.Amount,
                            CategoryId = model.CategoryId,
                            CreateUserId = this.Current.User.Id,
                            CreationDateUtc = DateTime.UtcNow,
                            Currency = model.Currency,
                            Description = model.Description,
                            DestinationBankAccountId = model.DestinationBankAccountId,
                            GroupId = this.Current.User.CurrentGroupId.Value,
                            SourceBankAccountId = model.SourceBankAccountId,
                            Title = model.Title,
                            ValueDateUtc = model.ValueDate.ToUniversalTime()
                        });

                    return this.JsonSuccess(
                        data: null,
                        message: string.Format(ValidationResources.Transaction_Create_Success, model.Title),
                        returnUrl: Routes.IndexTransactions.GetActionLink(Url));
                }

                return this.JsonError(data: null, message: ValidationResources.Generic_ModelState_Error);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Expense()
        {
            try
            {
                var defaultAccount = this.Current.Services.BankAccountService.GetDefaultAccountForuser(this.Current.User.Id);
                var defaultCategory = this.Current.Services.CategoryService.GetDefaultCategoryForUser(this.Current.User.Id);

                var model = new CreateExpenseViewModel
                {
                    SourceBankAccountId = defaultAccount.Id,
                    Currency = defaultAccount.Currency,
                    CategoryId = defaultCategory.Id,
                    ExchangeRate = 1.0f,
                    ValueDate = DateTime.UtcNow
                };

                return View(model);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                throw e;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Expense(CreateExpenseViewModel model)
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Income()
        {
            var model = new CreateIncomeViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Income(CreateIncomeViewModel model)
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}