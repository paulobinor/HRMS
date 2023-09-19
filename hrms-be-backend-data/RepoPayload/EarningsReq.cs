namespace hrms_be_backend_data.RepoPayload
{
    public class EarningsReq
    {
        public long EarningId { get; set; }
        public string EarningsName { get; set; }      
        public long CreatedByUserId { get; set;}
        public DateTime DateCreated { get; set; }
        public bool IsModification { get; set; }
    } 
}
