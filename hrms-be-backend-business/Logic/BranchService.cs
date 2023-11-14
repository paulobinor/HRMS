using ExcelDataReader;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class BranchService : IBranchService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<BranchService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IUriService _uriService;
        private readonly ILgaRepository _lgaRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;

        public BranchService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<BranchService> logger,
            IBranchRepository branchRepository, IAuditLog audit, IAuthService authService, IMailService mailService, IUriService uriService, IUserAppModulePrivilegeRepository privilegeRepository, ILgaRepository lgaRepository, IStateRepository stateRepository, ICountryRepository countryRepository)
        {
            _audit = audit;

            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _branchRepository = branchRepository;
            _authService = authService;
            _mailService = mailService;
            _uriService = uriService;
            _privilegeRepository = privilegeRepository;
            _lgaRepository = lgaRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
        }

        public async Task<ExecutedResult<string>> CreateBranch(CreateBranchDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BranchModulePrivilegeConstant.Create_Branch, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BranchName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Branch Name is required";
                }
                if (string.IsNullOrEmpty(payload.Address))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Address is required";
                }
                if (payload.LgaId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Local Gorvernment is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessBranchReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Address = payload.Address,
                    BranchName = payload.BranchName,
                    IsHeadQuater = payload.IsHeadQuater,
                    LgaId = payload.LgaId,
                    IsModifield = false,
                };
                string repoResponse = await _branchRepository.ProcessBranch(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateBranch",
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
                _logger.LogError($"BranchService (CreateBranch)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }



        public async Task<ExecutedResult<string>> CreateBranchBulkUpload(IFormFile payload, string AccessKey, IEnumerable<Claim> claim, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, requester.IpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

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

                        string BranchName = serviceDetails.Rows[0][0].ToString();
                        string Address = serviceDetails.Rows[0][1].ToString();
                        string Country = serviceDetails.Rows[0][2].ToString();
                        string State = serviceDetails.Rows[0][3].ToString();
                        string LgaName = serviceDetails.Rows[0][4].ToString();
                        string CompanyName = serviceDetails.Rows[0][5].ToString();


                        if (BranchName != "BranchName" || Address != "Address" || Country != "Country" || State != "State" || LgaName != "LgaName" || CompanyName != "CompanyName")
                            return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ValidationError).ToString(), responseMessage = "File header not in the Right format" };
                        else
                        {

                            var countryList = await _countryRepository.GetCountries();
                            var LgaList = await _lgaRepository.GetAllLgas();
                            string prevCountry = string.Empty;
                            List<StateVm> stateList = new List<StateVm>();
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {
                                int countryID = 0;
                                int stateID = 0;
                                int lgaID = 0;




                                string branchName = serviceDetails.Rows[row][0].ToString();
                                string address = serviceDetails.Rows[row][1].ToString();
                                string country = serviceDetails.Rows[row][2].ToString();
                                string state = serviceDetails.Rows[row][3].ToString();
                                string lgaName = serviceDetails.Rows[row][4].ToString();
                                string companyName = serviceDetails.Rows[row][5].ToString();

                                if (countryList.Count > 0)
                                {
                                    var selectedCountry = countryList.FirstOrDefault(m => m.CountryName == country.Trim());

                                    if (selectedCountry == null)
                                    {
                                        errorOutput.Append($"| Row {row} failed due to invalid Country {country}");
                                        continue;
                                    }
                                    countryID = Convert.ToInt32(selectedCountry.CountryID);
                                    prevCountry = country;
                                }

                                if (prevCountry != country || stateList.Count == 0)
                                    stateList = await _stateRepository.GetStates(countryID);

                                if (stateList.Count > 0)
                                {
                                    var selectedState = stateList.FirstOrDefault(m => m.StateName == state.Trim());
                                    if (selectedState == null)
                                    {
                                        errorOutput.Append($"| Row {row} failed due to invalid state {state}");
                                        continue;
                                    }

                                    stateID = selectedState.StateID;
                                }


                                var lga = LgaList.FirstOrDefault(m => m.StateID == stateID && m.LGA_Name == lgaName.Trim());
                                if (lga == null)
                                {

                                    errorOutput.Append($"| Row {row} failed due to invalid LGA {lgaName} in State {state}");
                                    continue;
                                }
                                var request = new CreateBranchDto
                                {
                                    Address = address,
                                    BranchName = branchName,
                                    LgaId = lga.LGAID

                                };

                                var resp = await CreateBranch(request, AccessKey, claim, requester.IpAddress, requester.Port);


                                if (resp.responseCode == "0")
                                {
                                    k++;
                                }
                                else
                                {
                                    errorOutput.Append($"| Row {row} failed due to {resp.responseMessage}");
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
                        return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ProcessingError).ToString(), responseMessage = errorOutput.ToString().TrimStart('|') };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.ToString()}");
                return new ExecutedResult<string> { responseCode = ((int)ResponseCode.ProcessingError).ToString(), responseMessage = "Exception occured creating branch" };
            }
        }

        public async Task<ExecutedResult<string>> UpdateBranch(UpdateBranchDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BranchModulePrivilegeConstant.Update_Branch, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BranchName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Branch Name is required";
                }
                if (string.IsNullOrEmpty(payload.Address))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Address is required";
                }
                if (payload.LgaId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Local Gorvernment is required";
                }
                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new ProcessBranchReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Address = payload.Address,
                    BranchName = payload.BranchName,
                    IsHeadQuater = payload.IsHeadQuater,
                    LgaId = payload.LgaId,
                    IsModifield = true,
                    BranchId = payload.BranchId,
                };
                string repoResponse = await _branchRepository.ProcessBranch(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateBranch",
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
                _logger.LogError($"BranchService (UpdateBranch)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteBranch(DeleteBranchDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BranchModulePrivilegeConstant.Delete_Branch, accessUser.data.UserId);
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
                var repoPayload = new DeleteBranchReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    Comment = payload.Comment,
                    BranchId = payload.BranchId,
                };
                string repoResponse = await _branchRepository.DeleteBranch(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteBranch",
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
                _logger.LogError($"BranchService (DeleteBranch)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<BranchVm>>> GetBranches(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BranchModulePrivilegeConstant.View_Branch, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _branchRepository.GetBranches(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<BranchVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchService (GetBranches)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<BranchVm>>> GetBranchesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(BranchModulePrivilegeConstant.View_Branch, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NoPrivilege).ToString(), checkPrivilege);

                }
                var returnData = await _branchRepository.GetBranchesDeleted(accessUser.data.CompanyId, filter.PageNumber, filter.PageSize);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.AuthorizationError.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<BranchVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchService (GetBranchesDeleted)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<BranchVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<BranchVm>> GetBranchById(long Id, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<BranchVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _branchRepository.GetBranchById(Id);
                if (returnData == null)
                {
                    return new ExecutedResult<BranchVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<BranchVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchService (GetBranchById)=====>{ex}");
                return new ExecutedResult<BranchVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<BranchVm>> GetBranchByName(string BranchName, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<BranchVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _branchRepository.GetBranchByName(BranchName, accessUser.data.CompanyId);
                if (returnData == null)
                {
                    return new ExecutedResult<BranchVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<BranchVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"BranchService (GetBranchByName)=====>{ex}");
                return new ExecutedResult<BranchVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
