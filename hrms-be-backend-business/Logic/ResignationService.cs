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

                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Resignation reason is required";
                }
                if (string.IsNullOrWhiteSpace(payload.StaffName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Staff name is required";
                }

                if (string.IsNullOrEmpty(payload.fileName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  Resignation letter is required";
                }

                if (!isModelStateValidate)
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };


                payload.EmployeeId = accessUser.data.EmployeeId;

                var resignation = new ResignationDTO
                {
                    StaffName = payload.StaffName,
                    ExitDate = payload.Date,
                    CompanyID = payload.CompanyID,
                    DateCreated = DateTime.Now,
                    CreatedByUserId = accessUser.data.EmployeeId,
                    LastDayOfWork = payload.LastDayOfWork,
                    EmployeeId = payload.EmployeeId,
                    ReasonForResignation = payload.ReasonForResignation,
                    SignedResignationLetter = payload.fileName
                };


                var resp = await _resignationRepository.CreateResignation(resignation);
                if (resp < 0)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{resp}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                var submittedresignation = _resignationRepository.GetResignationByID(resp);

                //Send mail to Hod/UnitHead
                if(submittedresignation.UnitHeadEmployeeID == null)
                {
                    _mailService.SendResignationApproveMailToApprover(submittedresignation.HodEmployeeID, submittedresignation.EmployeeId, submittedresignation.ExitDate);
                }
                else
                {
                    _mailService.SendResignationApproveMailToApprover(submittedresignation.UnitHeadEmployeeID, submittedresignation.EmployeeId, submittedresignation.ExitDate);

                }

                return new ExecutedResult<string>() { responseMessage = "Resignation submitted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> UploadLetter(IFormFile signedResignationLetter, string AccessKey, string RemoteIpAddress)
        {
            string fileName;
            try
            {

                var uploadPath = _config["Resignation:UploadFolderPath"];
                var uploadBaseURL = _config["Resignation:FileUploadBaseURL"];
                var errorMessages = String.Empty;

                if (signedResignationLetter.Length < 0)
                    errorMessages = errorMessages + "|Resignation letter is required";

                if (errorMessages.Length > 0)
                    return new ExecutedResult<string>() { responseMessage = $"{errorMessages}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };


                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (signedResignationLetter.Length > 0)
                {
                    var fileExt = System.IO.Path.GetExtension(signedResignationLetter.FileName).Substring(1);
                    fileName = Guid.NewGuid().ToString();//Path.GetFileName(formFile.FileName);
                    fileName = fileName.Replace(" ", "");
                    string filePath = Path.Combine(uploadPath, fileName.Trim() + "." + fileExt);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (Stream stream = System.IO.File.Create(filePath))
                    {
                        await signedResignationLetter.CopyToAsync(stream);
                        var data = new
                        {
                            fileName = $"{fileName}.{fileExt}",
                            fileURL = $"{uploadBaseURL}{fileName}.{fileExt}",
                            filePath = filePath,

                        };
                        return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

                    }
                }
                else
                {
                    return new ExecutedResult<string>() { responseMessage = "Invalid file", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<string>> UpdateResignation(UpdateResignationDTO updateDTO, string AccessKey, string RemoteIpAddress)
        {
     
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
                return new ExecutedResult<string>() { responseMessage = "Resignation updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

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

                //update action performed into audit log here

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetResignationByID(long ResignationID) ==> {ex.Message}");
                return new ExecutedResult<ResignationDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ResignationDTO>> GetResignationByUserID(long UserID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var resignation = await _resignationRepository.GetResignationByUserID(UserID);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<ResignationDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByUserID(long UserID) ==> {ex.Message}");
                return new ExecutedResult<ResignationDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetResignationByCompanyID(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var resignation = await _resignationRepository.GetResignationByCompanyID(companyID, filter.PageNumber, filter.PageSize, filter.SearchValue);

                if (resignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignations fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


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

        public async Task<ExecutedResult<IEnumerable<ResignationDTO>>> GetPendingResignationByUserID(long userID, string AccessKey, string RemoteIpAddress)
        {

            try
            {

                var PendingResignation = await _resignationRepository.GetPendingResignationByUserID(userID);

                if (PendingResignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignation fetched successfully.");
                return new  ExecutedResult<IEnumerable<ResignationDTO>>() { responseMessage = "Resignation fetched Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = PendingResignation };


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
                var approvedResignationResp = await _resignationRepository.ApprovePendingResignation(request.userID, request.ResignationId);

                if (accessUser.data.EmployeeId == resignation.HodEmployeeID || accessUser.data.EmployeeId == resignation.UnitHeadEmployeeID)
                {
                    _mailService.SendResignationApproveMailToApprover(resignation.HrEmployeeID, resignation.EmployeeId, resignation.ExitDate);

                }
                else
                {
                    _mailService.SendResignationApproveMailToApprover(resignation.HodEmployeeID, resignation.EmployeeId, resignation.ExitDate);

                }

                if (!approvedResignationResp.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{resignation}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }


                return new ExecutedResult<string>() { responseMessage = "Resignation approved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApprovePendingResignation ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DisapprovePendingResignation(DisapprovePendingResignation request, string AccessKey, string RemoteIpAddress)
        {
            try
            {
                var resignation = await _resignationRepository.GetResignationByID(request.ResignationID);
                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                var DisapprovedResignation = await _resignationRepository.DisapprovePendingResignation(request.userID, request.ResignationID, request.reason);

                if (!DisapprovedResignation.Contains("Success"))
                {

                    return new ExecutedResult<string>() { responseMessage = $"{DisapprovedResignation}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                return new ExecutedResult<string>() { responseMessage = "Resignation disapproved successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapprovePendingResignation ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

       


    }
}
