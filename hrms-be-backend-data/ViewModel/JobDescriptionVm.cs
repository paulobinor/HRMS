namespace hrms_be_backend_data.ViewModel
{
    public class JobDescriptionWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<JobDescriptionVm> data { get; set; }
    }
    public class JobDescriptionVm
    {
        public long JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public long CompanyID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
