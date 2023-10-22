namespace hrms_be_backend_common.DTO
{
    public class CreateEmployeeTypeDto
    {
        public string EmployeeTypeName { get; set; }       
    }

    public class UpdateEmployeeTypeDto
    {
        public long EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }       
    }

    public class DeleteEmployeeTypeDto
    {
        public long EmployeeTypeId { get; set; }
        public string Comment { get; set; }
    }
}
