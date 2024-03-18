using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_data.RepoPayload
{
    public class LogoutDto
    {
        [Required]      
        public string OfficialMail { get; set; }
    }
}
