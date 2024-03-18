namespace hrms_be_backend_data.ViewModel
{
    public class EarningsVm
    {
        public long EarningsId { get; set; }
        public string EarningsName { get; set; }
        public long CompanyId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }       
    }
    public class RestatedGrossVm
    {       
        public string GrossName { get; set; }      
        public List<DeductionVm> DeductionName { get; set; }      
    }
    public class EarningsView
    {
        public long EarningsId { get; set; }
        public string EarningsName { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public List<EarningsComputationVm> EarningsComputations { get; set; }
        public string RestatedGross { get; set; }
        public string EarningsCRA { get; set; }
    }

}
