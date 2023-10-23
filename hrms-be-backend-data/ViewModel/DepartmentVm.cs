namespace hrms_be_backend_data.ViewModel
{

    public class DepartmentWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<DepartmentVm> data { get; set; }
    }
    public class DepartmentVm
    {
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public long CompanyID { get; set; }       
        public bool IsHR { get; set; }
        public long HodEmployeeId { get; set; }
        public string HodEmployeeName { get; set; }       
        public DateTime DateCreated { get; set; }
    }
}
