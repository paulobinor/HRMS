using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace hrms_be_backend_business.Logic
{
    public class ExitClearanceSetupService : IExitClearanceSetupService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ExitClearanceSetupService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IExitClearanceSetupRepository _exitClearanceSetupRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;

        public ExitClearanceSetupService(IConfiguration config, IExitClearanceSetupRepository exitClearanceSetupRepository, ILogger<ExitClearanceSetupService> logger, IAccountRepository accountRepository, IMailService mailService, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _exitClearanceSetupRepository = exitClearanceSetupRepository;
            _authService = authService;
            _mailService = mailService;

        }
        public async Task<ExecutedResult<string>> CreateExitClearanceSetup(CreateExitClearanceSetupVm request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            bool isModelStateValidate = true;
            string validationMessage = "";

            try
            {


                if (request.CompanyID <= 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  CompanyId is required";
                }

                if (!isModelStateValidate)
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                var setup = new ExitClearanceSetupDTO
                {
                    CompanyID = request.CompanyID,
                    DateCreated = DateTime.Now,
                    CreatedByUserId = accessUser.data.EmployeeId,
                    IsFinalApproval = request.IsFinalApproval,
                    DepartmentID = request.DepartmentID,
                };

                var resp = await _exitClearanceSetupRepository.CreateExitClearanceSetup(setup);
                if (!resp.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{resp}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                return new ExecutedResult<string>() { responseMessage = "setup created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating setup", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> DeleteExitClearanceSetup(ExitClearanceSetupDTO request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var setup = await _exitClearanceSetupRepository.GetExitClearanceSetupByID(request.ExitClearanceSetupID);
                if (setup == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                var repoResponse = await _exitClearanceSetupRepository.DeleteExitClearanceSetup(request.ExitClearanceSetupID, accessUser.data.OfficialMail);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("setup deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Setup deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteResignation ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }


        public async Task<ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>> GetExitClearanceSetupByCompanyID(long companyID, string AccessKey, string RemoteIpAddress)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var setup = await _exitClearanceSetupRepository.GetAllExitClearanceSetupByCompanyID(companyID);

                if (setup == null)
                {
                    return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("setup fetched successfully.");
                return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = setup };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetExitClearanceSetupByCompanyID(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ExitClearanceSetupDTO>> GetExitClearanceSetupByID(long exitClearanceSetupID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var setup = await _exitClearanceSetupRepository.GetExitClearanceSetupByID(exitClearanceSetupID);

                if (setup == null)
                {
                    return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("setup fetched successfully.");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = setup };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetExitClearanceSetupByID(long exitClearanceSetupID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> UpdateExitClearanceSetup(ExitClearanceSetupDTO updateDTO, string AccessKey, string RemoteIpAddress)
        {
            try
            {
                var setup = await _exitClearanceSetupRepository.GetExitClearanceSetupByID(updateDTO.ExitClearanceSetupID);

                if (setup == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                var repoResponse = await _exitClearanceSetupRepository.UpdateExitClearanceSetup(updateDTO);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("setup updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "setup updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: ExitClearanceSetupDTO ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>> GetDepartmentsThatAreNotFinalApproval(long companyID, string AccessKey, string RemoteIpAddress)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var departments = await _exitClearanceSetupRepository.GetDepartmentsThatAreNotFinalApproval(companyID);

                if (departments == null)
                {
                    return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("departments fetched successfully.");
                return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = departments };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetDepartmentsThatAreNotFinalApproval( long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ExitClearanceSetupDTO>> GetDepartmentThatIsFinalApprroval(long companyID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var department = await _exitClearanceSetupRepository.GetDepartmentThatIsFinalApprroval(companyID);

                if (department == null)
                {
                    return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("department fetched successfully.");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = department };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetDepartmentThatIsFinalApprroval(long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ExitClearanceSetupDTO>> GetExitClearanceSetupByHodEmployeeID(long HodEmployeeID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var setup = await _exitClearanceSetupRepository.GetExitClearanceSetupByHodEmployeeID(HodEmployeeID);

                if (setup == null)
                {
                    return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("setup fetched successfully.");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = setup };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetExitClearanceSetupByID(long exitClearanceSetupID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ExitClearanceSetupDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }
    }
}
