using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Dynamic;
using System.Net;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hrms_be_backend_business.Logic
{
    public class ResignationService : IResignationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationRepository _resignationRepository;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;

        public ResignationService(IConfiguration config, IResignationRepository resignationRepository, ILogger<ResignationService> logger, IAccountRepository accountRepository, ILeaveRequestRepository leaveRequestRepository, IMailService mailService, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationRepository = resignationRepository;
            _authService = authService;
            _mailService = mailService;

        }
       
        public async Task<ExecutedResult<string>> SubmitResignation( ResignationRequestVM payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            bool isModelStateValidate = true;
            string validationMessage = "";

            //string fileName;
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var uploadPath = _config["Resignation:UploadFolderPath"];


                if (payload.CompanyID <= 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  CompanyId is required";
                }
                    

                if (payload.LastDayOfWork < DateTime.Now)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Invalid last day of work";
                }
                if (payload.ResumptionDate > DateTime.Now)
                {
                    isModelStateValidate = false;
                    validationMessage += "  Invalid resumption date";
                } 
                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Resignation reason is required";
                }

                if (string.IsNullOrEmpty(payload.fileName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Resignation letter is required";
                }

                if (!isModelStateValidate)
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                payload.EmployeeId =  accessUser.data.EmployeeId;

                //var alreadyResigned = await _resignationRepository.GetResignationByEmployeeID(payload.EmployeeId);
                //if (alreadyResigned != null )
                //{
                //    if (alreadyResigned.IsUnitHeadDisapproved == false && alreadyResigned.IsHodDisapproved == false && alreadyResigned.IsHrDisapproved == false)
                //    {
                //        return new ExecutedResult<string>() { responseMessage = $"Resignation form has previously been submitted by this user", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                //    }
                    

                //}

                var resignation = new ResignationDTO
                {
                    ExitDate = payload.Date,
                    CompanyID = payload.CompanyID,
                    //StaffName = payload.StaffName,
                    DateCreated = DateTime.Now,
                    CreatedByUserId = accessUser.data.UserId,
                    ResumptionDate  = payload.ResumptionDate,
                    LastDayOfWork = payload.LastDayOfWork,
                    EmployeeId = payload.EmployeeId,
                    ReasonForResignation = payload.ReasonForResignation,
                    SignedResignationLetter = payload.fileName,
                    //StaffID = payload.StaffId
                };

                dynamic resp = new ExpandoObject();
                resp.ResignationID = 0;
                resp.ReturnVal = string.Empty;
                var res = await _resignationRepository.CreateResignation(resignation);
                var resp1 = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(res)); 
                int resignationID = Convert.ToInt16(resp1.ResignationID);
                string returnVal = Convert.ToString(resp1.ReturnVal);
                if (resignationID < 0)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{returnVal}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }
                var submittedresignation = await _resignationRepository.GetResignationByID(resignationID);

                //Send mail to Hod/UnitHead
                if (submittedresignation.UnitHeadEmployeeID <= 0)
                {
                    _mailService.SendResignationApproveMailToApprover(submittedresignation.HodEmployeeID, submittedresignation.EmployeeId, submittedresignation.ExitDate);
                }
                else
                {
                    _mailService.SendResignationApproveMailToApprover(submittedresignation.UnitHeadEmployeeID, submittedresignation.EmployeeId, submittedresignation.ExitDate);

                }

                return new ExecutedResult<string>() { responseMessage = "Resignation submitted Successfully", responseCode = 00.ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> UploadLetter(IFormFile signedResignationLetter, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            try
            {
                var uploadPath = _config["ResignationFileConfig:UploadFolderPath"];
                var uploadBaseURL = _config["ResignationFileConfig:FileUploadBaseURL"];
                var errorMessages = string.Empty;

                if (signedResignationLetter == null || signedResignationLetter.Length == 0)
                    errorMessages += "|Resignation letter is required";

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                if (string.IsNullOrEmpty(errorMessages))
                {
                    using HttpClient httpClient = new HttpClient();
                    FileUploadRequest request = new FileUploadRequest
                    {
                        AppName = "HRMS",
                        UserId = accessUser.data.EmployeeId.ToString(),
                        Image = signedResignationLetter
                    };
                    MultipartFormDataContent formDataContent = new MultipartFormDataContent();
                    formDataContent.Add(new StreamContent(request.Image.OpenReadStream()), "Image", request.Image.FileName);
                    formDataContent.Add(new StringContent(request.AppName), "AppName");
                    formDataContent.Add(new StringContent(request.UserId), "UserId");
                    string url = uploadBaseURL + "UploadFile";

                    HttpResponseMessage response = await httpClient.PostAsync(url, formDataContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        await response.Content.ReadAsStringAsync();
                        return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                    }
                    var uploadResponse = JsonConvert.DeserializeObject<FileResponse>(await response.Content.ReadAsStringAsync());
                    if (uploadResponse.ResponseCode != "00")
                    {
                        _logger.LogInformation("file upload failed");
                        return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                    }
                    return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = 00.ToString(), data = uploadResponse.UploadUrl };
                }
                else
                {
                    return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occurred", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> UpdateResignation(UpdateResignationDTO updateDTO, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }

            try
            {
                var resignation = await _resignationRepository.GetResignationByID(updateDTO.ResignationID);

                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                var repoResponse = await _resignationRepository.UpdateResignation(updateDTO);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Resignation updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "Resignation updated successfully.", responseCode = 00.ToString(), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: UpdateResignationDTO ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ResignationDTO>> GetResignationByID(long ID, string AccessKey, string RemoteIpAddress)
        {

            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var resignation = await _resignationRepository.GetResignationByID(ID);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetResignationByID(long ResignationID) ==> {ex.Message}");
                return new ExecutedResult<ResignationDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ResignationDTO>> GetResignationByEmployeeID(long EmployeeId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var resignation = await _resignationRepository.GetResignationByEmployeeID(EmployeeId);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByUserID(long UserID) ==> {ex.Message}");
                return new ExecutedResult<ResignationDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetResignationByCompanyID(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            try
            {              

                var resignation = await _resignationRepository.GetResignationByCompanyID(companyID, filter.PageNumber, filter.PageSize, filter.SearchValue);

                if (resignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }


                _logger.LogInformation("Resignations fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByCompanyID(long companyId) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }

        }

        //public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetAllResignations( string AccessKey, string RemoteIpAddress)
        //{

        //    try
        //    {
        //        var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
        //        if (accessUser.data == null)
        //        {
        //            return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

        //        }

        //        var resignation = await _resignationRepository.GetAllResignations();

        //        if (resignation == null)
        //        {
        //            return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

        //        }

        //        //update action performed into audit log here

        //        _logger.LogInformation("Resignations fetched successfully.");
        //        return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetResignations() ==> {ex.Message}");
        //        return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

        //    }

        //}

        //public async Task<ExecutedResult<string>> DeleteResignation(DeleteResignationDTO request, string AccessKey, string RemoteIpAddress)
        //{
        //    var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
        //    if (accessUser.data == null)
        //    {
        //        return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

        //    }
        //    try
        //    {

        //        var resignation = await _resignationRepository.GetResignationByID(request.ID);
        //        if (resignation == null)
        //        {
        //            return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

        //        }

        //        var repoResponse = await _resignationRepository.DeleteResignation(request.ID, accessUser.data.OfficialMail, request.Reason);

        //        if (!repoResponse.Contains("Success"))
        //        {
        //            return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

        //        }

        //        _logger.LogInformation("Resignation deleted successfully.");
        //        return new ExecutedResult<string>() { responseMessage = "Resignation deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
        //    }


        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: DeleteResignation ==> {ex.Message}");
        //        return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

        //    }
        //}

        public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetPendingResignationByEmployeeID(long EmployeeId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var PendingResignation = await _resignationRepository.GetPendingResignationByEmployeeID(EmployeeId);

                if (PendingResignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Resignation fetched successfully.");
                return new  ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Resignation fetched Successfully", responseCode = (00).ToString(), data = PendingResignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingResignationByUserID(long userID) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetPendingResignationByCompanyID(long companyID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var PendingResignation = await _resignationRepository.GetPendingResignationByCompanyID(companyID);

                if (PendingResignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Resignation fetched Successfully", responseCode = (00).ToString(), data = PendingResignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingResignationByUserID(long userID) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> ApprovePendingResignation(ApprovePendingResignationDTO request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }

            try
            {
                var resignation = await _resignationRepository.GetResignationByID(request.ResignationId);
                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                var approvedResignationResp = await _resignationRepository.ApprovePendingResignation(request.EmployeeID, request.ResignationId);

                if (accessUser.data.EmployeeId == resignation.HodEmployeeID)
                {
                    _mailService.SendResignationApproveMailToApprover(resignation.HrEmployeeID, resignation.EmployeeId, resignation.ExitDate);

                }
                else
                {
                    _mailService.SendResignationApproveMailToApprover(resignation.HodEmployeeID, resignation.EmployeeId, resignation.ExitDate);

                }

                if (!approvedResignationResp.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{approvedResignationResp}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                _mailService.SendResignationApproveConfirmationMail(resignation.EmployeeId, accessUser.data.EmployeeId, resignation.ExitDate);

                return new ExecutedResult<string>() { responseMessage = "Resignation approved successfully.", responseCode = (00).ToString(), data = null };

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApprovePendingResignation ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DisapprovePendingResignation(DisapprovePendingResignation request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var resignation = await _resignationRepository.GetResignationByID(request.ResignationID);
                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                var DisapprovedResignation = await _resignationRepository.DisapprovePendingResignation(request.EmployeeID, request.ResignationID, request.reason);

                if (!DisapprovedResignation.Contains("Success"))
                {

                    return new ExecutedResult<string>() { responseMessage = $"{DisapprovedResignation}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                _mailService.SendResignationDisapproveConfirmationMail(resignation.EmployeeId, request.EmployeeID);

                return new ExecutedResult<string>() { responseMessage = "Resignation disapproved successfully.", responseCode = (00).ToString(), data = null };

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapprovePendingResignation ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

    }
}
