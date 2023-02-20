using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDapperGeneric _dapperr;

        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger, IDapperGeneric dapperr)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
            _dapperr = dapperr;
        }
        public async Task<dynamic> CreateEmployee(CreateEmployeeDTO createEmp, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@CompanyID", CompanyID);
                    param.Add("@HRTag", createEmp.HRTag);
                    param.Add("@Title", createEmp.Title);
                    param.Add("@LastName", createEmp.LastName);
                    param.Add("@FirstName", createEmp.FirstName);
                    param.Add("@MiddleName", createEmp.MiddleName);
                    param.Add("@PostalAddress", createEmp.PostalAddress);
                    param.Add("@ResidentialAddress", createEmp.ResidentialAddress);
                    param.Add("@Sex", createEmp.Sex);
                    param.Add("@DOB", createEmp.DOB);
                    param.Add("@GenotypeID", createEmp.GenotypeID);
                    param.Add("@MaritalID", createEmp.MaritalID);
                    param.Add("@LgaID", createEmp.LgaID);
                    param.Add("@StateID", createEmp.StateID);
                    param.Add("@EmailAddress", createEmp.EmailAddress);
                    param.Add("@PhoneNumber", createEmp.PhoneNumber);
                    param.Add("@NumberOfChildren", createEmp.NumberOfChildren);
                    param.Add("@NumberOfDepRelative", createEmp.NumberOfDepRelative);
                    param.Add("@Height", createEmp.Height);
                    param.Add("@BloodGroupID", createEmp.BloodGroupID);
                    param.Add("@ReligionID", createEmp.ReligionID);
                    param.Add("@DateJoined", createEmp.DateJoined);
                    param.Add("@DateConfimed", createEmp.DateConfimed);
                    param.Add("@StartBranch", createEmp.StartBranch);
                    param.Add("@CurrentBranch", createEmp.CurrentBranch);
                    param.Add("@StartDepartment", createEmp.StartDepartment);
                    param.Add("@CurrentDepartment", createEmp.CurrentDepartment);
                    param.Add("@StartPosition", createEmp.StartPosition);
                    param.Add("@CurrentPosition", createEmp.CurrentPosition);
                    param.Add("@BankID", createEmp.BankID);
                    param.Add("@BankAcctNo", createEmp.BankAcctNo);
                    param.Add("@PFAAcctNo", createEmp.PFAAcctNo);
                    param.Add("@PFAId", createEmp.PFAId);
                    param.Add("@EmpTypeId", createEmp.EmpTypeId);
                    //param.Add("@CompanyID", createEmp.CompanyID);
                    param.Add("@EmployeeStatus", createEmp.EmployeeStatus);
                    param.Add("@IsFirstLogin", createEmp.IsFirstLogin);
                    param.Add("@DepartmentID", createEmp.DepartmentID);
                    param.Add("@PositionID", createEmp.PositionID);
                    param.Add("@BranchID", createEmp.BranchID);
                    param.Add("@JobTitleId", createEmp.JobTitleId);
                    //param.Add("@DateCreated", createEmp.DateCreated);
                    param.Add("@DrivingLicence", createEmp.DrivingLicence);

                    dynamic response = await _dapper.ExecuteAsync("sp_AddEmployee", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateEmployee(CreateEmployeeDTO createEmp, int CompanyID) ===>{ex.Message}");
                throw;
            }
        }
        //public void CreateEmployeeBulk(CreateEmployeeDTOBulk payload, int CompanyID)
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", ACTION.INSERT);
        //            param.Add("@BankAcctNo", payload.BankAcctNo);
        //            param.Add("@BankID", payload.BankID);
        //            param.Add("@BloodGroupID", payload.BloodGroupID);
        //            param.Add("@CompanyID", CompanyID);
        //            param.Add("@DateConfirmed", payload.DateConfirmed);
        //            param.Add("@DateJoined", payload.DateJoined);
        //            param.Add("@DOB", payload.DOB);
        //            param.Add("@EmailAddress", payload.EmailAddress);
        //            param.Add("@EmployeeTypeID", payload.EmployeeTypeID);
        //            param.Add("@FirstName", payload.FirstName);
        //            param.Add("@height", payload.height);
        //            param.Add("@Hobby", payload.Hobby);
        //            param.Add("@LastName", payload.LastName);
        //            param.Add("@LGAID", payload.LGAID);
        //            param.Add("@MaritalID", payload.MaritalID);
        //            param.Add("@MiddleName", payload.MiddleName);
        //            param.Add("@NationalityID", payload.NationalityID);
        //            param.Add("@NumberOfChildren", payload.NumberOfChildren);
        //            param.Add("@NumberOfDepRelative", payload.NumberOfDepRelative);
        //            param.Add("@PhoneNumber", payload.PhoneNumber);
        //            param.Add("@PostalAddress", payload.PostalAddress);
        //            param.Add("@ReligionID", payload.ReligionID);
        //            param.Add("@ResidentialAddress", payload.ResidentialAddress);
        //            param.Add("@StateID", payload.StateID);
        //            dynamic response = _dapper.ExecuteAsync("[dbo].[Sp_CreateEmployee]", param: param, commandType: CommandType.StoredProcedure);
                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: CreateBankBranch(CreateBankDTO bankDTO) ===>{ex.Message}");
        //        throw;
        //    }
        //}
    }
}
