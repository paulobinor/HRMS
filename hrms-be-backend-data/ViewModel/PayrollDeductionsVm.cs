namespace hrms_be_backend_data.ViewModel
{
    public class PayrollDeductionsVm
    {
        public long DeductionId { get; set; }
        public string DeductionName { get; set; }
        public bool IsFixed { get; set; }
        public decimal DeductionFixedAmount { get; set; }
        public bool IsPercentage { get; set; }
        public decimal DeductionPercentageAmount { get; set; }
        public long CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
