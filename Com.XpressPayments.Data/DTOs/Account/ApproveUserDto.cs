using System.ComponentModel.DataAnnotations;

namespace Com.XpressPayments.Data.DTOs.Account
{
    public class ApproveUserDto
    {
        [Required]
        public string Email { get; set; }
    }
}
