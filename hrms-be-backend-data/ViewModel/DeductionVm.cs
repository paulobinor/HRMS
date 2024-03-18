namespace hrms_be_backend_data.ViewModel
{
    public class DeductionVm
    {
        public long DeductionId { get; set; }
        public string DeductionName { get; set; }
        public long CompanyId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class DeductionView
    {
        public long DeductionId { get; set; }
        public string DeductionName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public List<DeductionComputationVm> DeductionComputation { get; set; }
    }
}
