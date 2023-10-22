namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessEmployeeTypeReq
    {
        public long EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteEmployeeTypeReq
    {
        public long EmployeeTypeId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
