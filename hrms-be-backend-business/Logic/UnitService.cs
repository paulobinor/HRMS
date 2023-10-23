using ExcelDataReader;
using hrms_be_backend_business.Helpers;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class UnitService : IUnitService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<UnitService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;

        public UnitService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<UnitService> logger,
            IUnitRepository unitRepository, IAuditLog audit, IAuthService authService, IMailService mailService, IUriService uriService, IUserAppModulePrivilegeRepository privilegeRepository)
        {
            _audit = audit;

            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _unitRepository = unitRepository;
            _authService = authService;
            _mailService = mailService;
            _uriService = uriService;
            _privilegeRepository = privilegeRepository;
        }

        public async Task<ExecutedResult<string>> CreateUnit(CreateUnitDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UnitModulePrivilegeConstant.Create_Unit, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.UnitName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Unit Name is required";
                }
                if (payload.DepartmentId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "DepartmentId is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessUnitReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    UnitName = payload.UnitName,   
                    DepartmentId = payload.DepartmentId,
                    UnitHeadEmployeeId = payload.UnitHeadEmployeeId,
                    IsModifield = false,
                };
                string repoResponse = await _unitRepository.ProcessUnit(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateUnit",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (CreateUnit)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateUnit(UpdateUnitDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UnitModulePrivilegeConstant.Update_Unit, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.UnitName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Unit Name is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessUnitReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    UnitName = payload.UnitName,   
                    DepartmentId = payload.DepartmentId,
                    UnitHeadEmployeeId = payload.UnitHeadEmployeeId,
                    IsModifield = true,
                    UnitId = payload.UnitId,
                };
                string repoResponse = await _unitRepository.ProcessUnit(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateUnit",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Updated Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (UpdateUnit)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteUnit(DeleteUnitDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UnitModulePrivilegeConstant.Delete_Unit, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.Comment))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comment is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new DeleteUnitReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Comment = payload.Comment,
                    UnitId = payload.UnitId,
                };
                string repoResponse = await _unitRepository.DeleteUnit(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteUnit",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (DeleteUnit)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<UnitVm>>> GetUnites(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UnitModulePrivilegeConstant.View_Unit, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _unitRepository.GetUnites(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<UnitVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (GetUnites)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<UnitVm>>> GetUnitesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UnitModulePrivilegeConstant.View_Unit, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _unitRepository.GetUnitesDeleted(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<UnitVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (GetUnitesDeleted)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<UnitVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<UnitVm>> GetUnitById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<UnitVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _unitRepository.GetUnitById(Id);
                if (returnData == null)
                {
                    return new ExecutedResult<UnitVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<UnitVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (GetUnitById)=====>{ex}");
                return new ExecutedResult<UnitVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<UnitVm>> GetUnitByName(string UnitName, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<UnitVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _unitRepository.GetUnitByName(UnitName, accessUser.data.CompanyId);
                if (returnData == null)
                {
                    return new ExecutedResult<UnitVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<UnitVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitService (GetUnitByName)=====>{ex}");
                return new ExecutedResult<UnitVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

    }
}
