﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class ReactivateUserDto
    {
        [Required]
        public string OfficialMail { get; set; }
        public string ReactivatedComment { get; set; }
    }
}
