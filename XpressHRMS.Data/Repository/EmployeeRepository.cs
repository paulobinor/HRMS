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

namespace XpressHRMS.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<dynamic> CreateEmployee(CreateEmployeeDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@BankAcctNo", payload.BankAcctNo);
                    param.Add("@BankID", payload.BankID);
                    param.Add("@BloodGroupID", payload.BloodGroupID);
                    param.Add("@CompanyID", payload.CompanyID);
                    param.Add("@CurrentBranchID", payload.CurrentBranchID);
                    param.Add("@CurrentDepartmentID", payload.CurrentDepartmentID);
                    param.Add("@CurrentPositionID", payload.CurrentPositionID);
                    param.Add("@DateConfirmed", payload.DateConfirmed);
                    param.Add("@DateCreated", payload.DateCreated);
                    param.Add("@DateJoined", payload.DateJoined);
                    param.Add("@DOB", payload.DOB);
                    param.Add("@EmailAddress", payload.EmailAddress);
                    param.Add("@EmployeeStatus", payload.EmployeeStatus);
                    param.Add("@EmployeeTypeID", payload.EmployeeTypeID);
                    param.Add("@FirstName", payload.FirstName);
                    param.Add("@height", payload.height);
                    param.Add("@Hobby", payload.Hobby);
                    param.Add("@HRTag", payload.HRTag);
                    param.Add("@IsFirstLogin", payload.IsFirstLogin);
                    param.Add("@JobTitleID", payload.JobTitleID);
                    param.Add("@LastName", payload.LastName);
                    param.Add("@LGAID", payload.LGAID);
                    param.Add("@MaritalID", payload.MaritalID);
                    param.Add("@MiddleName", payload.MiddleName);
                    param.Add("@NationalityID", payload.NationalityID);
                    param.Add("@NumberOfChildren", payload.NumberOfChildren);
                    param.Add("@NumberOfDepRelative", payload.NumberOfDepRelative);
                    param.Add("@PFAAcctNo", payload.PFAAcctNo);
                    param.Add("@PFAID", payload.PFAID);
                    param.Add("@PhoneNumber", payload.PhoneNumber);
                    param.Add("@Picture", payload.Picture);
                    param.Add("@PostalAddress", payload.PostalAddress);
                    param.Add("@ReligionID", payload.ReligionID);
                    param.Add("@ResidentialAddress", payload.ResidentialAddress);
                    param.Add("@StartBranchID", payload.StartBranchID);
                    param.Add("@StartDepartmentID", payload.StartDepartmentID);
                    param.Add("@StartPositionID", payload.StartPositionID);
                    param.Add("@StateID", payload.StateID);
                    dynamic response = await _dapper.ExecuteAsync("AddEmployee", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBankBranch(CreateBankDTO bankDTO) ===>{ex.Message}");
                throw;
            }
        }
    }
}
