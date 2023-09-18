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
}
