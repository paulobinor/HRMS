namespace hrms_be_backend_common.DTO
{
    public class CreateEmploymentStatusDto
    {
        public string EmploymentStatusName { get; set; }      
    }

    public class UpdateEmploymentStatusDto
    {
        public long EmploymentStatusId { get; set; }
        public string EmploymentStatusName { get; set; }       
    }

    public class DeleteEmploymentStatusDto
    {
        public long EmploymentStatusId { get; set; }
        public string Comment { get; set; }
    }
}
