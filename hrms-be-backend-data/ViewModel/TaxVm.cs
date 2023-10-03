namespace hrms_be_backend_data.ViewModel
{
    public class TaxView
    {
        public string TaxIncomeComputation { get; set; }
        public string TaxPayableComputation { get; set; }
    }

    public class TaxPayableVm
    {
        public int StepNumber { get; set; }
        public string PayableAmount { get; set; }
        public float PayablePercentage { get; set; }
        public long CreatedByUserId { get;set; }
        public DateTime DateCreated { get; set;}
        public long LastUpdatedByUserId { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }

}
