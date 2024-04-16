namespace hrms_be_backend_data.RepoPayload
{
    public class DisapprovePendingResignationClearanceDTO
    {
        public long employeeID { get; set; }
        public long ResignationClearanceID { get; set; }
        public string reason { get; set; }
    }
}
