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
using static hrms_be_backend_business.Logic.CompanyAppModuleService;

namespace hrms_be_backend_business.Logic
{
    public class DepartmentalModulesService : IDepartmentalModulesService
    {
        private readonly IAuditLog _audit;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentalModulesService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyAppModuleRepository _companyAppModuleRepository;
        private readonly IDepartmentalModulesRepository _departmentalModulesRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        public DepartmentalModulesService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<DepartmentalModulesService> logger,
            IDepartmentalModulesRepository departmentalModulesRepository, IDepartmentRepository departmentRepository, IAuditLog audit, IMapper mapper, ICompanyAppModuleRepository companyAppModuleRepository, IUserAppModulePrivilegeRepository privilegeRepository , IAuthService authService)
        {
            _audit = audit;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
            _departmentalModulesRepository = departmentalModulesRepository;
            _companyAppModuleRepository = companyAppModuleRepository;
            _authService = authService;
            _companyAppModuleRepository = companyAppModuleRepository;
            _privilegeRepository = privilegeRepository;
        }

        public async Task<BaseResponse> GetDepartmentalAppModuleCount(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.View_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }


                var data = await _departmentalModulesRepository.GetDepartmentalAppModuleCount(accessUser.data.CompanyId);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Departmental app module fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetDepartmentalAppModuleCount ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetDepartmentAppModuleStatus(GetByStatus status, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var data = new List<GetDepartmentalModuleCount>();
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.View_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }

                switch (status)
                {
                    case GetByStatus.All:
                        data = await _departmentalModulesRepository.GetAllDepartmentalAppModuleCount(accessUser.data.CompanyId);
                        break;
                    case GetByStatus.Approved:
                        data = await _departmentalModulesRepository.GetDepartmentalAppModuleCount(accessUser.data.CompanyId);
                        break;
                    case GetByStatus.DisApproved:
                        data = await _departmentalModulesRepository.GetDisapprovedDepartmentalAppModuleCount(accessUser.data.CompanyId);
                        break;
                    default:

                        response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Invalid status provided";
                        return response;
                }

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Department app module fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetCompanyAppModuleCount ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetPendingDepartmentalAppModule(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.View_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }

                


                var data = await _departmentalModulesRepository.GetPendingDepartmentalAppModule(accessUser.data.CompanyId);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Pending departmental app modules fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetDepartmentalAppModuleByDepartmentID(long departmentID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyAppModuleConstant.View_CompanyAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }


                var data = await _departmentalModulesRepository.GetDepartmentalAppModuleByDepartmentID(departmentID);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Department app modules fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetDepartmentalAppModuleByDepartmentID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO createDepartmentalAppModule, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            string successfulModules = string.Empty;
            string failedModules = string.Empty;
            try
            {
                _logger.LogInformation($"CreateDepartmentalAppModule Request ==> {JsonConvert.SerializeObject(createDepartmentalAppModule)}");



                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.Create_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }

                if (createDepartmentalAppModule.AppModuleId == null || createDepartmentalAppModule.AppModuleId.Any(m => m <= 0))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid App Module selected";
                    return response;
                }
                else if (createDepartmentalAppModule.DepartmentId <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid Department selected";
                    return response;
                }

                var department = await _departmentRepository.GetDepartmentById(createDepartmentalAppModule.DepartmentId);

                if (department == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Department not found";
                    return response;
                }

                foreach (var appModule in createDepartmentalAppModule.AppModuleId)
                {
                    var appModuleDetails = await _companyAppModuleRepository.GetAppModuleByID(appModule);

                    if (appModuleDetails == null)
                    {
                        failedModules = $"{failedModules} , Module with ID - {appModule} not found";
                        continue;
                    }

                    var isExists = await _departmentalModulesRepository.GetDepartmentalAppModuleByDepartmentandModuleID(createDepartmentalAppModule.DepartmentId, appModule);
                    if (null != isExists)
                    {
                        response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"{appModuleDetails.AppModuleName} have already been added to {department.DepartmentName}";

                        failedModules = $"{failedModules}, {appModuleDetails.AppModuleName} have already been added to {department.DepartmentName}";

                        continue;
                        //return response;
                    }

                    var departmentAppModule = new DepartmentalModulesDTO
                    {
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        DepartmentId = createDepartmentalAppModule.DepartmentId,
                        CreatedByUserId = accessUser.data.UserId,
                        AppModuleId = appModule,
                        IsActive = false
                    };


                    var resp = await _departmentalModulesRepository.CreateDepartmentalAppModule(departmentAppModule);
                    if (resp > 0)
                    {
                        successfulModules = $"{successfulModules} , {appModuleDetails.AppModuleName}";

                        departmentAppModule.DepartmentalModuleId = resp;
                        var auditLog = new AuditLogDto
                        {
                            userId = accessUser.data.UserId,
                            actionPerformed = "DepartmentAppModuleCreation",
                            payload = JsonConvert.SerializeObject(departmentAppModule),
                            response = JsonConvert.SerializeObject(response),
                            actionStatus = response.ResponseMessage,
                            ipAddress = RemoteIpAddress
                        };
                        await _audit.LogActivity(auditLog);

                    }
                    else
                    {
                        failedModules = $"{failedModules}, {appModuleDetails.AppModuleName} failed";
                    }
                    
                }
                if(failedModules.Length == 0)
                {
                    response.Data = createDepartmentalAppModule;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Modules {successfulModules.TrimStart()} have been added successfully to {department.DepartmentName}";
                    return response;
                }
                else
                {
                    response.Data = createDepartmentalAppModule;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"{failedModules.TrimStart()}";
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateDepartmentAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApproveDepartmentalAppModule(ApproveDepartmentalModules request, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            var listOfResponse = new List<Response>();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };
                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.Approve_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };
                }

                foreach(var departmentAppModuleID in request.DepartmentAppModuleID)
                {
                    var data = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                    if (data == null)
                    {
                        listOfResponse.Add(new Response { ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'), ResponseMessage = $"Departmental App Module with ID {departmentAppModuleID} not found" });
                        continue;
                    }

                    if (data.IsApproved == true)
                    {
                        listOfResponse.Add(new Response { ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'), ResponseMessage = $"Record  with ID {departmentAppModuleID}  already approved" });
                        continue;
                    }

                    if (data.CreatedByUserId == accessUser.data.UserId)
                    {
                        listOfResponse.Add(new Response { ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), ResponseMessage = $"Auhorization error. Same user can't act as maker and checker" });
                        continue;
                    }

                    data.IsApproved = true;
                    data.DateApproved = DateTime.Now;
                    data.IsActive = true;
                    data.ApprovedByUserId = accessUser.data.UserId;

                    var resp = await _departmentalModulesRepository.ApproveDepartmentalAppModule(data);

                    listOfResponse.Add(new Response { ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), ResponseMessage = $"Departmental app module {data.AppModuleName} approved successfully" });
                    continue;
                }
                
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DepartmentalAppModuleApproval",
                    payload = JsonConvert.SerializeObject(request),
                    response = JsonConvert.SerializeObject(listOfResponse),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = listOfResponse;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Departmental app module processed successfully";
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
        public async Task<BaseResponse> DisapproveDepartmentalAppModule(long departmentAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.Approve_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Department App Module not found";
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

                var resp = await _departmentalModulesRepository.DisapproveDepartmentalAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DepartmentalAppModuleDisapproval",
                    payload = JsonConvert.SerializeObject(new { DepartmentalAppModuleID = departmentAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Departmental app module disapproved successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapproveDepartmentalAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DepartmentAppModuleActivationSwitch(long departmentAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.Update_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Department App Module not found";
                    return response;
                }


                request.IsActive = !request.IsActive;


                var resp = await _departmentalModulesRepository.UpdateDepartmentAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DepartmentAppModuleActivationSwitch",
                    payload = JsonConvert.SerializeObject(new { departmentAppModuleID = departmentAppModuleID }),
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

        public async Task<BaseResponse> DeleteDepartmentAppModule(long departmentAppModuleID, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.NotAuthenticated).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(DepartmentalAppModuleConstant.Delete_DepartmentAppModule, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                var request = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
                    return response;
                }


                request.IsDeleted = true;
                request.DeletedByUserId = accessUser.data.UserId;



                var resp = await _departmentalModulesRepository.UpdateDepartmentAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DepartmentAppModuleDeletion",
                    payload = JsonConvert.SerializeObject(new { departmentAppModuleID = departmentAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Department app module deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteDepartmentAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

    }
}
