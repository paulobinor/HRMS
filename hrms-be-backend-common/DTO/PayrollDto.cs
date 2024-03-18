namespace hrms_be_backend_common.DTO
{
    public class PayrollCreateDto
    {
        public string PayrollTitle { get; set; }
        public string PayrollDescription { get; set; }
        public int CurrencyId { get; set; }
        public int PayrollCycleId { get; set; }
        public DateTime Payday { get; set; }
        public bool PaydayLastDayOfTheCycle { get; set; }
        public bool PaydayLastWeekOfTheCycle { get; set; }
        public bool PaydayCustomDayOfTheCycle { get; set; }
        public bool ProrationPolicy { get; set; }
        public List<PayrollEarningsDto> Earnings { get; set; }
        public List<PayrollDeductionDto> Deductions { get; set; }
    }
    public class PayrollUpdateDto
    {
        public long PayrollId { get; set; }
        public string PayrollTitle { get; set; }
        public string PayrollDescription { get; set; }
        public int CurrencyId { get; set; }
        public int PayrollCycleId { get; set; }
        public DateTime Payday { get; set; }
        public bool PaydayLastDayOfTheCycle { get; set; }
        public bool PaydayLastWeekOfTheCycle { get; set; }
        public bool PaydayCustomDayOfTheCycle { get; set; }
        public bool ProrationPolicy { get; set; }
        public List<PayrollEarningsDto> Earnings { get; set; }
        public List<PayrollDeductionDto> Deductions { get; set; }
    }
    public class PayrollEarningsDto
    {       
        public long EarningsItemId { get; set; }
        public decimal EarningsItemAmount { get; set; }
    }
    public class PayrollDeductionDto
    {     
        public long DeductionId { get; set; }
        public bool IsFixed { get; set; }
        public decimal DeductionFixedAmount { get; set; }
        public bool IsPercentage { get; set; }
        public decimal DeductionPercentageAmount { get; set; }
    }
    public class RunPayrollDto
    {
        public long PayrollId { get; set; }
        public string Title { get; set; }
    }
}
