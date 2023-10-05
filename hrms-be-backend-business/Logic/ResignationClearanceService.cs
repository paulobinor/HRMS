using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace hrms_be_backend_business.Logic
{
    public class ResignationClearanceService : IResignationClearanceService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationClearanceService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationClearanceRepository _resignationClearanceRepository;

        public ResignationClearanceService(IConfiguration config, IResignationClearanceRepository resignationClearanceRepository, ILogger<ResignationClearanceService> logger, IAccountRepository accountRepository)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationClearanceRepository = resignationClearanceRepository;
        }


        public async Task<BaseResponse> SubmitResignationClearance(RequesterInfo requesterInfo, ResignationClearanceVM payload)
        {
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var errorMessages = String.Empty;
                StringBuilder errorBuilder = new StringBuilder();

                if (payload.LastDayOfWork > DateTime.Now)
                    errorMessages = errorMessages + "|Invalid Last Day of work";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToAdmin))
                    errorMessages = errorMessages + "|Items returned is required";
                if (string.IsNullOrEmpty(payload.FileName))
                    errorMessages = errorMessages + "|Resignation letter is required";


                if (errorMessages.Length > 0)
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };




                payload.UserID = requesterInfo.UserId;

                var resignationClearance = new ResignationClearanceDTO
                {
                    UserID = payload.UserID,
                    SRFID = payload.SRFID,
                    InterviewID = payload.InterviewID,
                    ItemsReturnedToDepartment = payload.ItemsReturnedToDepartment,
                    ItemsReturnedToAdmin = payload.ItemsReturnedToAdmin,
                    Created_By_User_Email = requesterInfo?.Username,
                    Loans = payload.Loans,
                    ItemsReturnedToHR = payload.ItemsReturnedToHR,
                    Collateral = payload.Collateral,
                    LastDayOfWork = payload.LastDayOfWork,
                    IsDeleted = false,
                    IsDisapproved = false

                };


                var resp = await _resignationClearanceRepository.CreateResignationClearance(resignationClearance);

                //if (resp < 1)
                //{
                //    switch (resp)
                //    {
                //        case -1:
                //            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Interview Record already exist" };


                //        default:
                //            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                //    }
                //}

                return new BaseResponse { ResponseCode = ((int)ResponseCode.Ok).ToString(), ResponseMessage = "Resignation Clearance submitted successfully", Data = resignationClearance };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation Clearance", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }

        public async Task<BaseResponse> UploadItemsReturnedToDepartmant(IFormFile ItemsReturnedToDepartmant)
        {
            string fileName;
            try
            {

                var uploadPath = _config["Resignation:UploadFolderPath"];
                var uploadBaseURL = _config["Resignation:FileUploadBaseURL"];
                var errorMessages = String.Empty;

                if (ItemsReturnedToDepartmant.Length < 0)
                    errorMessages = errorMessages + "|Resignation letter is required";

                if (errorMessages.Length > 0)
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };


                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (ItemsReturnedToDepartmant.Length > 0)
                {
                    var fileExt = System.IO.Path.GetExtension(ItemsReturnedToDepartmant.FileName).Substring(1);
                    fileName = Guid.NewGuid().ToString();//Path.GetFileName(formFile.FileName);
                    fileName = fileName.Replace(" ", "");
                    string filePath = Path.Combine(uploadPath, fileName.Trim() + "." + fileExt);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (Stream stream = System.IO.File.Create(filePath))
                    {
                        await ItemsReturnedToDepartmant.CopyToAsync(stream);
                        var data = new
                        {
                            fileName = $"{fileName}.{fileExt}",
                            fileURL = $"{uploadBaseURL}{fileName}.{fileExt}",
                            filePath = filePath,

                        };
                        return new BaseResponse
                        {
                            ResponseCode = ((int)ResponseCode.Ok).ToString(),
                            ResponseMessage = "File Uploaded Successfully",
                            Data = data
                        };
                    }
                }
                else
                {
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "Invalid file." };

                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Error Uploading Items Returned To Departmant", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }

        public async Task<BaseResponse> GetResignationClearanceByID(long ID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(ID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "resignationClearance not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "resignation Clearance fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetResignationClearanceByID(long ID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationClearanceByID(long ID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetResignationClearanceByUserID(long UserID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var resignation = await _resignationClearanceRepository.GetResignationClearanceByUserID(UserID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "resignation Clearance not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "resignation clearance fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationClearanceByUserID(long UserID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationClearanceByUserID(long UserID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetPendingResignationClearanceByUserID(RequesterInfo requester, long userID)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var PendingResignation = await _resignationClearanceRepository.GetPendingResignationClearanceByUserID(userID);

                if (PendingResignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "PendingResignation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = PendingResignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "PendingResignation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingResignationClearanceByUserID(long userID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetPendingResignationClearanceByUserID(long userID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }
                var approvedResignationResp = await _resignationClearanceRepository.ApprovePendingResignationClearance(request.userID, request.ID);

                if (approvedResignationResp < 0)
                {
                    switch (approvedResignationResp)
                    {
                        case -1:
                            response.ResponseMessage = "Resignation not found";
                            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                            break;
                        case -2:
                            response.ResponseMessage = "UnAthorized";
                            response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                            break;
                        case -3:
                            response.ResponseMessage = "Already Approved";
                            response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                            break;
                        default:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                    }
                    return response;
                }


                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Approved successfully.";
                return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }
                var DisapprovedResignation = await _resignationClearanceRepository.DisapprovePendingResignationClearance(request.userID, request.ID, request.reason);

                if (DisapprovedResignation < 0)
                {
                    switch (DisapprovedResignation)
                    {
                        case -1:
                            response.ResponseMessage = "Resignation not found";
                            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                            break;
                        case -2:
                            response.ResponseMessage = "UnAuthorize";
                            response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                            break;
                        case -3:
                            response.ResponseMessage = "Invalid Approval Status";
                            response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
                            break;
                        default:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                    }

                    return response;
                }

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Disapproved Resignation Clearance.";
                return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapprovePendingResignationClearance ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DisapprovePendingResignationClearance ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
