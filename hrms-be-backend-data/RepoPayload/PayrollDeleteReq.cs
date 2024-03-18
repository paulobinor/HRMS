namespace hrms_be_backend_data.RepoPayload
{
    public class PayrollDeleteReq
    {
        public long PayrollId { get; set; }
        public string DeleteComment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
