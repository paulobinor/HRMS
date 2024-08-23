namespace hrms_be_backend_data.AppConstants
{
    public class ApplicationConstant
    {
        public const string SuccessResponseCode = "00";
        public const int successResponseCode = 00;
        public const string FailureResponse = "-1";
        public const int SuccessStatusCode = 200;
        public const int NotFoundStatusCode = 404;
        public const int NotAuthenticatedStatusCode = 401;
        public const int BadRequestStatusCode = 400;
        public const int ExceptionOccuredStatusCode = -1;
        public const string SuccessMessage = "Successful";
        public const string FailureMessage = "Failed";


        //StoredProcedures
        public const string Sp_UserAuthandLogin = "Sp_UserAuthandLogin";
        public const string Sp_Company = "Sp_Company";
        public const string Sp_Departments = "Sp_Departments";
        public const string Sp_Branch = "Sp_Branch";
        public const string Sp_HOD = "Sp_HOD";
        public const string Sp_get_countries = "Sp_get_countries";
        public const string Sp_get_states = "Sp_get_states";
        public const string Sp_get_lga = "Sp_get_lga";
        public const string Sp_Unit = "Sp_Unit";
        public const string Sp_UnitHead = "Sp_UnitHead";
        public const string Sp_JobDescription = "Sp_JobDescription";
        public const string Sp_EmployeeType = "Sp_EmployeeType";
        public const string Sp_Grade = "Sp_Grade";
        public const string Sp_Position = "Sp_Position";
        public const string Sp_Gender = "Sp_Gender";
        public const string Sp_MaritalStatus = "Sp_MaritalStatus";
        public const string Sp_Institutions = "Sp_Institutions";
        public const string Sp_Employee = "Sp_Employee";
        public const string sp_EmployeeLocation = "sp_EmployeeLocation";
        public const string sp_EmploymentStatus = "sp_EmploymentStatus";
        public const string Sp_Group = "Sp_Group";
        public const string Sp_HMO = "Sp_HMO";
        public const string Sp_HospitalProviders = "Sp_HospitalProviders";
        public const string Sp_HospitalPlan = "Sp_HospitalPlan";
        public const string Sp_Roles = "Sp_Roles";
        public const string Sp_Reviwer = "Sp_Reviwer";
        public const string Sp_ReviwersRole = "Sp_ReviwersRole";

        //Vacation module
        public const string Sp_GradeLeave = "Sp_GradeLeave";
        public const string Sp_LeaveType = "Sp_LeaveType";
        public const string Sp_LeaveRequest = "Sp_LeaveRequest";
        public const string Sp_RescheduleLeave = "Sp_RescheduleLeave";
        public const string sp_GetLeaveRequest = "Sp_GetLeaveRequest";


        public const string Sp_CreateEmpLeaveRequestLineItem = "Sp_CreateEmpLeaveRequestLineItem";
        public const string Sp_CreateEmpLeaveInfo = "Sp_CreateEmpLeaveInfo";
        public const string Sp_GetEmpLeaveInfoHistory = "Sp_GetEmpLeaveInfoHistory";
        public const string Sp_GetEmpLeaveInfo = "Sp_GetEmpLeaveInfo";
        public const string Sp_GetLeaveApproval = "Sp_GetLeaveApproval";
        public const string Sp_UpdateLeaveApprovalLineItem = "Sp_UpdateLeaveApprovalLineItem";
        public const string Sp_GetLeaveRequestLineItem = "Sp_GetLeaveRequestLineItem";
        public const string Sp_GetLeaveApprovalByRequestItem = "Sp_GetLeaveApprovalByRequestItem";
        public const string Sp_GetLeaveApprovalLineItem = "Sp_GetLeaveApprovalLineItem";
        public const string Sp_UpdateLeaveApproval = "Sp_UpdateLeaveApproval";
        public const string Sp_GetLeaveRequestLineItems  = "Sp_GetLeaveRequestLineItems";
        public const string Sp_GetEmployeeGradeLeave  = "Sp_GetEmployeeGradeLeave";
        public const string Sp_GetLeaveApprovalByEmployeeId  = "Sp_GetLeaveApprovalByEmployeeId";
        public const string Sp_UpdateLeaveRequestInfoStatus  = "Sp_UpdateLeaveRequestInfoStatus";
        public const string Sp_GetLeaveApprovalLineItems  = "Sp_GetLeaveApprovalLineItemS";
        public const string Sp_UpdateLeaveRequestLineItemApproval  = "Sp_UpdateLeaveRequestLineItemApproval";
        public const string Sp_GetLeaveType  = "sp_GetLeaveType";
        public const string Sp_GetLeaveTypeById  = "Sp_GetLeaveTypeById";
        public const string Sp_GetLeaveRequestByCompanyId  = "Sp_GetLeaveRequestByCompanyId";
        public const string Sp_GetLeaveTypeByName  = "Sp_GetLeaveTypeByName";
        public const string Sp_CreateLeaveType  = "Sp_CreateLeaveType";
        public const string Sp_GetGradeLeaveById  = "Sp_GetGradeLeaveById";
        public const string Sp_UpdateLeaveTypeById  = "Sp_UpdateLeaveTypeById";
        public const string Sp_DeleteLeaveTypeById  = "Sp_DeleteLeaveTypeById";
        public const string Sp_UpdateGradeLeave  = "Sp_UpdateGradeLeave";
        public const string Sp_DeleteGradeLeave  = "Sp_DeleteGradeLeave";
        public const string Sp_GetActiveLeaveTypes  = "Sp_GetActiveLeaveTypes";
        public const string Sp_GetAllActiveGradeLeave  = "Sp_GetAllActiveGradeLeave";
        public const string Sp_GetAllLeaveRequestLineItems  = "Sp_GetAllLeaveRequestLineItems";
        public const string Sp_GetLeaveApprovalIfoByCompanyID  = "Sp_GetLeaveApprovalIfoByCompanyID";
        public const string Sp_GetPendingLeaveApprovals  = "Sp_GetPendingLeaveApprovals";
        public const string Sp_GetEmployeeGradeLeaveTypes  = "Sp_GetEmployeeGradeLeaveTypes";
        public const string Sp_GetAllEmpLeaveRequestLineItems  = "Sp_GetAllEmpLeaveRequestLineItems";
        public const string Sp_GetEmployeeLeaveRequests  = "GetEmployeeLeaveRequests";
        public const string Sp_GetExistingLeaveApproval  = "Sp_GetExistingLeaveApproval";
        public const string Sp_GetAnnualLeaveApproval  = "Sp_GetAnnualLeaveApproval";
        public const string Sp_GetPendingAnnualLeaveApprovals  = "Sp_GetPendingAnnualLeaveApprovals";
        public const string Sp_GetAllAnnualLeaveRequestLineItems  = "Sp_GetAllAnnualLeaveRequestLineItems";
        public const string Sp_GetLeaveApprovals  = "Sp_GetLeaveApprovals";
        public const string Sp_GetAnnualLeaveApprovals  = "Sp_GetAnnualLeaveApprovals";
        public const string Sp_CreateEmpAnnualLeave  = "Sp_CreateEmpAnnualLeave";
        public const string Sp_GetEmpAnnualLeaveInfo  = "Sp_GetEmpAnnualLeaveInfo";
        public const string Sp_GetEmpAnnualLeaveRequestLineItems  = "Sp_GetEmpAnnualLeaveRequestLineItems";
        public const string Sp_GetEmpAnnualLeave  = "Sp_GetEmpAnnualLeave";
        public const string Sp_UpdateEmpAnnualLeave = "Sp_UpdateEmpAnnualLeave";
        public const string Sp_CreateApprovals = "Sp_CreateAproval";
        public const string Sp_CreateApprovalsLineItem = "Sp_CreateAprovalLineItem";
        public const string sp_get_hr_Id = "sp_get_hr_Id";

        //System Default
        public const int DefaultDeptId = 1;
        public const string DefaultPassword = "Password123";
        public const int totalFormCountSectionOne = 22;
        public const int totalFormCountSectionTwo = 12;

        //Roles
        public const int SuperAdmin = 1;
        public const int HrAdmin = 2;
        public const int GeneralUser = 3;
        public const int HrHead = 4;

        //Learning and development module
        public const string Sp_TrainingPlan = "Sp_TrainingPlan";
        public const string Sp_TrainingSchedule = "Sp_TrainingSchedule";
        public const string Sp_TrainingInduction = "Sp_TrainingInduction";
        public const string Sp_TrainingFeedbackForm = "Sp_TrainingFeedbackForm";
        public const string Sp_CheckEmpAnnualLeave = "Sp_CheckEmpAnnualLeave";
        public const string Sp_UpdateLeaveRequestApprovalID = "Sp_UpdateLeaveRequestApprovalID";
        public const string Sp_CreatLeaveApproval = "Sp_CreatLeaveApproval";
        public const string Sp_CreateLeaveApprovalLineItem = "Sp_CreateLeaveApprovalLineItem";
        public const string Sp_CreateEmpLeaveRequestLineItem1 = "Sp_CreateEmpLeaveRequestLineItem1";
        public const string Sp_GetAnnualLeaveRequestLineItems = "Sp_GetAnnualLeaveRequestLineItems";
        public const string Sp_GetAllLeaveApprovalLineItems = "Sp_GetAllLeaveApprovalLineItems";
        public const string Sp_GetAllAnnualLeaveApprovals = "Sp_GetAllAnnualLeaveApprovals";
        public const string Sp_GetAnnualLeaveInfo = "Sp_GetAnnualLeaveInfo";
        public const string Sp_GetLeaveApprovalItem = "Sp_GetLeaveApprovalItem";
        public const string Sp_GetEmpAnnualLeaveInfo1 = "Sp_GetEmpAnnualLeaveInfo1";
        public const string Sp_DeleteAnnualLeaveRequestItems = "Sp_DeleteAnnualLeaveRequestItems";
        public const string Sp_UpdateAnnualLeaveApproval = "Sp_UpdateAnnualLeaveApproval";
        public const string Sp_RescheduleLeaveRequestLineItem = "Sp_RescheduleLeaveRequestLineItem";
        public const string Sp_CreateEmpAnnualLeave1 = "Sp_CreateEmpAnnualLeave1";
        public const string Sp_RescheduleAnnualLeaveRequestLineItem = "Sp_RescheduleAnnualLeaveRequestLineItem";
        public const string Sp_CheckForExistingEmpLeaveRequest = "Sp_CheckForExistingEmpLeaveRequest";
        public const string Sp_UpdateLeaveApprovalRequestLineItemID = "Sp_UpdateLeaveApprovalRequestLineItemID";
        public const string Sp_GetAllAnnualLeaveRequests = "Sp_GetAllAnnualLeaveRequests";
        public const string Sp_GetAnnualLeaveRequestLineItemsByAnnualLeaveId = "Sp_GetAnnualLeaveRequestLineItemsByAnnualLeaveId";
        internal static string Sp_GetLeaveApprovalByApprovalKey = "Sp_GetLeaveApprovalByApprovalKey";
    }
}
