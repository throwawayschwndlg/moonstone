using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.BankAccount
{
    public class CreateBankAccountViewModel
    {
        [Required]
        [UIHint("CurrencySelect")]
        public string Currency { get; set; }

        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal StartingBalance { get; set; }
    }
}