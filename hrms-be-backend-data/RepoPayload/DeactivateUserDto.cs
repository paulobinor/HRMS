using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class DeactivateUserDto
    {
        [Required]
        public string OfficialMail { get; set; }
        [Required]
        public string DeactivatedComment { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
