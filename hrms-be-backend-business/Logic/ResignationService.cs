using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace hrms_be_backend_business.Logic
{
    public class ResignationService : IResignationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationRepository _resignationRepository;

        public ResignationService(IConfiguration config, IResignationRepository resignationRepository, ILogger<ResignationService> logger, IAccountRepository accountRepository)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationRepository = resignationRepository;
        }
        public async Task<BaseResponse> SubmitResignation(RequesterInfo requesterInfo, ResignationRequestVM payload)
        {
            //string fileName;
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var uploadPath = _config["Resignation:UploadFolderPath"];
                var errorMessages = String.Empty;

                if (payload.CompanyID <= 0)
                    errorMessages = errorMessages + "|Company ID is Required";
                if (payload.LastDayOfWork < DateTime.Now)
                    errorMessages = errorMessages + "|Invalid Last Day of work";
                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                    errorMessages = errorMessages + "|Resignation reason is required";
                if (string.IsNullOrEmpty(payload.fileName))
                    errorMessages = errorMessages + "|Resignation letter is required";

                if (errorMessages.Length > 0)
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };


                payload.UserId = requesterInfo.UserId;

                var resignation = new ResignationDTO
                {
                    Date = payload.Date,
                    CompanyID = payload.CompanyID,
                    DateCreated = DateTime.Now,
                    IsDeleted = false,
                    Created_By_User_Email = requesterInfo?.Username,
                    IsDisapproved = false,
                    LastDayOfWork = payload.LastDayOfWork,
                    UserId = payload.UserId,
                    ReasonForResignation = payload.ReasonForResignation,
                    SignedResignationLetter = payload.fileName
                };


                var resp = await _resignationRepository.CreateResignation(resignation);

                //Write a Switch case for the -1 -2
                if (resp < 0)
                {
                    switch (resp)
                    {
                        case -1:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "User Not Found" };

                        case -2:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "User Doesn't have a unit head" };

                        case -3:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "User Deosn't have an HOD" };

                        case -4:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "HR USer Not Found" };
                        case -5:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "User record already exist" };

                        default:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };

                    }
                }


                return new BaseResponse { ResponseCode = ((int)ResponseCode.Ok).ToString(), ResponseMessage = "Resignation submitted successfully", Data = resignation };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }

        public async Task<BaseResponse> UploadLetter(IFormFile signedResignationLetter)
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
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };


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
                _logger.LogError("Error Submitting resignation", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }

        public async Task<BaseResponse> GetResignationByID(long ID, RequesterInfo requester)
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

                var resignation = await _resignationRepository.GetResignationByID(ID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "resignation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "resignation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetResignationByID(long ResignationID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationByID(long ResignationID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetResignationByUserID(long UserID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var resignation = await _resignationRepository.GetResignationByUserID(UserID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "resignation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "resignation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByUserID(long UserID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationByUserID(long UserID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetResignationByCompanyID(long companyID, bool isApproved, RequesterInfo requester)
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

                if (requesterInfo == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                var Resignation = await _resignationRepository.GetResignationByCompanyID(companyID, isApproved);

                if (Resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Resignation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Resignation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByCompanyID(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationByCompanyID(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> DeleteResignation(DeleteResignationDTO request, RequesterInfo requester)
        {
            ////BaseResponse response = new BaseResponse();
            //var resignation = await _resignationRepository.GetResignationByID(ID);
            //if (resignation == null)
            //{
            //    throw new NotFoundException("Resignation not found");
            //}
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationRepository.GetResignationByID(request.ID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }

                var resign = await _resignationRepository.DeleteResignation(request.ID, requester.Username, request.Reason);

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Resignation deleted successfully.";
                return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteResignation ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetPendingResignationByUserID(RequesterInfo requester, long userID)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var PendingResignation = await _resignationRepository.GetPendingResignationByUserID(userID);

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
                _logger.LogError($"Exception Occured: GetPendingResignationByUserID(long userID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetPendingResignationByUserID(long userID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApprovePendingResignation(ApprovePendingResignationDTO request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationRepository.GetResignationByID(request.SRFID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }
                var approvedResignationResp = await _resignationRepository.ApprovePendingResignation(request.userID, request.SRFID);

                if (approvedResignationResp < 0)
                {
                    switch (approvedResignationResp)
                    {
                        case -1:
                            response.ResponseMessage = "Resignation not found";
                            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                            break;
                        case -2:
                            response.ResponseMessage = "You don't have access to approve this Resignation";
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
                _logger.LogError($"Exception Occured: ApprovePendingResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ApprovePendingResignation ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DisapprovePendingResignation(DisapprovePendingResignation request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationRepository.GetResignationByID(request.SRFID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }
                var DisapprovedResignation = await _resignationRepository.DisapprovePendingResignation(request.userID, request.SRFID, request.reason);

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
                            response.ResponseMessage = "Record already disapproved";
                            response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
                            break;
                        case -4:
                            response.ResponseMessage = "Record cannot be disapproved";
                            response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
                            break;
                        default:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                    }

                    return response;
                }

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Disapproved Resignation.";
                return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapprovePendingResignation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DisapprovePendingResignation ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UpdateResignation(UpdateResignationDTO updateDTO, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string roleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var resignation = await _resignationRepository.GetResignationByID(updateDTO.SRFID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }

                if (resignation.ApprovalStatus == (int)ResignationApprovalStageEnum.PendingOnUnitHead)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation details can't be updated.";
                    return response;
                }

                if (resignation.IsApproved)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation has Already Been Approved.";
                    return response;
                }


                resignation.SignedResignationLetter = updateDTO.SignedResignationLetter;
                resignation.ReasonForResignation = updateDTO.ReasonForResignation;
                resignation.LastDayOfWork = updateDTO.LastDayOfWork;

                var resp = await _resignationRepository.UpdateResignation(resignation);

                _logger.LogInformation("Resignation updated successfully.");
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Resignation updated successfully.";
                response.Data = updateDTO;
                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: UpdateResignationDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occurred: UpdateResignationDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


    }
}
