using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class ResignationInterviewService : IResignationInterviewService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationInterviewService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationInterviewRepository _resignationInterviewRepository;

        public ResignationInterviewService(IConfiguration config, IResignationInterviewRepository resignationInterviewRepository, ILogger<ResignationInterviewService> logger, IAccountRepository accountRepository)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationInterviewRepository = resignationInterviewRepository;
        }



        public async Task<BaseResponse> SubmitResignationInterview(RequesterInfo requesterInfo, ResignationInterviewVM payload)
        {
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var uploadPath = _config["Resignation:UploadFolderPath"];
                var errorMessages = String.Empty;
                StringBuilder errorBuilder = new StringBuilder();

                if (payload.LastDayOfWork < DateTime.Now)
                    errorMessages = errorMessages + "|Invalid Last Day of work";
                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                    errorMessages = errorMessages + "|Resignation reason is required";
                if (string.IsNullOrWhiteSpace(payload.QuestionOne))
                    errorMessages = errorMessages + "|What did you like most about Xpress Payments and your job? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.QuestionTwo))
                    errorMessages = errorMessages + "|What did you least like about Xpress Payments and your job? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.QuestionThree))
                    errorMessages = errorMessages + "|Do you feel you were placed in a position compatible with your skillset? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.QuestionFour))
                    errorMessages = errorMessages + "|If you are taking another job, what kind of work will you be taking? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.QuestionFive))
                    errorMessages = errorMessages + "|Could Xpress Payments have made any improvements that might have influenced you to stay on the job? Must be filled";

                if (errorMessages.Length > 0)
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };

                if (payload.SectionOne.Count < ApplicationConstant.totalFormCountSectionOne)
                {
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Please kindly check all radio button on section one" };
                }

                if (payload.SectionTwo.Count < ApplicationConstant.totalFormCountSectionTwo)
                {
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Please kindly check all radio button on section two" };
                }

                payload?.SectionOne?.ForEach(x =>
                {
                    if (x.Value < 1)
                    {
                        errorBuilder.Append($"| row {x.ID} has not be ckecked");
                    }
                });

                for (int x = 0; payload.SectionOne.Count > x; x++)
                {
                    if (payload.SectionOne[x].Value < 1)
                        return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Please kindly check all radio button on section one" };

                    payload.SectionOne[x].Scale = Enum.GetName(typeof(ExitInterviewScaleSectionOne), (ExitInterviewScaleSectionOne)payload.SectionOne[x].Value);
                }


                for (int x = 0; payload.SectionTwo.Count > x; x++)
                {
                    if (payload.SectionTwo[x].Value < 1)
                        return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Please kindly check all radio button on section two" };

                    payload.SectionTwo[x].Scale = Enum.GetName(typeof(ExitInterviewScaleSectionTwo), (ExitInterviewScaleSectionOne)payload.SectionTwo[x].Value);
                }

                var sectionOneDataTable = DatatableUtilities.ConvertSectionListToDataTable(payload.SectionOne);
                var sectionTwoDataTable = DatatableUtilities.ConvertSectionListToDataTable(payload.SectionTwo);


                payload.UserID = requesterInfo.UserId;

                var resignationInterview = new ResignationInterviewDTO
                {
                    Date = payload.Date,
                    DateCreated = DateTime.Now,
                    Created_By_User_Email = requesterInfo?.Username,
                    LastDayOfWork = payload.LastDayOfWork,
                    UserID = payload.UserID,
                    SRFID = payload.SRFID,
                    ReasonForResignation = payload.ReasonForResignation,
                    QuestionOne = payload.QuestionOne,
                    QuestionTwo = payload.QuestionTwo,
                    QuestionThree = payload.QuestionThree,
                    QuestionFour = payload.QuestionFour,
                    QuestionFive = payload.QuestionFive,
                    Comment = payload.Comment
                };


                var resp = await _resignationInterviewRepository.CreateResignationInterview(resignationInterview, sectionOneDataTable, sectionTwoDataTable);

                if (resp < 1)
                {
                    switch (resp)
                    {
                        case -1:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Interview Record already exist" };


                        default:
                            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                    }
                }

                return new BaseResponse { ResponseCode = ((int)ResponseCode.Ok).ToString(), ResponseMessage = "Resignation submitted successfully", Data = resignationInterview };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation Interview", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }

        public async Task<BaseResponse> GetInterviewScaleDetails(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                //string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                var InterviewScaleDetails = await _resignationInterviewRepository.GetInterviewScaleDetails();

                if (InterviewScaleDetails == null || InterviewScaleDetails.Count == 0)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "InterviewScaleDetails not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = InterviewScaleDetails;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Resignation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetInterviewScaleDetails ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetInterviewScaleDetails ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetResignationInterview(long SRFID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                //string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                var resignation = await _resignationInterviewRepository.GetResignationInterview(SRFID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "resignationInterview not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "resignationInterview fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationInterview(long SRFID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationInterview(long SRFID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetResignationInterviewDetails(long InterviewID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                //string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                var resignation = await _resignationInterviewRepository.GetResignationInterviewDetails(InterviewID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Resignation Interview Details not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "ResignationInterviewDetails fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationInterviewDetails(long SRFID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetResignationInterviewDetails(long SRFID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApprovePendingResignationInterview(ApproveResignationInterviewDTO request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationInterviewRepository.GetResignationInterview(request.ID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}
                var ApprovedResignation = await _resignationInterviewRepository.ApprovePendingResignationInterview(request.userID, request.ID, request.isApproved);
                if (ApprovedResignation < 0)
                {
                    switch (ApprovedResignation)
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
                _logger.LogError($"Exception Occured: ApprovePendingResignationInterview ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ApprovePendingResignationInterview ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DisapprovePendingResignationInterview(DisapproveResignationInterviewDTO request, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();




                var resignation = await _resignationInterviewRepository.GetResignationInterview(request.ID);
                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Resignation Details Cannot be Found.";
                    return response;
                }

                var DisapprovedResignation = await _resignationInterviewRepository.DisapprovePendingResignationInterview(request.userID, request.ID, request.IsDisapproved, request.DisapprovedComment);
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
                response.ResponseMessage = "Disapproved successfully.";
                return response;
            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DisapprovePendingResignationInterview ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DisapprovePendingResignationInterview ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllApprovedResignationInterview(long UserID, bool isApproved, RequesterInfo requester)
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

                var Resignation = await _resignationInterviewRepository.GetAllApprovedResignationInterview(UserID, isApproved);

                if (Resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Approved Resignation Interview not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Resignation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Resignation Interview fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllApprovedResignationInterview(long UserID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllApprovedResignationInterview(long UserID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }
    }
}

