using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class DisapproveUserDto
    {
        [Required]
        public string officialMail { get; set; }

        [Required]
        public string DisapprovedComment { get; set; }
    }
}
