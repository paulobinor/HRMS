using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.XpressPayments.Bussiness.ViewModels
{
    public class LoginModel
    {
        [Required]
        //public string Email { get; set; }
        public string OfficialMail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
