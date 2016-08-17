using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.User
{
    public class SettingsViewModel
    {
        public bool AutoUpdateTimeZone { get; set; }

        [Required]
        [UIHint("DateFormatSelect")]
        public string DateFormat { get; set; }

        [UIHint("TextBox")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [UIHint("TimeZoneSelect")]
        public string TimeZone { get; set; }
    }
}