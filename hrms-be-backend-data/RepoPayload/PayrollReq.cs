namespace hrms_be_backend_data.RepoPayload
{
    public class PayrollReq
    {
        public long PayrollId { get; set; }
        public string PayrollTitle { get; set;}
        public string PayrollDescription { get; set; }
        public int CurrencyId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModification { get; set; }
    }
}
