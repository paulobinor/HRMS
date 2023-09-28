namespace hrms_be_backend_data.ViewModel
{
    public class DeductionComputationVm
    {
        public long EarningsItemId { get; set; }
        public string EarningItemName { get; set; }
        public string DeductionName { get; set; }
        public long DeductionId { get; set; }      
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
