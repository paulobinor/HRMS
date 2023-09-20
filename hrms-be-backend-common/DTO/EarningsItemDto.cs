namespace hrms_be_backend_common.DTO
{
    public class EarningsItemCreateDto
    {
        public string EarningsItemName { get; set; }
    }
    public class EarningsItemUpdateDto
    {
        public long EarningsItemId { get; set; }
        public string EarningsItemName { get; set; }
    }
}
