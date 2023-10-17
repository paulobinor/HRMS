using AutoMapper;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public UserAppModulePrivilegesService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<UserAppModulePrivilegesService> logger,
            IUserAppModulePrivilegeRepository userAppModulePrivilegeRepository, IDepartmentRepository departmentRepository, IAuditLog audit, IMapper mapper, ICompanyAppModuleRepository companyAppModuleRepository, IEmployeeRepository employeeRepository)
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
        }

        public async Task<BaseResponse> GetAppModulePrivileges(RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();



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

        public async Task<BaseResponse> GetAppModulePrivilegesByAppModuleID(long appModuleID ,RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

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

        public async Task<BaseResponse> GetUserAppModulePrivileges(RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                var data = await _userAppModulePrivilegeRepository.GetUserAppModulePrivileges();

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

        public async Task<BaseResponse> GetPendingUserAppModulePRivilege(RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }


                var data = await _userAppModulePrivilegeRepository.GetPendingUserAppModulePrivileges();

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

        public async Task<BaseResponse> GetUserAppModulePrivilegesByUserID(long userID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

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


        public async Task<BaseResponse> CreateUserAppModulePrivileges(CreateUserAppModulePrivilegesDTO createUserAppModulePrivileges, RequesterInfo requester)
        {
            var response = new BaseResponse();
            string successfulModules = string.Empty;
            string failedModules = string.Empty;
            try
            {
                _logger.LogInformation($"CreateUserAppModulePrivileges Request ==> {JsonConvert.SerializeObject(createUserAppModulePrivileges)}");
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                if (createUserAppModulePrivileges.PrivilegeID == null ||createUserAppModulePrivileges.PrivilegeID.Any(m => m <= 0))
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
                        response.ResponseMessage = $"{privilegesDetails.PrivilegeName} have already been added to {User.GradeName}";

                        failedModules = $"{failedModules}, {privilegesDetails.PrivilegeName} have already been added to {User.GradeName}";

                        //return response;
                    }

                    var userAppModulePrivilege = new UserAppModulePrivilegesDTO
                    {
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        AppModulePrivilegeID = privileges,
                        CreatedByUserId = requester.UserId,
                        UserID = createUserAppModulePrivileges.UserID,
                        IsActive = false
                    };


                    var resp = await _userAppModulePrivilegeRepository.CreateUserAppModulePrivileges(userAppModulePrivilege);
                    if (resp > 0)
                    {
                        successfulModules = $"{successfulModules} , {privilegesDetails.PrivilegeName}";

                        userAppModulePrivilege.UserAppModulePrivilegeID = resp;
                        var auditLog = new AuditLogDto
                        {
                            userId = requester.UserId,
                            actionPerformed = "UserAppModulePrivilegeCreation",
                            payload = JsonConvert.SerializeObject(userAppModulePrivilege),
                            response = JsonConvert.SerializeObject(response),
                            actionStatus = response.ResponseMessage,
                            ipAddress = ipAddress
                        };
                        await _audit.LogActivity(auditLog);

                    }
                    else
                    {
                        failedModules = $"{failedModules}, {privilegesDetails.PrivilegeName} failed";
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

        public async Task<BaseResponse> ApproveUserAppModulePrivilege(long userAppModulePrivilegeID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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

                if (request.CreatedByUserId == requester.UserId)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Auhorization error. Same user can't act as maker and checker";
                    return response;
                }

                request.IsApproved = true;
                request.DateApproved = DateTime.Now;
                request.IsActive = true;
                request.ApprovedByUserId = requester.UserId;

                var resp = await _userAppModulePrivilegeRepository.ApproveUserAppModulePrivileges(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "UserAppModulePrivilegeApproval",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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
        public async Task<BaseResponse> DisapproveUserAppModulePrivilage(long userAppModulePrivilegeID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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

                if (request.CreatedByUserId == requester.UserId)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Auhorization error. Same user can't act as maker and checker";
                    return response;
                }

                request.IsDisapproved = true;
                request.DateApproved = DateTime.Now;
                request.IsActive = false;
                request.DisapprovedByUserId = requester.UserId;

                var resp = await _userAppModulePrivilegeRepository.DisapproveUserAppModulePrivilege(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "USerAppModulePrivilegeDisapproval",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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

        public async Task<BaseResponse> UserAppModulePrivilegeActivationSwitch(long userAppModulePrivilegeID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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
                    userId = requester.UserId,
                    actionPerformed = "DepartmentAppModuleActivationSwitch",
                    payload = JsonConvert.SerializeObject(new { UserAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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

        public async Task<BaseResponse> DeleteUserAppModulePrivilege(long userAppModulePrivilegeID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 2 && Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }
                var request = await _userAppModulePrivilegeRepository.GetUserAppModulePrivilegeByID(userAppModulePrivilegeID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User App Module privilege not found";
                    return response;
                }


                request.IsDeleted = true;
                request.DeletedByUserId = requester.UserId;



                var resp = await _userAppModulePrivilegeRepository.UpdateUserAppModulePRivileges(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "UserAppModulePrivilageDeletion",
                    payload = JsonConvert.SerializeObject(new { userAppModulePrivilegeID = userAppModulePrivilegeID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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
