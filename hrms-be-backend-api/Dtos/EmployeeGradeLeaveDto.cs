namespace hrms_be_backend_api.Dtos
{
    public class EmployeeGradeLeaveDto
    {
        public long GradeLeaveID { get; set; }
        public long LeaveTypeId { get; set; }
        public long GradeID { get; set; }
        public long GenderID { get; set; }
        public long NumbersOfDays { get; set; }
        public long NumberOfVacationSplit { get; set; }
        public int MaximumNumberOfLeaveDays { get; set; }

        public string GradeName { get; set; }
        public string LeaveTypeName { get; set; }
        public long CompanyID { get; set; }

    }
}
