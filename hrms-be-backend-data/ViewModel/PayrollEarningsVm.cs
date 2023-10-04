namespace hrms_be_backend_data.ViewModel
{
    public class PayrollEarningsVm
    {
        public long EarningsId { get; set; }
        public string EarningsName { get; set; }
        public long EarningItemsId { get; set; }
        public string EarningItemName { get;set; }
        public decimal EarningItemAmount { get; set; }
        public long CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
