using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class LogoutDto
    {
        [Required]
        //public string Email { get; set; }
        public string OfficialMail { get; set; }
    }
}
