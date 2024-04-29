namespace hrms_be_backend_data.AppConstants
{
    public class ApplicationConstant
    {
        public static string SuccessResponseCode = "00";
        public static int successResponseCode = 00;
        public static string FailureResponse = "-1";
        public static int SuccessStatusCode = 200;
        public static int NotFoundStatusCode = 404;
        public static int NotAuthenticatedStatusCode = 401;
        public static int BadRequestStatusCode = 400;
        public static int ExceptionOccuredStatusCode = -1;
        public static string SuccessMessage = "Successful";
        public static string FailureMessage = "Failed";


        //StoredProcedures
        public static string Sp_UserAuthandLogin = "Sp_UserAuthandLogin";
        public static string Sp_Company = "Sp_Company";
        public static string Sp_Departments = "Sp_Departments";
        public static string Sp_Branch = "Sp_Branch";
        public static string Sp_HOD = "Sp_HOD";
        public static string Sp_get_countries = "Sp_get_countries";
        public static string Sp_get_states = "Sp_get_states";
        public static string Sp_get_lga = "Sp_get_lga";
        public static string Sp_Unit = "Sp_Unit";
        public static string Sp_UnitHead = "Sp_UnitHead";
        public static string Sp_JobDescription = "Sp_JobDescription";
        public static string Sp_EmployeeType = "Sp_EmployeeType";
        public static string Sp_Grade = "Sp_Grade";
        public static string Sp_Position = "Sp_Position";
        public static string Sp_Gender = "Sp_Gender";
        public static string Sp_MaritalStatus = "Sp_MaritalStatus";
        public static string Sp_Institutions = "Sp_Institutions";
        public static string Sp_Employee = "Sp_Employee";
        public static string sp_EmployeeLocation = "sp_EmployeeLocation";
        public static string sp_EmploymentStatus = "sp_EmploymentStatus";
        public static string Sp_Group = "Sp_Group";
        public static string Sp_HMO = "Sp_HMO";
        public static string Sp_HospitalProviders = "Sp_HospitalProviders";
        public static string Sp_HospitalPlan = "Sp_HospitalPlan";
        public static string Sp_Roles = "Sp_Roles";
        public static string Sp_Reviwer = "Sp_Reviwer";
        public static string Sp_ReviwersRole = "Sp_ReviwersRole";

        //Vacation module
        public static string Sp_GradeLeave = "Sp_GradeLeave";
        public static string Sp_LeaveType = "Sp_LeaveType";
        public static string Sp_LeaveRequest = "Sp_LeaveRequest";
        public static string Sp_RescheduleLeave = "Sp_RescheduleLeave";
        public static string sp_GetLeaveRequest = "Sp_GetLeaveRequest";


        public static string Sp_CreateEmpLeaveRequestLineItem = "Sp_CreateEmpLeaveRequestLineItem";
        public static string Sp_CreateEmpLeaveInfo = "Sp_CreateEmpLeaveInfo";
        public static string Sp_RescheduleLeaveRequestLineItem = "Sp_RescheduleLeaveRequestLineItem";
        public static string Sp_GetEmpLeaveInfoHistory = "Sp_GetEmpLeaveInfoHistory";
        public static string Sp_GetEmpLeaveInfo = "Sp_GetEmpLeaveInfo";
        public static string Sp_GetLeaveApproval = "Sp_GetLeaveApproval";
        public static string Sp_UpdateLeaveApprovalLineItem = "Sp_UpdateLeaveApprovalLineItem";
        public static string Sp_GetLeaveRequestLineItem = "Sp_GetLeaveRequestLineItem";
        public static string Sp_GetLeaveApprovalByRequestItem = "Sp_GetLeaveApprovalByRequestItem";
        public static string Sp_GetLeaveApprovalLineItem = "Sp_GetLeaveApprovalLineItem";
        public static string Sp_UpdateLeaveApproval = "Sp_UpdateLeaveApproval";
        public static string Sp_GetLeaveRequestLineItems { get; set; } = "Sp_GetLeaveRequestLineItems";
        public static string Sp_GetEmployeeGradeLeave { get; set; } = "Sp_GetEmployeeGradeLeave";
        public static string Sp_GetLeaveApprovalByEmployeeId { get; set; } = "Sp_GetLeaveApprovalByEmployeeId";
        public static string Sp_UpdateLeaveRequestInfoStatus { get; set; } = "Sp_UpdateLeaveRequestInfoStatus";
        public static string Sp_GetLeaveApprovalLineItems { get; set; } = "Sp_GetLeaveApprovalLineItemS";
        public static string Sp_UpdateLeaveRequestLineItemApproval { get; set; } = "Sp_UpdateLeaveRequestLineItemApproval";
        public static string Sp_GetLeaveType { get; set; } = "sp_GetLeaveType";
        public static string Sp_GetLeaveTypeById { get; set; } = "Sp_GetLeaveTypeById";
        public static string Sp_GetLeaveRequestByCompanyId { get; set; } = "Sp_GetLeaveRequestByCompanyId";
        public static string Sp_GetLeaveTypeByName { get; set; } = "Sp_GetLeaveTypeByName";
        public static string Sp_CreateLeaveType { get;  set; } = "Sp_CreateLeaveType";
        public static string Sp_GetGradeLeaveById { get; set; } = "Sp_GetGradeLeaveById";
        public static string Sp_UpdateLeaveTypeById { get; set; } = "Sp_UpdateLeaveTypeById";
        public static string Sp_DeleteLeaveTypeById { get; set; } = "Sp_DeleteLeaveTypeById";
        public static string Sp_UpdateGradeLeave { get; set; } = "Sp_UpdateGradeLeave";
        public static string Sp_DeleteGradeLeave { get; set; } = "Sp_DeleteGradeLeave";
        public static string Sp_GetActiveLeaveTypes { get; set; } = "Sp_GetActiveLeaveTypes";
        public static string Sp_GetAllActiveGradeLeave { get; set; } = "Sp_GetAllActiveGradeLeave";
        public static string Sp_GetAllLeaveRequestLineItems { get; set; } = "Sp_GetAllLeaveRequestLineItems";
        public static string Sp_GetLeaveApprovalIfoByCompanyID { get; set; } = "Sp_GetLeaveApprovalIfoByCompanyID";
        public static string Sp_GetPendingLeaveApprovals { get; set; } = "Sp_GetPendingLeaveApprovals";
        public static string Sp_GetEmployeeGradeLeaveTypes { get; set; } = "Sp_GetEmployeeGradeLeaveTypes";
        public static string Sp_GetAllEmpLeaveRequestLineItems { get; set; } = "Sp_GetAllEmpLeaveRequestLineItems";

        //System Default
        public static int DefaultDeptId = 1;
        public static string DefaultPassword = "Password123";
        public static int totalFormCountSectionOne = 22;
        public static int totalFormCountSectionTwo = 12;

        //Roles
        public static int SuperAdmin = 1;
        public static int HrAdmin = 2;
        public static int GeneralUser = 3;
        public static int HrHead = 4;

        //Learning and development module
        public static string Sp_TrainingPlan = "Sp_TrainingPlan";
        public static string Sp_TrainingSchedule = "Sp_TrainingSchedule";
        public static string Sp_TrainingInduction = "Sp_TrainingInduction";
        public static string Sp_TrainingFeedbackForm = "Sp_TrainingFeedbackForm";

    }
}
