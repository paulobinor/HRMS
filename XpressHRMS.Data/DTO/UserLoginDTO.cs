using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string CompanyID { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        //public DateTime DateCreated { get; set; }

    }

    public class UserLogoutDTO
    {
        public string Email { get; set; }
    }

    public class CreateAdminUserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string RoleName { get; set; }
        //public DateTime DateCreated { get; set; }
    }

    public class GetAllAdminUserLoginDTO
    {
        public int AdminUserID { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UpdateAdminUserLoginDTO
    {
        public int AdminUserID { get; set; }
        public int CompanyID { get; set; }
        //public string Email { get; set; }
        public string RoleName { get; set; }



    }

    public class DeleteAdminUserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
    }


    public class AdminUserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
    }

    public class CreateUserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //[JsonIgnore]
        public string RoleName { get; set; }
        //[JsonIgnore]
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        //public DateTime DateCreated { get; set; }

    }

    public class AdminDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string CompanyID { get; set; }
        public string CreatedBy { get; set; }
        //public DateTime DateCreated { get; set; }

    }
}
