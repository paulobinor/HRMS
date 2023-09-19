using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.DTO
{
    public class EarningsCreateDto
    {
        [Required]
        public string EarningsName { get; set; }      
    }
    public class EarningsItemsDto
    {
        [Required]
        public string EarningsItemName { get; set; }       
    }
}
