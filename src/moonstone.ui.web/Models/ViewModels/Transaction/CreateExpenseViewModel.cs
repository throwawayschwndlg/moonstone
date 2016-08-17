using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Models.ViewModels.Transaction
{
    public class CreateExpenseViewModel
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [UIHint("CategorySelect")]
        public Guid CategoryId { get; set; }

        [Required]
        [UIHint("CurrencySelect")]
        public string Currency { get; set; }

        [UIHint("TextArea")]
        public string Description { get; set; }

        [Required]
        public float ExchangeRate { get; set; }

        [Required]
        [UIHint("BankAccountSelect")]
        public Guid SourceBankAccountId { get; set; }

        [Required]
        [UIHint("TextBox")]
        public string Title { get; set; }

        [Required]
        [UIHint("Date")]
        public DateTime ValueDate { get; set; }
    }
}