using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class ResignationRepository : IResignationRepository
    {

        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<ResignationRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationRepository(ILogger<ResignationRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateResignation(ResignationDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("EmployeeID", resignation.EmployeeId);
                //param.Add("StaffID", resignation.StaffID);
               // param.Add("StaffName", resignation.StaffName);
                param.Add("SignedResignationLetter", resignation.SignedResignationLetter);
                param.Add("CompanyID", resignation.CompanyID);
                param.Add("ResumptionDate", resignation.ResumptionDate);
                param.Add("ExitDate", resignation.ExitDate);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);
                param.Add("CreatedByUserId", resignation.CreatedByUserId);
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("DateCreated", resignation.DateCreated);

                // Add an output parameter to capture the ResignationID
                param.Add("@ResignationIDOut", dbType: DbType.Int32, direction: ParameterDirection.Output);

                dynamic response = await _dapper.Get<string>("Sp_SubmitResignation", param, commandType: CommandType.StoredProcedure);

                // Retrieve the ResignationID from the output parameter
                int resignationID = param.Get<int>("@ResignationIDOut");

                return resignationID;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateResignation(ResignationDTO resignation) => {ex.Message}");
                throw;
            }
        }
        public async Task<dynamic> UpdateResignation(UpdateResignationDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ResignationId", resignation.ResignationID);
                //param.Add("EmployeeId", resignation.EmployeeId);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("SignedResignationLetter", resignation.SignedResignationLetter);

                dynamic response =  await _dapper.Get<string>("Sp_update_resignation", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateResignation(ResignationDTO resignation) ===>{ex.Message}");
                throw;
            }
        }


        public async Task<ResignationDTO> GetResignationByID(long ID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ResignationId", ID);

                var resignationDetails = await _dapper.Get<ResignationDTO>("Sp_get_resignation_by_id", param, commandType: CommandType.StoredProcedure);

                return resignationDetails;
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ID}", ex);
                throw;
            }
        }

        public async Task<ResignationDTO> GetResignationByEmployeeID(long employeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("employeeID", employeeID);

                var response = await _dapper.Get<ResignationDTO>("Sp_get_resignation_by_user", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by UserID - {employeeID}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ResignationDTO>> GetResignationByCompanyID(long companyID, int PageNumber, int RowsOfPage, string SearchVal)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyId", companyID);
                param.Add("PageNumber", PageNumber);
                param.Add("RowsOfPage", RowsOfPage);
                param.Add("SearchVal", SearchVal.ToLower());

                var response = await _dapper.GetAll<ResignationDTO>("Sp_get_resignation_by_company", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by Company - {companyID}", ex);
                throw;
            }
        }

        //public async Task<IEnumerable<ResignationDTO>> GetAllResignations()
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        var response = await _dapper.GetAll<ResignationDTO>("Sp_get_all_resignations", param, commandType: CommandType.StoredProcedure);

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error Getting Resignations", ex);
        //        throw;
        //    }
        //}


        //public async Task<dynamic> DeleteResignation(long ID, string deletedBy, string deleteReason)
        //{
        //    try
        //    {
        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
                    
        //            var param = new DynamicParameters();
        //            param.Add("ResignationId", ID);
        //            param.Add("DeletedBy", deletedBy);
        //            param.Add("IsDeleted", true);
        //            param.Add("deleteReason", deleteReason);
        //            param.Add("DateDeleted", DateTime.Now);


        //            int response = await _dapper.ExecuteAsync("Sp_delete_resignation", param: param, commandType: CommandType.Text);

        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: CreateBranch(CreateBranchDTO branch, string createdbyUserEmail) ===>{ex.Message}");
        //        throw;
        //    }
        //}


        public async Task<IEnumerable<ResignationDTO>> GetPendingResignationByEmployeeID(long employeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("employeeID", employeeID);

                var response = await _dapper.GetAll<ResignationDTO>("Sp_GetPendingResignationByUserID", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingResignationByUserID(long userID) => {ex.Message}");
                throw;
            }
        }

        public async Task<string> ApprovePendingResignation(long employeeID, long ResignationID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("EmployeeID", employeeID);
                param.Add("ResignationID", ResignationID);
                param.Add("DateApproved", DateTime.Now);
               // param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var response = await _dapper.Get<string>("Sp_ApprovePendingResignation", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApprovePendingResignationAsync(long employeeID, long ResignationID) => {ex.Message}");
                throw;
            }
        }

        public async Task<string> DisapprovePendingResignation(long employeeID, long ResignationID, string reason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", employeeID);
                param.Add("ResignationID", ResignationID);
                param.Add("DateDisapproved", DateTime.Now);
                param.Add("DisapprovedReason", reason);

                var response = await _dapper.Get<string>("Sp_DisapprovePendingResignation", param, commandType: CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapprovePendingResignationAsync(long employeeID, long ResignationID, string reason) => {ex.Message}");
                throw;
            }
        }

    }
}
