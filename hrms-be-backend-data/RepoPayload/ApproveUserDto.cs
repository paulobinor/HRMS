using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_data.RepoPayload
{
    public class ApproveUserDto
    {
        [Required]
        public string officialMail { get; set; }
    }
}
