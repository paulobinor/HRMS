namespace hrms_be_backend_data.ViewModel
{
    public class EarningsItemVm
    {
        public long EarningItemId { get; set; }       
        public string EarningItemName { get; set; }
        public long CompanyId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
