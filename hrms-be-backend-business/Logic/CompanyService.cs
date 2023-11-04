using AutoMapper;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.Helpers;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace hrms_be_backend_business.Logic
{
    public class CompanyService : ICompanyService
    {
        private readonly IAuditLog _audit;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAppModulesRepository _appModulesRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentalModulesRepository _departmentalModulesRepository;

        public CompanyService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<CompanyService> logger,
            ICompanyRepository companyRepository, IAuditLog audit, IAuthService authService, IMailService mailService, IUriService uriService, IUserAppModulePrivilegeRepository privilegeRepository, IAppModulesRepository appModulesRepository, IDepartmentRepository departmentRepository, IDepartmentalModulesRepository departmentalModulesRepository)
        {
            _audit = audit;
            _authService = authService;
            _mailService = mailService;
            _uriService = uriService;
            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _companyrepository = companyRepository;
            _privilegeRepository = privilegeRepository;
            _appModulesRepository = appModulesRepository;
            _departmentRepository = departmentRepository;
            _departmentalModulesRepository = departmentalModulesRepository;
        }
        public async Task<ExecutedResult<string>> CreateCompany(CompanyCreateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.Create_Company, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.CompanyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Company Name is required";
                }
                if (string.IsNullOrEmpty(payload.FullAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Full Address is required";
                }
                if (string.IsNullOrEmpty(payload.Email))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Email Address is required";
                }
                if (string.IsNullOrEmpty(payload.ContactPhone))
                {
                    isModelStateValidate = false;
                    validationMessage += " Contact Phone is required";
                }
                if (string.IsNullOrEmpty(payload.AdminFirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Admin First name is required";
                }
                if (payload.AdminLastName == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin Last name is required";
                }
                if (payload.AdminOfficialMail == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin Official email is required";
                }
                if (payload.AdminPhoneNumber == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin Phone number is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var defaultPassword = Utils.GenerateDefaultPassword(6);
                var repoPayload = new ProcessCompanyReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    FullAddress = payload.FullAddress,
                    CompanyCode = payload.CompanyCode,
                    CompanyLogo = payload.CompanyLogo,
                    CompanyName = payload.CompanyName,
                    ContactPhone = payload.ContactPhone,
                    Email = payload.Email,
                    IsPublicSector = payload.IsPublicSector,
                    Website = payload.Website,
                    AdminFirstName = payload.AdminFirstName,
                    AdminLastName = payload.AdminLastName,
                    AdminMiddleName = payload.AdminMiddleName,
                    AdminOfficialMail = payload.AdminOfficialMail,
                    AdminPasswordHash = defaultPassword,
                    AdminPhoneNumber = payload.AdminPhoneNumber,
                    IsModifield = false,
                };
                string repoResponse = await _companyrepository.ProcessCompany(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var UserId = repoResponse.Replace("Success", "");
                //var appModules = await _appModulesRepository.GetAppModules();
                //var departmentPayload = new ProcessDepartmentReq
                //{
                //    CreatedByUserId = Convert.ToInt64(UserId),
                //    DateCreated = DateTime.Now,
                //    DepartmentName = "Default Department",
                //    IsHr = true,
                //    IsModifield = false,
                //};
                //var deptResponse = await _departmentRepository.ProcessDepartment(departmentPayload);
                //var departmentId = deptResponse.Replace("Success", "");
                //foreach (var appModule in appModules)
                //{
                //    var departmentalModulesDTO = new DepartmentalModulesReq
                //    {
                //        AppModuleId = appModule.AppModuleId,
                //        DepartmentId = Convert.ToInt64(departmentId),
                //        CreatedByUserId = Convert.ToInt64(UserId),
                //        DateCreated = DateTime.Now,
                //    };
                //    await _departmentalModulesRepository.CreateDepartmentalAppModule(departmentalModulesDTO);
                //    var appModulePrivileges = await _privilegeRepository.GetAppModulePrivilegeByModuleID(appModule.AppModuleId);
                //    foreach (var appModulePrivilege in appModulePrivileges)
                //    {
                //        var userAppModulePrivilegesReq = new UserAppModulePrivilegesReq
                //        {
                //            AppModulePrivilegeId = appModulePrivilege.AppModulePrivilegeID,
                //            CreatedByUserId = Convert.ToInt64(UserId),
                //            UserId = Convert.ToInt64(UserId),
                //            DateCreated = DateTime.Now
                //        };
                //        await _privilegeRepository.CreateUserAppModulePrivileges(userAppModulePrivilegesReq);
                //    }
                //}
                var userDetails = await _accountRepository.GetUserById(Convert.ToInt64(UserId));
                string token = $"{RandomGenerator.GetNumber(20)}{defaultPassword}";

                _mailService.SendEmailApproveUser(userDetails.OfficialMail, $"{userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName}", defaultPassword, "Xpress HRMS User Creation", EncryptDecrypt.EncryptResult(token));

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateCompany",
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
                _logger.LogError($"CompanyService (CreateCompany)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateCompany(CompanyUpdateDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.Update_Company, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.CompanyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Company Name is required";
                }
                if (string.IsNullOrEmpty(payload.FullAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Full Address is required";
                }
                if (string.IsNullOrEmpty(payload.Email))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Email Address is required";
                }
                if (string.IsNullOrEmpty(payload.ContactPhone))
                {
                    isModelStateValidate = false;
                    validationMessage += " Contact Phone is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessCompanyReq
                {
                    CompanyId = payload.CompanyId,
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    FullAddress = payload.FullAddress,
                    CompanyCode = payload.CompanyCode,
                    CompanyLogo = payload.CompanyLogo,
                    CompanyName = payload.CompanyName,
                    ContactPhone = payload.ContactPhone,
                    Email = payload.Email,
                    IsPublicSector = payload.IsPublicSector,
                    Website = payload.Website,
                    IsModifield = true,
                };
                string repoResponse = await _companyrepository.ProcessCompany(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateCompany",
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
                _logger.LogError($"CompanyService (UpdateCompany)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteCompany(long CompanyId, string ReasonToDelete, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.Delete_Company, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(ReasonToDelete))
                {
                    isModelStateValidate = false;
                    validationMessage += "Reason To Delete is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new DeleteCompanyReq
                {
                    CompanyId = CompanyId,
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Comment = ReasonToDelete
                };
                string repoResponse = await _companyrepository.DeleteCompany(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteCompany",
                    payload = null,
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (DeleteCompany)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> ApproveCompany(long CompanyId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.Approve_Company, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                var repoPayload = new ApproveCompanyReq
                {
                    CompanyId = CompanyId,
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now
                };
                string repoResponse = await _companyrepository.ApproveCompany(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "ApproveCompany",
                    payload = null,
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };


            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (ApproveCompany)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompanies(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompanies(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompanies)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompaniesPending(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompaniesPending(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompaniesActivated)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompaniesActivated(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompaniesActivated(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompaniesActivated)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompaniesDeactivated(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompaniesDeactivated(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompaniesDeactivated)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompaniesPublicSector(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompaniesPublicSector(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompaniesPublicSector)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<CompanyVm>>> GetCompaniesPrivateSector(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _companyrepository.GetCompaniesPrivateSector(filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<CompanyVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompaniesPrivateSector)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<CompanyVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<CompanyFullVm>> GetCompanyById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<CompanyFullVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BkCompanyModulePrivilegeConstant.View_Company_Details, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<CompanyFullVm>() { responseMessage =ResponseCode.NoPrivilege.ToString(), responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                var returnData = await _companyrepository.GetCompanyById(Id);
                if (returnData == null)
                {
                    return new ExecutedResult<CompanyFullVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<CompanyFullVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompanyById)=====>{ex}");
                return new ExecutedResult<CompanyFullVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<CompanyFullVm>> GetCompanyByUser(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<CompanyFullVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }                
                var returnData = await _companyrepository.GetCompanyById(accessUser.data.CompanyId);
                if (returnData == null)
                {
                    return new ExecutedResult<CompanyFullVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<CompanyFullVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyService (GetCompanyByUser)=====>{ex}");
                return new ExecutedResult<CompanyFullVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
