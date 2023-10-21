namespace hrms_be_backend_common.DTO
{
    public class CreateDepartmentDto
    {       
        public string DepartmentName { get; set; }
        public bool IsHr { get; set; }
        public long HodEmployeeId { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsHr { get; set; }
        public long HodEmployeeId { get; set; }
    }

    public class DeleteDepartmentDto
    {
        public long DepartmentId { get; set; }
        public string Comment { get; set; }
    }
}
