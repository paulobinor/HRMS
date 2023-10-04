using AutoMapper;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DepartmentalModulesService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<DepartmentalModulesService> logger,
            IDepartmentalModulesRepository departmentalModulesRepository, IDepartmentRepository departmentRepository, IAuditLog audit, IMapper mapper, ICompanyAppModuleRepository companyAppModuleRepository)
        {
            _audit = audit;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
            _departmentalModulesRepository = departmentalModulesRepository;
            _companyAppModuleRepository = companyAppModuleRepository;
        }

        public async Task<BaseResponse> GetDepartmentalAppModuleCount(RequesterInfo requester)
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


                var data = await _departmentalModulesRepository.GetDepartmentalAppModuleCount();

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

        public async Task<BaseResponse> GetDepartmentAppModuleStatus(GetByStatus status, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();
                var data = new List<GetDepartmentalModuleCount>();

                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                switch (status)
                {
                    case GetByStatus.All:
                        data = await _departmentalModulesRepository.GetAllDepartmentalAppModuleCount();
                        break;
                    case GetByStatus.Approved:
                        data = await _departmentalModulesRepository.GetDepartmentalAppModuleCount();
                        break;
                    case GetByStatus.DisApproved:
                        data = await _departmentalModulesRepository.GetDisapprovedDepartmentalAppModuleCount();
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

        public async Task<BaseResponse> GetPendingDepartmentalAppModule(RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }


                var data = await _departmentalModulesRepository.GetPendingDepartmentalAppModule();

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

        public async Task<BaseResponse> GetDepartmentalAppModuleByDepartmentID(long departmentID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


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


        public async Task<BaseResponse> CreateDepartmentalAppModule(CreateDepartmentalModuleDTO createDepartmentalAppModule, RequesterInfo requester)
        {
            var response = new BaseResponse();
            string successfulModules = string.Empty;
            string failedModules = string.Empty;
            try
            {
                _logger.LogInformation($"CreateDepartmentalAppModule Request ==> {JsonConvert.SerializeObject(createDepartmentalAppModule)}");
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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
                        CreatedByUserId = requester.UserId,
                        AppModuleId = appModule,
                        IsActive = false
                    };


                    var resp = await _departmentalModulesRepository.CreateDepartmentalAppModule(departmentAppModule);
                    if (resp > 0)
                    {
                        successfulModules = $"{successfulModules} , {appModuleDetails.AppModuleName}";

                        departmentAppModule.DeparmentalModuleId = resp;
                        var auditLog = new AuditLogDto
                        {
                            userId = requester.UserId,
                            actionPerformed = "DepartmentAppModuleCreation",
                            payload = JsonConvert.SerializeObject(departmentAppModule),
                            response = JsonConvert.SerializeObject(response),
                            actionStatus = response.ResponseMessage,
                            ipAddress = ipAddress
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

        public async Task<BaseResponse> ApproveDepartmentalAppModule(long departmentAppModuleID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }
                var request = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Departmental App Module not found";
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

                request.IsApproved = true;
                request.DateApproved = DateTime.Now;
                request.IsActive = true;
                request.ApprovedByUserId = requester.UserId;

                var resp = await _departmentalModulesRepository.ApproveDepartmentalAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "DepartmentalAppModuleApproval",
                    payload = JsonConvert.SerializeObject(new { DepartmentalAppModuleID = departmentAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Departmental app module approved successfully";
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
        public async Task<BaseResponse> DisapproveDepartmentalAppModule(long departmentAppModuleID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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

                var resp = await _departmentalModulesRepository.DisapproveDepartmentalAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "DepartmentalAppModuleDisapproval",
                    payload = JsonConvert.SerializeObject(new { DepartmentalAppModuleID = departmentAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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

        public async Task<BaseResponse> DepartmentAppModuleActivationSwitch(long departmentAppModuleID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
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
                    userId = requester.UserId,
                    actionPerformed = "DepartmentAppModuleActivationSwitch",
                    payload = JsonConvert.SerializeObject(new { departmentAppModuleID = departmentAppModuleID }),
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

        public async Task<BaseResponse> DeleteDepartmentAppModule(long departmentAppModuleID, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }
                var request = await _departmentalModulesRepository.GetDepartmentalAppModuleByID(departmentAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
                    return response;
                }


                request.IsDeleted = true;
                request.DeletedByUserId = requester.UserId;



                var resp = await _departmentalModulesRepository.UpdateDepartmentAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "DepartmentAppModuleDeletion",
                    payload = JsonConvert.SerializeObject(new { departmentAppModuleID = departmentAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
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
