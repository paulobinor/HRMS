namespace hrms_be_backend_data.RepoPayload
{
    public class DeductionComputationReq
    {
        public long DeductionId { get; set; }
        public long EarningsItemId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDelete { get; set; }
    }
}
