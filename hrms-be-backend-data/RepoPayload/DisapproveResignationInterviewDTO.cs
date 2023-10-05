namespace hrms_be_backend_data.RepoPayload
{
    public class DisapproveResignationInterviewDTO
    {

        public long userID { get; set; }
        public bool IsDisapproved { get; set; }
        public long ID { get; set; }
        public string DisapprovedComment { get; set; }


    }
}
