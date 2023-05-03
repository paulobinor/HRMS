using System.Collections.Generic;

namespace Com.XpressPayments.Data.AppConstants
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


        //System Default
        public static int DefaultDeptId = 1;
        public static string DefaultPassword = "Password123";


        //Roles
        public static int SuperAdmin = 1;
        public static int HrAdmin = 2;
        public static int GeneralUser = 3;
        public static int HrHead = 4;
      
    }
}
