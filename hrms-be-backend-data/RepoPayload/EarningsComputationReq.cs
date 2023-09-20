namespace hrms_be_backend_data.RepoPayload
{
    public class EarningsComputationReq
    {
        public long EarningsId { get; set; }
        public long EarningsItemId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDelete { get; set; }
    }
}
