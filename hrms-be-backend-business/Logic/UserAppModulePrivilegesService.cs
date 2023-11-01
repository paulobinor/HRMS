using AutoMapper;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class UserAppModulePrivilegesService : IUserAppModulePrivilegeService
    {
        private readonly IAuditLog _audit;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAppModulePrivilegesService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyAppModuleRepository _companyAppModuleRepository;
        private readonly IUserAppModulePrivilegeRepository _userAppModulePrivilegeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        public UserAppModulePrivilegesService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<UserAppModulePrivilegesService> logger,
            IUserAppModulePrivilegeRepository userAppModulePrivilegeRepository, IDepartmentRepository departmentRepository, IAuditLog audit, IMapper mapper, ICompanyAppModuleRepository companyAppModuleRepository, IEmployeeRepository employeeRepository, IUserAppModulePrivilegeRepository privilegeRepository, IAuthService authService)
        {
            _audit = audit;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
            _userAppModulePrivilegeRepository = userAppModulePrivilegeRepository;
            _companyAppModuleRepository = companyAppModuleRepository;
            _employeeRepository = employeeRepository;
            _authService = authService;
            _companyAppModuleRepository = companyAppModuleRepository;
            _privilegeRepository = privilegeRepository;

        }

        public async Task<BaseResponse> GetAppModulePrivileges(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }

                var data = await _userAppModulePrivilegeRepository.GetAppModulePrivileges();

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"App module privileges fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAppModulePrivilegesByAppModuleID(long appModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                
                var data = await _userAppModulePrivilegeRepository.GetAppModulePrivilegeByAppModuleID(appModuleID);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"App module privileges fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetUserAppModulePrivileges(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.View_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }


                var data = await _userAppModulePrivilegeRepository.GetUserAppModulePrivileges(accessUser.data.CompanyId);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"User app module privileges fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetPendingUserAppModulePRivilege(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.View_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }


                var data = await _userAppModulePrivilegeRepository.GetPendingUserAppModulePrivileges(accessUser.data.CompanyId);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Pending user app modules fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingUserAppModulePRivilege ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetUserAppModulePrivilegesByUserID(long userID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.View_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }

                var data = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegesByUserID(userID);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"User app modules privileges fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetUserAppModulePrivilegesByUserID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO createUserAppModulePrivileges, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            string successfulModules = string.Empty;
            string failedModules = string.Empty;
            _logger.LogInformation($"CreateUserAppModulePrivileges Request ==> {JsonConvert.SerializeObject(createUserAppModulePrivileges)}");
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.Create_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }

                if (createUserAppModulePrivileges.PrivilegeID == null || createUserAppModulePrivileges.PrivilegeID.Any(m => m <= 0))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid App Module privilege selected";
                    return response;
                }
                else if (createUserAppModulePrivileges.UserID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid User selected";
                    return response;
                }

                var User = await _employeeRepository.GetEmployeeByUserId(createUserAppModulePrivileges.UserID);

                if (User == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User not found";
                    return response;
                }

                var appModulePrivilegeList = await _userAppModulePrivilegeRepository.GetAppModulePrivileges();

                foreach (var privileges in createUserAppModulePrivileges.PrivilegeID)
                {
                    var privilegesDetails = appModulePrivilegeList.FirstOrDefault(x => x.AppModulePrivilegeID == privileges);

                    if (privilegesDetails == null)
                    {
                        failedModules = $"{failedModules} , privilege with ID - {privilegesDetails} not found";
                        continue;
                    }

                    var isExists = await _userAppModulePrivilegeRepository.GetUserAppModuleByUserandPrivilegeID(createUserAppModulePrivileges.UserID, privileges);
                    if (null != isExists)
                    {
                        response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"{privilegesDetails.AppModulePrivilegeName} have already been added to {User.GradeName}";

                        failedModules = $"{failedModules}, {privilegesDetails.AppModulePrivilegeName} have already been added to {User.GradeName}";

                        //return response;
                    }

                    var userAppModulePrivilege = new UserAppModulePrivilegesDTO
                    {
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        AppModulePrivilegeID = privileges,
                        CreatedByUserId = accessUser.data.UserId,
                        UserID = createUserAppModulePrivileges.UserID,
                        IsActive = false
                    };


                    var resp = await _userAppModulePrivilegeRepository.CreateUserAppModulePrivileges(userAppModulePrivilege);
                    if (resp > 0)
                    {
                        successfulModules = $"{successfulModules} , {privilegesDetails.AppModulePrivilegeName}";

                        userAppModulePrivilege.UserAppModulePrivilegeID = resp;
                        var auditLog = new AuditLogDto
                        {
                            userId = accessUser.data.UserId,
                            actionPerformed = "UserAppModulePrivilegeCreation",
                            payload = JsonConvert.SerializeObject(userAppModulePrivilege),
                            response = JsonConvert.SerializeObject(response),
                            actionStatus = response.ResponseMessage,
                            ipAddress = RemoteIpAddress
                        };
                        await _audit.LogActivity(auditLog);

                    }
                    else
                    {
                        failedModules = $"{failedModules}, {privilegesDetails.AppModulePrivilegeName} failed";
                    }

                }
                if (failedModules.Length == 0)
                {
                    response.Data = createUserAppModulePrivileges;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Privileges {successfulModules.TrimStart()} have been added successfully to {User.FirstName}";
                    return response;
                }
                else
                {
                    response.Data = createUserAppModulePrivileges;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"{failedModules.TrimStart()}";
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateUserAppModulePrivileges ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApproveUserAppModulePrivilege(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.Approve_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegeByID(userAppModulePrivilegeID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User App Module privilege not found";
                    return response;
                }

                if (request.IsApproved == true || request.IsDisapproved == true)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Record already approved";
                    return response;
                }

                if (request.CreatedByUserId == accessUser.data.UserId)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Auhorization error. Same user can't act as maker and checker";
                    return response;
                }

                request.IsApproved = true;
                request.DateApproved = DateTime.Now;
                request.IsActive = true;
                request.ApprovedByUserId = accessUser.data.UserId;

                var resp = await _userAppModulePrivilegeRepository.ApproveUserAppModulePrivileges(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UserAppModulePrivilegeApproval",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"User app module privilege approved successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApproveCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> DisapproveUserAppModulePrivilage(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.Approve_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegeByID(userAppModulePrivilegeID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User App Module privilege not found";
                    return response;
                }

                if (request.IsApproved == true)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Record already approved";
                    return response;
                }

                if (request.CreatedByUserId == accessUser.data.UserId)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Auhorization error. Same user can't act as maker and checker";
                    return response;
                }

                request.IsDisapproved = true;
                request.DateApproved = DateTime.Now;
                request.IsActive = false;
                request.DisapprovedByUserId = accessUser.data.UserId;

                var resp = await _userAppModulePrivilegeRepository.DisapproveUserAppModulePrivilege(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "USerAppModulePrivilegeDisapproval",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"User app module privilege disapproved successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapproveUserAppModulePrivilage ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UserAppModulePrivilegeActivationSwitch(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.Update_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegeByID(userAppModulePrivilegeID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User App Module privilege not found";
                    return response;
                }


                request.IsActive = !request.IsActive;


                var resp = await _userAppModulePrivilegeRepository.UpdateUserAppModulePRivileges(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DepartmentAppModuleActivationSwitch",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Department app module status  switched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DepartmentAppModuleActivationSwitch ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteUserAppModulePrivilege(long userAppModulePrivilegeID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserAppmodulePrivilegesConstant.Delete_UserAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegeByID(userAppModulePrivilegeID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User App Module privilege not found";
                    return response;
                }


                request.IsDeleted = true;
                request.DeletedByUserId = accessUser.data.UserId;



                var resp = await _userAppModulePrivilegeRepository.UpdateUserAppModulePRivileges(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UserAppModulePrivilageDeletion",
                    payload = JsonConvert.SerializeObject(new { userAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"User app module privilege deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteUserAppModulePrivilege ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

    }
}
