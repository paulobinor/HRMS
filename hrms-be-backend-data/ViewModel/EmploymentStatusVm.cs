namespace hrms_be_backend_data.ViewModel
{
    public class EmploymentStatusWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<EmploymentStatusVm> data { get; set; }
    }
    public class EmploymentStatusVm
    {
        public long EmploymentStatusID { get; set; }
        public string EmploymentStatusName { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
