namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessEmploymentStatusReq
    {
        public long EmploymentStatusId { get; set; }
        public string EmploymentStatusName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteEmploymentStatusReq
    {
        public long EmploymentStatusId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
