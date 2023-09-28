using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_common.DTO
{
    public class DeductionCreateDto
    {
        [Required]
        public string DeductionName { get; set; }
        public List<EarningsItemsDto> EarningItems { get; set; }
    }
    public class DeductionUpdateDto
    {
        [Required]
        public long DeductionId { get; set; }
        [Required]
        public string DeductionName { get; set; }
        public List<EarningsItemsDto> EarningItems { get; set; }
    }
}
