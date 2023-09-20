namespace hrms_be_backend_data.RepoPayload
{
    public class EarningsDeleteReq
    {
        public long EarningsId { get; set; }
        public string DeleteComment { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }      
    }
}
