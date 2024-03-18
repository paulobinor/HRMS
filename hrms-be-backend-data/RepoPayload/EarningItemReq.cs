namespace hrms_be_backend_data.RepoPayload
{
    public class EarningItemReq
    {
        public long EarningItemId { get; set; }
        public string EarningsItemName { get; set; }
        public long CompanyId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModification { get; set; }
    }
}
