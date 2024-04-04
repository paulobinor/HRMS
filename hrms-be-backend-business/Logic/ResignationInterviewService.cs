using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class ResignationInterviewService : IResignationInterviewService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationInterviewService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationInterviewRepository _resignationInterviewRepository;
        private readonly IAuthService _authService;


        public ResignationInterviewService(IConfiguration config, IResignationInterviewRepository resignationInterviewRepository, ILogger<ResignationInterviewService> logger, IAccountRepository accountRepository, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationInterviewRepository = resignationInterviewRepository;
            _authService = authService;
        }



        public async Task<ExecutedResult<string>> SubmitResignationInterview(ResignationInterviewVM payload, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var uploadPath = _config["Resignation:UploadFolderPath"];
                var errorMessages = String.Empty;
                StringBuilder errorBuilder = new StringBuilder();

                if (payload.ExitDate < DateTime.Now)
                    errorMessages = errorMessages + "|Invalid Last Day of work";
                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                    errorMessages = errorMessages + "|Resignation reason is required";
                if (string.IsNullOrWhiteSpace(payload.WhatDidYouLikeMostAboutTheCompanyAndYourJob))
                    errorMessages = errorMessages + "|What did you like most about the company and your job? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.WhatDidYouLeastLikeAboutTheCompanyAndYourJob))
                    errorMessages = errorMessages + "|What did you least like about the company and your job? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.DoYouFeelYouWerePlacedInAPositionCompatibleWithYourSkillSet))
                    errorMessages = errorMessages + "|Do you feel you were placed in a position compatible with your skillset? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.IfYouAreTakingAnotherJob_WhatKindOfJobWillYouBeTaking))
                    errorMessages = errorMessages + "|If you are taking another job, what kind of work will you be taking? Must be filled";
                if (string.IsNullOrWhiteSpace(payload.CouldOurCompanyHaveMadeAnyImprovementsThatMightHaveMadeYouStay))
                    errorMessages = errorMessages + "|Could Xpress Payments have made any improvements that might have influenced you to stay on the job? Must be filled";

                if (errorMessages.Length > 0)
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                if (payload.SectionOne.Count < ApplicationConstant.totalFormCountSectionOne)
                {
                    return new ExecutedResult<string>() { responseMessage = "Please kindly check all radio button on section one", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
                }

                if (payload.SectionTwo.Count < ApplicationConstant.totalFormCountSectionTwo)
                {
                    return new ExecutedResult<string>() { responseMessage = "Please kindly check all radio button on section two", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
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
                        return new ExecutedResult<string>() { responseMessage = "Please kindly check all radio button on section one", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                    payload.SectionOne[x].Scale = Enum.GetName(typeof(ExitInterviewScaleSectionOne), (ExitInterviewScaleSectionOne)payload.SectionOne[x].Value);
                }


                for (int x = 0; payload.SectionTwo.Count > x; x++)
                {
                    if (payload.SectionTwo[x].Value < 1)
                        return new ExecutedResult<string>() { responseMessage = "Please kindly check all radio button on section two", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                    payload.SectionTwo[x].Scale = Enum.GetName(typeof(ExitInterviewScaleSectionTwo), (ExitInterviewScaleSectionOne)payload.SectionTwo[x].Value);
                }

                var sectionOneDataTable = DatatableUtilities.ConvertSectionListToDataTable(payload.SectionOne);
                var sectionTwoDataTable = DatatableUtilities.ConvertSectionListToDataTable(payload.SectionTwo);


                payload.EmployeeId = accessUser.data.EmployeeId;

                var resignationInterview = new ResignationInterviewDTO
                {
                    Date = payload.Date,
                    DateCreated = DateTime.Now,
                    CreatedByUserId = accessUser.data.UserId,
                    ExitDate = payload.ExitDate,
                    EmployeeId = payload.EmployeeId,
                    ResignationId = payload.ResignationId,
                    ReasonForResignation = payload.ReasonForResignation,
                    OtherRemarks = payload.OtherRemarks,
                    ResumptionDate = payload.ResumptionDate,
                    Signature = payload.Signature,
                    
                };


                var resp = await _resignationInterviewRepository.CreateResignationInterview(resignationInterview, sectionOneDataTable, sectionTwoDataTable);

                if (!resp.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                _logger.LogInformation("Resignation Interview form Submitted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Resignation Interview form Submitted successfully.", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation interview form", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<ResignationInterviewDTO>> GetResignationInterviewById(long ResignationInterviewId, string AccessKey, string RemoteIpAddress)
        {

            try
            {


                var resignation = await _resignationInterviewRepository.GetResignationInterviewById(ResignationInterviewId);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignation interview fetched successfully.");
                return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationInterviewById(long ResignationInterviewId) ==> {ex.Message}");
                return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }


       public async Task<ExecutedResult<IEnumerable<ResignationInterviewDTO>>> GetAllResignationInterviewsByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationInterviewDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                }

                var resignation = await _resignationInterviewRepository.GetAllResignationInterviewsByCompany(companyID, filter.PageNumber, filter.PageSize, filter.SearchValue);

                if (resignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationInterviewDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignations fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationInterviewDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllResignationInterviewsByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationInterviewDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }

        }

        public async Task<BaseResponse> GetResignationInterviewDetails(long InterviewID, string AccessKey, string RemoteIpAddress)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var resignation = await _resignationInterviewRepository.GetResignationInterviewDetails(InterviewID);

                if (resignation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Resignation Interview Details not found.";
                    response.Data = null;
                    return response;
                }

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

        public async Task<ExecutedResult<ResignationInterviewDTO>> GetResignationInterviewByEmployeeID(long EmployeeId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var resignation = await _resignationInterviewRepository.GetResignationInterviewByEmployeeID(EmployeeId);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationByUserID(long UserID) ==> {ex.Message}");
                return new ExecutedResult<ResignationInterviewDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }


        //public async Task<BaseResponse> GetInterviewScaleDetails(RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();
        //    try
        //    {
        //        //string requesterUserEmail = requester.Username;
        //        //string requesterUserId = requester.UserId.ToString();
        //        //string RoleId = requester.RoleId.ToString();

        //        //var ipAddress = requester.IpAddress.ToString();
        //        //var port = requester.Port.ToString();

        //        var InterviewScaleDetails = await _resignationInterviewRepository.GetInterviewScaleDetails();

        //        if (InterviewScaleDetails == null || InterviewScaleDetails.Count == 0)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "InterviewScaleDetails not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = InterviewScaleDetails;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Resignation fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetInterviewScaleDetails ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: GetInterviewScaleDetails ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}

        //public async Task<BaseResponse> ApprovePendingResignationInterview(ApproveResignationInterviewDTO request, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();




        //        var resignation = await _resignationInterviewRepository.GetResignationInterview(request.ID);
        //        if (resignation == null)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Resignation Details Cannot be Found.";
        //            return response;
        //        }

        //        //if (Convert.ToInt32(RoleId) != 2)
        //        //{
        //        //    if (Convert.ToInt32(RoleId) != 4)
        //        //    {
        //        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
        //        //        return response;

        //        //    }

        //        //}
        //        var ApprovedResignation = await _resignationInterviewRepository.ApprovePendingResignationInterview(request.EmployeeId, request.ID, request.isApproved);
        //        if (ApprovedResignation < 0)
        //        {
        //            switch (ApprovedResignation)
        //            {
        //                case -1:
        //                    response.ResponseMessage = "Resignation not found";
        //                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -2:
        //                    response.ResponseMessage = "You don't have access to approve this Resignation";
        //                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -3:
        //                    response.ResponseMessage = "Already Approved";
        //                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
        //                    break;
        //                default:
        //                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
        //            }
        //            return response;
        //        }

        //        response.Data = resignation;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Approved successfully.";
        //        return response;
        //    }


        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: ApprovePendingResignationInterview ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ApprovePendingResignationInterview ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}

        //public async Task<BaseResponse> DisapprovePendingResignationInterview(DisapproveResignationInterviewDTO request, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();




        //        var resignation = await _resignationInterviewRepository.GetResignationInterview(request.ID);
        //        if (resignation == null)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Resignation Details Cannot be Found.";
        //            return response;
        //        }

        //        var DisapprovedResignation = await _resignationInterviewRepository.DisapprovePendingResignationInterview(request.userID, request.ID, request.IsDisapproved, request.DisapprovedComment);
        //        if (DisapprovedResignation < 0)
        //        {
        //            switch (DisapprovedResignation)
        //            {
        //                case -1:
        //                    response.ResponseMessage = "Resignation not found";
        //                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -2:
        //                    response.ResponseMessage = "UnAuthorize";
        //                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -3:
        //                    response.ResponseMessage = "Record already disapproved";
        //                    response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -4:
        //                    response.ResponseMessage = "Record cannot be disapproved";
        //                    response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
        //                    break;
        //                default:
        //                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
        //            }

        //            return response;
        //        }


        //        response.Data = resignation;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Disapproved successfully.";
        //        return response;
        //    }


        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: DisapprovePendingResignationInterview ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: DisapprovePendingResignationInterview ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}


    }
}

