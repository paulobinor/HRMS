namespace hrms_be_backend_data.ViewModel
{
    public class BranchWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<BranchVm> data { get; set; }
    }
    public class BranchVm
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long CompanyID { get; set; }
        public string Address { get; set; }
        public bool IsHeadQuater { get; set; }
        public int LgaID { get; set; }
        public string LGAName { get; set;}
        public int StateID { get; set;}
        public string StateName { get; set;}
        public DateTime DateCreated { get; set;}
    }
}
