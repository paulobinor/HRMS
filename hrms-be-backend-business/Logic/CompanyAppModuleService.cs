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
using System.Text.Json;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Logic
{
    public class CompanyAppModuleService : ICompanyAppModuleService
    {
        private readonly IAuditLog _audit;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyAppModuleService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyAppModuleRepository _companyAppModuleRepository;
        private readonly ICompanyRepository _companyRepository;
        public CompanyAppModuleService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<CompanyAppModuleService> logger,
            ICompanyAppModuleRepository companyAppModuleRepository , ICompanyRepository companyRepository, IAuditLog audit, IMapper mapper)
        {
            _audit = audit;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _companyRepository = companyRepository;
            _accountRepository = accountRepository;
            _companyAppModuleRepository = companyAppModuleRepository;
        }
        //Get by status
        //
        public async Task<BaseResponse> GetCompanyAppModuleCount( RequesterInfo requester)
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
            

                var data = await _companyAppModuleRepository.GetCompanyAppModuleCount();

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module fetched successfully";
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

        public enum GetByStatus : byte
        {
            All,
            Approved,
            DisApproved
        }

        public async Task<BaseResponse> GetCompanyAppModuleStatus(GetByStatus status , RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();
                var data = new List<GetCompanyAppModuleCount>();

                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                switch (status)
                {
                    case GetByStatus.All:
                        data = await _companyAppModuleRepository.GetAllCompanyAppModule();
                        break;
                    case GetByStatus.Approved:
                        data = await _companyAppModuleRepository.GetCompanyAppModuleCount();
                        break;
                    case GetByStatus.DisApproved:
                        data = await _companyAppModuleRepository.GetDisapprovedCompanyAppModule();
                        break;
                    default:

                        response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Invalid status provided";
                        return response;
                }

                data = await _companyAppModuleRepository.GetDisapprovedCompanyAppModule();

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module fetched successfully";
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

        public async Task<BaseResponse> GetPendingCompanyAppModule(RequesterInfo requester)
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


                var data = await _companyAppModuleRepository.GetPendingCompanyAppModule();

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Pending company app module fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetCompanyAppModuleByCompanyID(long companyID,RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                var data = await _companyAppModuleRepository.GetCompanyAppModuleByCompanyID(companyID);

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetCompanyAppModuleByCompanyID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllAppModules(RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();


                var data = await _companyAppModuleRepository.GetAllAppModules();

                response.Data = data;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"App modules fetched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllAppModules ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> CreateCompanyAppModule(CreateCompanyAppModuleDTO createAppModule, RequesterInfo requester)
        {
            string successfulModules = string.Empty;
            string failedModules = string.Empty;
            var response = new BaseResponse();
            try
            {
                _logger.LogInformation($"CreateCompanyAppModule Request ==> {JsonConvert.SerializeObject(createAppModule)}");
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

                if(createAppModule.AppModuleId.Any(m => m <= 0))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid App Module selected";
                    return response;
                }else if(createAppModule.CompanyId <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid company selected";
                    return response;
                }

                var company = await _companyRepository.GetCompanyById(createAppModule.CompanyId);

                if(company == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company not found";
                    return response;
                }
                
                foreach(var appModuleId in createAppModule.AppModuleId)
                {
                    var appModule = await _companyAppModuleRepository.GetAppModuleByID(appModuleId);

                    if (appModule == null)
                    {
                        response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"App Module with {appModuleId} not found";

                        failedModules = $"{failedModules},App Module with {appModuleId} not found";
                        continue;
                        //return response;
                    }

                    var isExists = await _companyAppModuleRepository.GetCompanyAppModuleByCompanyandModuleID(createAppModule.CompanyId, appModuleId);
                    if (null != isExists)
                    {
                        response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"{appModule.AppModuleName} have already been added to {company.CompanyName}";


                        failedModules = $"{failedModules},{appModule.AppModuleName} have already been added to {company.CompanyName}";

                        continue;
                    }

                    var companyAppModule = new CompanyAppModuleDTO
                    {
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                        CompanyId = createAppModule.CompanyId,
                        CreatedByUserId = requester.UserId,
                        AppModuleId = appModuleId,
                        IsActive = false
                    };


                    var resp = await _companyAppModuleRepository.CreateCompanyAppModule(companyAppModule);
                    if (resp > 0)
                    {

                        successfulModules = $"{successfulModules} , {appModule.AppModuleName}";

                        companyAppModule.CompanyAppModuleId = resp;
                        var auditLog = new AuditLogDto
                        {
                            userId = requester.UserId,
                            actionPerformed = "CompanyAppModuleCreation",
                            payload = JsonConvert.SerializeObject(companyAppModule),
                            response = JsonConvert.SerializeObject(response),
                            actionStatus = response.ResponseMessage,
                            ipAddress = ipAddress
                        };
                        await _audit.LogActivity(auditLog);


                        response.Data = companyAppModule;
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Module {appModule.AppModuleName} added successfully to {company.CompanyName}";
                        // return response;
                    }
                    else
                    {
                        failedModules = $"{failedModules}, {appModule.AppModuleName} failed";
                    }
                }


                if (failedModules.Length == 0)
                {
                    response.Data = createAppModule;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Modules {successfulModules.TrimStart()} have been added successfully to {company.CompanyName}";
                    return response;
                }
                else
                {
                    response.Data = createAppModule;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"{failedModules.TrimStart()}";
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateAppModuleCompany ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApproveCompanyAppModule(long companyAppModuleID, RequesterInfo requester)
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
                var request = await _companyAppModuleRepository.GetCompanyAppModuleByID(companyAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
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

                var resp = await _companyAppModuleRepository.ApproveCompanyAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "CompanyAppModuleApproval",
                    payload = JsonConvert.SerializeObject(new { companyAppModuleID = companyAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module approved successfully";
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
        public async Task<BaseResponse> DisapproveCompanyAppModule(long companyAppModuleID , RequesterInfo requester)
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
                var request = await _companyAppModuleRepository.GetCompanyAppModuleByID(companyAppModuleID);

                if(request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
                    return response;
                }

                if(request.IsApproved == true)
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

                var resp = await _companyAppModuleRepository.DisapproveCompanyAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "CompanyAppModuleDisapproval",
                    payload = JsonConvert.SerializeObject(new { companyAppModuleID = companyAppModuleID}),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module disapproved successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapproveCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CompanyAppModuleActivationSwitch(long companyAppModuleID, RequesterInfo requester)
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
                var request = await _companyAppModuleRepository.GetCompanyAppModuleByID(companyAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
                    return response;
                }


                request.IsActive = !request.IsActive;


                var resp = await _companyAppModuleRepository.UpdateCompanyAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "CompanyAppModuleActivationSwitch",
                    payload = JsonConvert.SerializeObject(new { companyAppModuleID = companyAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module status  switched successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CompanyAppModuleActivationSwitch ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteCompanyAppModule(long companyAppModuleID, RequesterInfo requester)
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
                var request = await _companyAppModuleRepository.GetCompanyAppModuleByID(companyAppModuleID);

                if (request == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Company App Module not found";
                    return response;
                }


                request.IsDeleted = true;
                request.DeletedByUserId = requester.UserId;
                


                var resp = await _companyAppModuleRepository.UpdateCompanyAppModule(request);
                var auditLog = new AuditLogDto
                {
                    userId = requester.UserId,
                    actionPerformed = "CompanyAppModuleDeletion",
                    payload = JsonConvert.SerializeObject(new { companyAppModuleID = companyAppModuleID }),
                    response = JsonConvert.SerializeObject(request),
                    actionStatus = response.ResponseMessage,
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);


                response.Data = request;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Company app module deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteCompanyAppModule ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Operation failed. Kindly contact admin";
                response.Data = null;
                return response;
            }
        }

    }
}
