namespace hrms_be_backend_data.ViewModel
{
    public class EarningsCRAVm
    {
        public float EarningsCRAPercentage { get; set; }
        public float EarningsCRAHigherOfPercentage { get; set; }
        public decimal EarningsCRAHigherOfValue { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
