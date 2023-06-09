using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class DeactivateUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string DeactivatedComment { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
