namespace hrms_be_backend_data.RepoPayload
{
    public class PayrollDeductionReq
    {
        public long PayrollId { get; set; }
        public long DeductionId { get; set; }
        public bool IsFixed { get; set; }
        public decimal DeductionFixedAmount { get; set; }
        public bool IsPercentage { get; set; }
        public decimal DeductionPercentageAmount { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
