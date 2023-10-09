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
        public List<PayrollEarningsVm> EarningsItems { get; set; }
        public decimal TotalEarningAmount { get; set; }
        public int PayrollCycleId { get; set; }
        public string PayrollCycleName { get; set; }
        public DateTime Payday { get; set; }
        public bool PaydayCustomDayOfTheCycle { get; set; }
        public bool PaydayLastWeekOfTheCycle { get; set; }
        public bool PaydayLastDayOfTheCycle { get; set; }
        public bool ProrationPolicy { get; set; }
    }
}

