namespace hrms_be_backend_common.DTO
{
    public class EmployeeLeaveTypeCumulative
    {
        public string LeaveTypeId { get; set; }
        public long LeaveRequestLineItemID { get; set; }
        public int NoOfDaysAllocated { get; set; }
        public int NoOfDaysTaken { get; set; }
        public int NoOfDaysRequested { get; set; }

        public int NoOfDaysRemaining
        {
            get
            {
                return (NoOfDaysRequested - NoOfDaysRemaining);
            }
        }
        public bool IsValid
        {
            get 
            {
                if (NoOfDaysRemaining > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
