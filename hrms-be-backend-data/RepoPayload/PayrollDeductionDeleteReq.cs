namespace hrms_be_backend_data.RepoPayload
{
    public class PayrollDeductionDeleteReq
    {
        public long PayrollId { get; set; }
        public long DeductionId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
