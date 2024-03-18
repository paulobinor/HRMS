namespace hrms_be_backend_data.RepoPayload
{
    public class PayrollEarningsDeleteReq
    {
        public long PayrollId { get; set; }
        public long EarningsItemId { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
