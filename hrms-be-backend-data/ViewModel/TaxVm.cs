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
        public decimal PayableAmount { get; set; }
        public decimal PayablePercentage { get; set; }
        public bool IsLast { get; set; }
        public long CreatedByUserId { get;set; }
        public DateTime DateCreated { get; set;}
        public long LastUpdatedByUserId { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }

}
