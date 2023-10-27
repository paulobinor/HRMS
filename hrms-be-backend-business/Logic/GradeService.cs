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
    public class GradeService : IGradeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<GradeService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;

        public GradeService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<GradeService> logger,
            IGradeRepository gradeRepository, IAuditLog audit, IAuthService authService, IMailService mailService, IUriService uriService, IUserAppModulePrivilegeRepository privilegeRepository)
        {
            _audit = audit;

            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _gradeRepository = gradeRepository;
            _authService = authService;
            _mailService = mailService;
            _uriService = uriService;
            _privilegeRepository = privilegeRepository;
        }

        public async Task<ExecutedResult<string>> CreateGrade(CreateGradeDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(GradeModulePrivilegeConstant.Create_Grade, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.GradeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Grade Name is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessGradeReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    GradeName = payload.GradeName,                   
                    IsModifield = false,
                };
                string repoResponse = await _gradeRepository.ProcessGrade(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateGrade",
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
                _logger.LogError($"GradeService (CreateGrade)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> CreateGradeBulkUpload(IFormFile payload, string AccessKey, IEnumerable<Claim> claim, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            try
            {
                if (payload == null || payload.Length <= 0)
                    return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ValidationError).ToString(), responseMessage = "No file attached" };
                else if (!Path.GetExtension(payload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ValidationError).ToString(), responseMessage = "File not an Excel Format" };
                else
                {
                    var stream = new MemoryStream();
                    await payload.CopyToAsync(stream);

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    var reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    int rowCount = ds.Tables[0].Rows.Count;
                    DataTable serviceDetails = ds.Tables[0];

                    int k = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        string GradeName = serviceDetails.Rows[0][0].ToString();


                        if (GradeName != "GradeName")
                            return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ValidationError).ToString(), responseMessage = "File header not in the Right format" };
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                string gradeName = serviceDetails.Rows[row][0].ToString();

                                if (string.IsNullOrEmpty(gradeName))
                                {
                                    errorOutput.Append($"Row {row} failed due to Invalid grade name {gradeName}" + "\n");
                                    continue;
                                }
                                //var company = serviceDetails.Rows[row][2].ToString();

                                var gradeRequest = new CreateGradeDto
                                {
                                    GradeName = gradeName

                                };

                                var resp = await CreateGrade(gradeRequest, AccessKey, claim, requester.IpAddress, requester.Port);


                                if (resp.responseCode == "00")
                                {
                                    k++;
                                }
                                else
                                {
                                    errorOutput.Append($"Row {row} failed due to {resp.responseMessage}" + "\n");
                                }
                            }
                        }

                    }



                    if (k == rowCount - 1)
                    {
                        return new ExecutedResult<string> { responseCode = ((int)ResponseCode.Ok).ToString(), responseMessage = "All record inserted successfully" };
                    }
                    else
                    {
                        return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ProcessingError).ToString(), responseMessage = errorOutput.ToString() };
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ProcessingError).ToString(), responseMessage = "Exception occured creating department" };
            }
        }

        public async Task<ExecutedResult<string>> UpdateGrade(UpdateGradeDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(GradeModulePrivilegeConstant.Update_Grade, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.GradeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Grade Name is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessGradeReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    GradeName = payload.GradeName,                  
                    IsModifield = true,
                    GradeId = payload.GradeId,
                };
                string repoResponse = await _gradeRepository.ProcessGrade(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateGrade",
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
                _logger.LogError($"GradeService (UpdateGrade)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteGrade(DeleteGradeDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(GradeModulePrivilegeConstant.Delete_Grade, accessUser.data.UserId);
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
                var repoPayload = new DeleteGradeReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Comment = payload.Comment,
                    GradeId = payload.GradeId,
                };
                string repoResponse = await _gradeRepository.DeleteGrade(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteGrade",
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
                _logger.LogError($"GradeService (DeleteGrade)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<GradeVm>>> GetGrades(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(GradeModulePrivilegeConstant.View_Grade, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _gradeRepository.GetGrades(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<GradeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeService (GetGradees)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<GradeVm>>> GetGradesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(GradeModulePrivilegeConstant.View_Grade, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _gradeRepository.GetGradesDeleted(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<GradeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeService (GetGradeesDeleted)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<GradeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<GradeVm>> GetGradeById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<GradeVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _gradeRepository.GetGradeById(Id);
                if (returnData == null)
                {
                    return new ExecutedResult<GradeVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<GradeVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeService (GetGradeById)=====>{ex}");
                return new ExecutedResult<GradeVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<GradeVm>> GetGradeByName(string GradeName, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<GradeVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };

                }
                var returnData = await _gradeRepository.GetGradeByName(GradeName, accessUser.data.CompanyId);
                if (returnData == null)
                {
                    return new ExecutedResult<GradeVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<GradeVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeService (GetGradeByName)=====>{ex}");
                return new ExecutedResult<GradeVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

    }
}
