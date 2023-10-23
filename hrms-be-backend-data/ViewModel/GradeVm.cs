namespace hrms_be_backend_data.ViewModel
{
    public class GradeWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<GradeVm> data { get; set; }
    }
    public class GradeVm
    {
        public long GradeID { get; set; }
        public string GradeName { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
