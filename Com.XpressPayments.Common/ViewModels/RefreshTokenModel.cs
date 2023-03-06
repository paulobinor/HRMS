using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.XpressPayments.Bussiness.ViewModels
{
    public class RefreshTokenModel
    {
        [Required]
        public string JwtToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
