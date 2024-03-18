namespace hrms_be_backend_data.RepoPayload
{
    public class DeductionDeleteReq
    {
        public long DeductionId { get; set; }
        public string DeleteComment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
