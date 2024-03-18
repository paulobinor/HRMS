using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace hrms_be_backend_data.ViewModel
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
