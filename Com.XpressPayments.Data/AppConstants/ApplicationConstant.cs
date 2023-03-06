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

        //System Default
        public static int DefaultDeptId = 1;
        public static string DefaultPassword = "Password123";
    }
}
