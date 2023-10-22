namespace hrms_be_backend_data.ViewModel
{
    public class UnitWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<UnitVm> data { get; set; }
    }
    public class UnitVm
    {
        public long UnitID { get; set; }
        public string UnitName { get; set; }
        public long CompanyID { get; set; }     
        public long UnitHeadEmployeeId { get; set; }
        public string UnitHeadEmployeeName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
