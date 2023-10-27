using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class UserAppModulePrivilegesRepository : IUserAppModulePrivilegeRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<UserAppModulePrivilegesRepository> _logger;
        private readonly IConfiguration _configuration;
        public UserAppModulePrivilegesRepository(ILogger<UserAppModulePrivilegesRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> CheckUserAppPrivilege(string PrivilegeCode, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@PrivilegeCode", PrivilegeCode);
                param.Add("@CreatedByUserId", CreatedByUserId);

                return await _repository.Get<string>("sp_check_user_app_privilege", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"UserAppModulePrivilegesRepository -> CheckUserAppPrivilege => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }

        public async Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivileges(long companyID)
        {
            try
            {
                string query = @"Select e.StaffID, u.FirstName , u.LastName, u.OfficialMail as 'Email', am.AppModuleName ,amp.AppModulePrivilegeName PrivilegeName, amp.AppModulePrivilegeCode PrivilegeCode, am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId 
                  Join USers u on u.UserID = uamp.USerID join Employee e on e.EmployeeID = u.EmployeeId where uamp.IsDeleted = @IsDeleted and uamp.IsApproved = @IsApproved and u.CompanyId = @CompanyID";
                var param = new DynamicParameters();
                param.Add("IsApproved", true);
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetUserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUserAppModulePrivileges => {ex.ToString()}");
                throw;
            }
        }

        public async Task<GetUserAppModulePrivilegesDTO> GetUserAppModuleByUserandPrivilegeID(long userID, int privilegeID)
        {
            try
            {
                string query = @"Select e.StaffID, u.FirstName , u.LastName, u.OfficialMail as 'Email', am.AppModuleName ,amp.AppModulePrivilegeName PrivilegeName, amp.AppModulePrivilegeCode PrivilegeCode, am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId 
                    Join USers u on u.UserID = uamp.USerID join Employee e on e.EmployeeID = e.EmployeeID where uamp.IsDeleted = @IsDeleted and uamp.IsDisapproved = @IsDisapproved and uamp.UserID = @UserID and uamp.AppModulePrivilegeID = @AppModulePrivilegeID";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("AppModulePrivilegeID", privilegeID);
                param.Add("UserID", userID);

                var resp = await _repository.Get<GetUserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUserAppModuleByUserandPrivilegeID => {ex.ToString()}");
                throw;
            }
        }

        public async Task<UserAppModulePrivilegesDTO> GetUserAppModulePrivilegeByID(long userAppModulePrivilegeID)
        {
            try
            {
                string query = @"Select am.AppModuleName , am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId  where uamp.IsDeleted = @IsDeleted and uamp.IsDisapproved = @IsDisapproved and uamp.UserAppModulePrivilegeID = @UserAppModulePrivilegeID";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("UserAppModulePrivilegeID", userAppModulePrivilegeID);

                var resp = await _repository.Get<UserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUserAppModulePrivilegeByID => {ex.ToString()}");
                throw;
            }
        }


        public async Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByUserID(long userID)
        {
            try
            {
                string query = @"Select e.StaffID, u.FirstName , u.LastName, u.OfficialMail as 'Email', am.AppModuleName ,amp.AppModulePrivilegeName PrivilegeName, amp.AppModulePrivilegeCode PrivilegeCode, am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId 
                  Join USers u on u.UserID = uamp.USerID join Employee e on e.EmployeeID = u.EmployeeId 
                  where uamp.IsDeleted = @IsDeleted and uamp.IsApproved = @IsApproved and uamp.UserID = @UserID";
                var param = new DynamicParameters();
                param.Add("UserID", userID);
                param.Add("IsDeleted", false);
                param.Add("IsApproved", true);

                var resp = await _repository.GetAll<GetUserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUserAppModulePrivilegesByUserID => {ex.ToString()}");
                throw;
            }
        }


        public async Task<List<AppModulePrivilegeDTO>> GetAppModulePrivileges()
        {
            try
            {
                string query = @"select * from AppModulePrivilege";

                var resp = await _repository.GetAll<AppModulePrivilegeDTO>(query, null, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAppModulePrivileges => {ex.ToString()}");
                throw;
            }
        }

        public async Task<List<AppModulePrivilegeDTO>> GetAppModulePrivilegeByAppModuleID(long moduleID)
        {
            try
            {
                string query = @"select * from AppModulePrivilege where AppModuleID = @AppModuleID";
                var param = new DynamicParameters();
                param.Add("AppModuleID", moduleID);

                var resp = await _repository.GetAll<AppModulePrivilegeDTO>(query, null, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAppModulePrivilegeByID => {ex.ToString()}");
                throw;
            }
        }

        public async Task<List<GetUserAppModulePrivilegesDTO>> GetUserAppModulePrivilegesByPrivilegeID(long privilegeID)
        {
            try
            {
                string query = @"Select e.StaffID, u.FirstName , u.LastName, u.OfficialMail as 'Email', am.AppModuleName ,amp.AppModulePrivilegeName PrivilegeName, amp.AppModulePrivilegeCode PrivilegeCode, am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId 
                  Join USers u on u.UserID = uamp.USerID join Employee e on e.EmployeeID = u.EmployeeId 
                  where uamp.IsDeleted = @IsDeleted and uamp.IsApproved = @IsApproved and uamp.AppModulePrivilegeID = @AppModulePrivilegeID";
                var param = new DynamicParameters();
                param.Add("AppModulePrivilegeID", privilegeID);
                param.Add("IsDeleted", false);
                param.Add("IsApproved", true);

                var resp = await _repository.GetAll<GetUserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUserAppModulePrivilegesByPrivilegeID => {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<GetUserAppModulePrivilegesDTO>> GetPendingUserAppModulePrivileges(long companyID)
        {
            try
            {
                string query = @"Select e.StaffID, u.FirstName , u.LastName, u.OfficialMail as 'Email', am.AppModuleName ,amp.AppModulePrivilegeName PrivilegeName,amp.AppModulePrivilegeCode PrivilegeCode, am.AppModuleCode , uamp.* from UserAppModulePrivileges uamp join AppModulePrivilege amp on uamp.AppModulePrivilegeID = amp.AppModulePrivilegeID join AppModules am on amp.AppModuleID = am.AppModuleId 
                  Join USers u on u.UserID = uamp.USerID join Employee e on e.EmployeeID = u.EmployeeId where uamp.IsDeleted = @IsDeleted and uamp.IsApproved = @IsApproved and uamp.IsDisapproved = @IsDisapproved and u.CompanyId = @CompanyID";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsApproved", false);
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetUserAppModulePrivilegesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingUserAppModulePrivileges => {ex.ToString()}");
                throw;
            }
        }


        public async Task<int> CreateUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO)
        {
            try
            {
                string query = @"Insert into UserAppModulePrivileges (AppModulePrivilegeID , UserID , DateCreated , IsActive , CreatedByUserId) values (@AppModulePrivilegeID , @UserID , @DateCreated , @IsActive , @CreatedByUserId) SELECT SCOPE_IDENTITY()";
                var param = new DynamicParameters();
                param.Add("AppModulePrivilegeID", userAppModulePrivilegesDTO.AppModulePrivilegeID);
                param.Add("UserID", userAppModulePrivilegesDTO.UserID);
                param.Add("DateCreated", userAppModulePrivilegesDTO.DateCreated);
                param.Add("IsActive", userAppModulePrivilegesDTO.IsActive);
                param.Add("CreatedByUserId", userAppModulePrivilegesDTO.CreatedByUserId);

                var resp = await _repository.Insert<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateUserAppModulePrivileges => {ex.ToString()}");
                throw;
            }
        }


        public async Task<int> UpdateUserAppModulePRivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO)
        {
            try
            {
                string query = @" Update UserAppModulePrivileges set IsActive = @IsActive , IsDeleted = @IsDeleted , DeletedByUserId = @DeletedByUserId where UserAppModulePrivilegeID = @UserAppModulePrivilegeID";
                var param = new DynamicParameters();

                param.Add("IsActive", userAppModulePrivilegesDTO.IsActive);
                param.Add("IsDeleted", userAppModulePrivilegesDTO.IsDeleted);
                param.Add("DeletedByUserId", userAppModulePrivilegesDTO.DeletedByUserId);
                param.Add("UserAppModulePrivilegeID", userAppModulePrivilegesDTO.UserAppModulePrivilegeID);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateUserAppModulePRivileges => {ex.ToString()}");
                throw;
            }
        }


        public async Task<int> ApproveUserAppModulePrivileges(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO)
        {
            try
            {
                string query = @"Update UserAppModulePrivileges set IsActive = @IsActive , IsApproved = @IsApproved , ApprovedByUserId = @ApprovedByUserId , DateApproved = @DateApproved where UserAppModulePrivilegeID = @UserAppModulePrivilegeID";
                var param = new DynamicParameters();

                param.Add("IsActive", userAppModulePrivilegesDTO.IsActive);
                param.Add("IsApproved", userAppModulePrivilegesDTO.IsApproved);
                param.Add("ApprovedByUserId", userAppModulePrivilegesDTO.ApprovedByUserId);
                param.Add("DateApproved", userAppModulePrivilegesDTO.DateApproved);
                param.Add("UserAppModulePrivilegeID", userAppModulePrivilegesDTO.UserAppModulePrivilegeID);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveUserAppModulePrivileges => {ex.ToString()}");
                throw;
            }
        }

        public async Task<int> DisapproveUserAppModulePrivilege(UserAppModulePrivilegesDTO userAppModulePrivilegesDTO)
        {
            try
            {
                string query = @"Update UserAppModulePrivileges set IsActive = @IsActive , IsDisapproved = @IsDisapproved , DisapprovedByUserId = @DisapprovedByUserId , DateApproved = @DateApproved where UserAppModulePrivilegeID = @UserAppModulePrivilegeID";
                var param = new DynamicParameters();

                param.Add("IsActive", userAppModulePrivilegesDTO.IsActive);
                param.Add("IsDisapproved", userAppModulePrivilegesDTO.IsDisapproved);
                param.Add("DisapprovedByUserId", userAppModulePrivilegesDTO.DisapprovedByUserId);
                param.Add("DateApproved", userAppModulePrivilegesDTO.DateApproved);
                param.Add("UserAppModulePrivilegeID", userAppModulePrivilegesDTO.UserAppModulePrivilegeID);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapproveUserAppModulePrivilege => {ex.ToString()}");
                throw;
            }
        }
    }
}
