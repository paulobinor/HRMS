namespace hrms_be_backend_common.DTO
{
    public class PayrollCreateDto
    {
        public string PayrollTitle { get; set; }
        public string PayrollDescription { get; set; }
        public int CurrencyId { get; set; }
    }
    public class PayrollUpdateDto
    {
        public long PayrollId { get; set; }
        public string PayrollTitle { get; set; }
        public string PayrollDescription { get; set; }
        public int CurrencyId { get; set; }
    }
}
