using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace hrms_be_backend_data.ViewModel
{
    public class RequestPasswordChange
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string officialMail { get; set; }
    }
}
