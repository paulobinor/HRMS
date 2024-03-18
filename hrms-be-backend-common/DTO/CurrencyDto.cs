using System.ComponentModel.DataAnnotations;

namespace hrms_be_backend_common.DTO
{
    public class CurrencyCreateDto
    {
        [Required]
        public string CurrencyName { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public string CurrencyLogo { get; set; }
        public bool IsActive { get; set; }
    }
    public class CurrencyUpdateDto
    {
        [Required]
        public long CurrencyId { get; set; }
        [Required]
        public string CurrencyName { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        public string CurrencyLogo { get; set; }
        public bool IsActive { get; set; }
    }
}
