namespace hrms_be_backend_data.ViewModel
{
    public class EmployeeTypeWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<EmployeeTypeVm> data { get; set; }
    }
    public class EmployeeTypeVm
    {
        public long EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
