using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private string _connectionString;
        private readonly ILogger<GroupRepository> _logger;
        private readonly IConfiguration _configuration;

        public GroupRepository(IConfiguration configuration, ILogger<GroupRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateGroup(CreateGroupDTO Creategroup, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.CREATE);
                    param.Add("@GroupName", Creategroup.GroupName.Trim());
                    param.Add("@HodID", Creategroup.HodID);
                    param.Add("@CompanyId", Creategroup.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateGroup(CreateGroupDTO group, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateGroup(UpdateGroupDTO group, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.UPDATE);
                    param.Add("@GroupIDUpd", group.GroupID);
                    param.Add("@GroupNameUpd", group.GroupName.Trim());
                    param.Add("@HodIDUpd", group.HodID);
                    param.Add("@CompanyId", group.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateGroup(UpdateGroupDTO group, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteGroup(DeleteGroupDTO group, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.DELETE);
                    param.Add("@GroupIDDelete", Convert.ToInt32(group.GroupID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Department", group.Reasons_For_Delete == null ? "" : group.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteGroup(DeleteGroupDTO group, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GroupDTO>> GetAllActiveGroup()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETALLACTIVE);

                    var GroupDetails = await _dapper.QueryAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveGroup() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GroupDTO>> GetAllGroup()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETALL);

                    var GroupDetails = await _dapper.QueryAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllGroup() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<GroupDTO> GetGroupById(long GroupID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETBYID);
                    param.Add("@GroupIDGet", GroupID);

                    var GroupDetails = await _dapper.QueryFirstOrDefaultAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetGroupById(long GroupID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<GroupDTO> GetGroupByName(string GroupName)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETBYEMAIL);
                    param.Add("@GroupNameGet", GroupName);

                    var GroupDetails = await _dapper.QueryFirstOrDefaultAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetGroupByName(string GroupName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<GroupDTO> GetGroupByCompany(string GroupName, int companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETBYEMAIL);
                    param.Add("@GroupNameGet", GroupName);
                    param.Add("@CompanyIdGet", companyId);

                    var GroupDetails = await _dapper.QueryFirstOrDefaultAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetGroupByName(string GroupName) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<GroupDTO>> GetAllGroupCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", GroupEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var GroupDetails = await _dapper.QueryAsync<GroupDTO>(ApplicationConstant.Sp_Group, param: param, commandType: CommandType.StoredProcedure);

                    return GroupDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllGroupCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }



    }
}
