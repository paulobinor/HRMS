using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_common.DTO
{
    public class EarningsCreateDto
    {
        [Required]
        public string EarningsName { get; set; }
        public List<EarningsItemsDto> EarningItems { get; set; }
    }
    public class EarningsItemsDto
    {
        [Required]
        public long EarningsItemId { get; set; }       
    }
}
