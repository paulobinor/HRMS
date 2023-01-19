using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.Response
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AllApp
    {
        public int appId { get; set; }
        public string appName { get; set; }
        public string appUrl { get; set; }
        public string appImage { get; set; }
        public string appKey { get; set; }
        public bool isDefaultApp { get; set; }
        public int createdByUserId { get; set; }
        public DateTime dateCreated { get; set; }
        public object lastModifiedByUserId { get; set; }
        public object dateModified { get; set; }
        public bool isApproved { get; set; }
        public object dateApproved { get; set; }
        public object approvedByUserId { get; set; }
        public bool isDisApproved { get; set; }
        public object disApprovedByUserId { get; set; }
        public object disApprovedComment { get; set; }
    }

    public class Data
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int roleId { get; set; }
        public bool isSelfOnboarding { get; set; }
        public bool isIntegrator { get; set; }
        public bool isFirstLogin { get; set; }
        public object approvalStatus { get; set; }
        public bool isActive { get; set; }
        public bool isLockedOut { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime lastLoginDate { get; set; }
        public UserRole userRole { get; set; }
        //public List<UserApp> userApps { get; set; }
       // public List<AllApp> allApps { get; set; }
        public string phoneNumber { get; set; }
    }

    public class SSOResponse
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        //public string jwtToken { get; set; }
        //public string refreshToken { get; set; }
        public Data data { get; set; }
    }

    //public class UserApp
    //{
    //    public int appId { get; set; }
    //    public string appName { get; set; }
    //    public string appUrl { get; set; }
    //    public string appImage { get; set; }
    //    public string appKey { get; set; }
    //    public bool isDefaultApp { get; set; }
    //    public int createdByUserId { get; set; }
    //    public DateTime dateCreated { get; set; }
    //    public object lastModifiedByUserId { get; set; }
    //    public object dateModified { get; set; }
    //    public bool isApproved { get; set; }
    //    public object dateApproved { get; set; }
    //    public object approvedByUserId { get; set; }
    //    public bool isDisApproved { get; set; }
    //    public object disApprovedByUserId { get; set; }
    //    public object disApprovedComment { get; set; }
    //}

    public class UserRole
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
    }


}
