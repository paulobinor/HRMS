using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.XpressPayments.Bussiness.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]        
        public string officialMail { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
