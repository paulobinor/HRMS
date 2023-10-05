namespace hrms_be_backend_data.RepoPayload
{
    public class DisapprovePendingResignationClearanceDTO
    {
        public long userID { get; set; }
        public long ID { get; set; }
        public string reason { get; set; }
    }
}
