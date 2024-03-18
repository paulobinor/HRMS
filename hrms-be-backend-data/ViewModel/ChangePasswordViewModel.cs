using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_data.ViewModel
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
