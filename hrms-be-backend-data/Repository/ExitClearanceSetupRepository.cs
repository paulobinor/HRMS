using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.Repository
{
    public class ExitClearanceSetupRepository : IExitClearanceSetupRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<ExitClearanceSetupRepository> _logger;
        private readonly IConfiguration _configuration;
        public ExitClearanceSetupRepository(ILogger<ExitClearanceSetupRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateExitClearanceSetup(ExitClearanceSetupDTO request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", request.CompanyID);
                param.Add("CreatedByUserId", request.CreatedByUserId);
                param.Add("DateCreated", request.DateCreated);
                param.Add("DepartmentID", request.DepartmentID);

                return await _dapper.Get<string>("Sp_create_exit_clearance_setup", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateExitClearanceSetup(ExitClearanceSetupDTO request) => {ex.Message}");
                throw;
            }
        } 
        
        public async Task<dynamic> UpdateExitClearanceSetup(ExitClearanceSetupDTO request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ExitClearanceSetupID", request.ExitClearanceSetupID);
                param.Add("DeletedByUserId", request.DeletedByUserId);
                param.Add("IsFinalApproval", request.IsFinalApproval);

                return await _dapper.Get<string>("Sp_update_exit_clearance_setup", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateExitClearanceSetup(ExitClearanceSetupDTO request) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteExitClearanceSetup(long ExitClearanceSetupID, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ExitClearanceSetupId", ExitClearanceSetupID);
                param.Add("DeletedBy", deletedBy);
                param.Add("IsDeleted", true);
                param.Add("DateDeleted", DateTime.Now);


                return await _dapper.Get<string>("Sp_delete_exit_clearance_setup", param, commandType: CommandType.StoredProcedure);
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteExitClearanceSetup(long ExitClearanceSetupID, string deletedBy) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<ExitClearanceSetupDTO> GetExitClearanceSetupByID(long ExitClearanceSetupID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ExitClearanceSetupID", ExitClearanceSetupID);

                return await _dapper.Get<ExitClearanceSetupDTO>("Sp_get_exit_clearance_setup_by_id", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ExitClearanceSetupID}", ex);
                throw;
            }
        }
        public async Task<ExitClearanceSetupDTO> GetExitClearanceSetupByHodEmployeeID(long HodEmployeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("HodEmployeeID", HodEmployeeID);

                return await _dapper.Get<ExitClearanceSetupDTO>("Sp_get_exit_clearance_setup_by_hod_employee_id", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {HodEmployeeID}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ExitClearanceSetupDTO>> GetAllExitClearanceSetupByCompanyID(long companyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", companyID);
                //param.Add("@PageNumber", PageNumber);
                //param.Add("@RowsOfPage", RowsOfPage);
                //param.Add("@SearchVal", SearchVal.ToLower());

                var response = await _dapper.GetAll<ExitClearanceSetupDTO>("Sp_get_all_exit_clearance_setup", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by Company - {companyID}", ex);
                throw;
            }
        }

        public async Task<ExitClearanceSetupDTO> GetDepartmentThatIsFinalApprroval(long companyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", companyID);

                return await _dapper.Get<ExitClearanceSetupDTO>("Sp_get_exit_clearance_department_that_is_final_approval", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured - {companyID}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ExitClearanceSetupDTO>> GetDepartmentsThatAreNotFinalApproval(long companyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyID", companyID);


                var response = await _dapper.GetAll<ExitClearanceSetupDTO>("Sp_get_exit_clearance_departments_that_are_not_final_approval", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured - {companyID}", ex);
                throw;
            }
        }

    }
}
