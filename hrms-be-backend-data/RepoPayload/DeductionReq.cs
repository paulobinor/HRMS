namespace hrms_be_backend_data.RepoPayload
{
    public class DeductionReq
    {
        public long DeductionId { get; set; }
        public string DeductionName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModification { get; set; }
    }
}
