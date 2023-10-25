using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_common.DTO
{
    public class ChangePasswordDto
    {       
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class ChangeDefaultPasswordDto
    {       
        [Required]
        public string token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
