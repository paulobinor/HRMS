namespace hrms_be_backend_data.ViewModel
{
    public class PayrollWithTotalVm
    {
        public long totalRecords { get; set; }
        public List<PayrollVm> data { get; set; }
    }
    public class PayrollVm
    {
        public long PayrollId { get; set; }
        public string PayRollTitle { get; set; }
        public string PayRollDescription { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int PayrollCycleId { get; set; }
        public string PayrollCycleName { get; set; }
        public DateTime Payday { get; set; }
        public bool PaydayCustomDayOfTheCycle { get; set; }
        public bool PaydayLastWeekOfTheCycle { get; set; }
        public bool PaydayLastDayOfTheCycle { get; set; }
        public bool ProrationPolicy { get; set; }
        public long CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime DateCreated { get; set; }
        public long LastUpdatedByUserId { get; set; }
        public string LastUpdateByUserName { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
    public class PayrollAllView
    {
        public long PayrollId { get; set; }
        public string PayRollTitle { get; set; }
        public string PayrollCycleName { get; set; }
        public List<string> Deductions { get; set; }
        public DateTime Payday { get; set; }
        public string CurrencyName { get; set; }
        public string CreatedByUserName { get; set; }
    }
    public class PayrollSingleView
    {
        public long PayrollId { get; set; }
        public string PayRollTitle { get; set; }
        public string PayRollDescription { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string EarningsName { get; set; }
        public List<PayrollEarnings> EarningsItems { get; set; }
        public decimal TotalEarningAmount { get; set; }
        public List<PayrollDeduction> PayrollDeductions { get; set; }
        public decimal DeductionTotalAmount { get; set; }
        public string CRAComputation { get; set; }
        public decimal CRAAmount { get; set; }
        public string RestatedGrossComputation { get; set; }
        public decimal RestatedGrossAmount { get; set; }
        public string TaxIncomeComputation { get; set; }
        public decimal TaxIncomeAmount { get; set; }
        public string TaxPayableComputation { get; set; }
        public decimal TaxPayableAmount { get; set; }
        public List<PayrollPayments> Payments { get; set; }
        public int PayrollCycleId { get; set; }
        public string PayrollCycleName { get; set; }
        public DateTime Payday { get; set; }
        public bool PaydayCustomDayOfTheCycle { get; set; }
        public bool PaydayLastWeekOfTheCycle { get; set; }
        public bool PaydayLastDayOfTheCycle { get; set; }
        public bool ProrationPolicy { get; set; }
    }

    public class PayrollEarnings
    {
        public long EarningsItemId { get; set; }
        public string EarningsItemName { get; set; }
        public decimal EarningsItemAmount { get; set; }
    }
    public class PayrollDeduction
    {
        public long DeductionId { get; set; }
        public string DeductionName { get; set; }
        public bool IsFixed { get; set; }
        public decimal DeductionFixedAmount { get; set; }
        public bool IsPercentage { get; set; }
        public decimal DeductionPercentageAmount { get; set; }
    }
    public class PayrollPayments
    {
        public string PaymentName { get; set; }
        public string PaymentSubTitle { get; set; }
        public decimal PaymentAmount { get; set; }
    }

}

