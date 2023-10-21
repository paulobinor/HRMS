namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessDepartmentReq
    {
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }      
        public bool IsHr { get; set; }
        public long HodEmployeeId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteDepartmentReq
    {
        public long DepartmentId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
