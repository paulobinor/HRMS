using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
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
        private readonly IAuthService _authService;


        public ResignationClearanceService(IConfiguration config, IResignationClearanceRepository resignationClearanceRepository, ILogger<ResignationClearanceService> logger, IAccountRepository accountRepository, IAuthService authService)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationClearanceRepository = resignationClearanceRepository;
            _authService = authService;
        }


        public async Task<ExecutedResult<string>> SubmitResignationClearance(ResignationClearanceDTO payload, string AccessKey, string RemoteIpAddress)
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
                var errorMessages = String.Empty;
                StringBuilder errorBuilder = new StringBuilder();

                if (payload.ExitDate > DateTime.Now)
                    errorMessages = errorMessages + "|Invalid exit date";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToAdmin))
                    errorMessages = errorMessages + "|Items returned is required";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToDepartment))
                    errorMessages = errorMessages + "|Items returned to department is required";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToHr))
                    errorMessages = errorMessages + "|Items returned to Hr is required";


                if (errorMessages.Length > 0)
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };


                var resignationClearance = new ResignationClearanceDTO
                {
                    EmployeeID = accessUser.data.EmployeeId,
                    CompanyID = payload.CompanyID,
                    //FirstName = payload.FirstName,
                    //LastName = payload.LastName,
                    //MiddleName = payload.MiddleName,
                    //PreferredName = payload.PreferredName,
                    Signature = payload.Signature,
                    ReasonForExit = payload.ReasonForExit,
                    ResignationID = payload.ResignationID,
                    ItemsReturnedToDepartment = payload.ItemsReturnedToDepartment,
                    ItemsReturnedToAdmin = payload.ItemsReturnedToAdmin,
                    CreatedByUserID = accessUser.data.UserId,
                    LoansOutstanding = payload.LoansOutstanding,
                    ItemsReturnedToHr = payload.ItemsReturnedToHr,
                    ExitDate = payload.ExitDate,

                };


                var resp = await _resignationClearanceRepository.CreateResignationClearance(resignationClearance);

                if (resp < 0 )
                {
                    return new ExecutedResult<string>() { responseMessage = $"{resp}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                return new ExecutedResult<string>() { responseMessage = "Resignation clearance submitted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation clearance", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occured", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
        }


        public async Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(ID);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Clearance fetched successfully.");
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<ResignationClearanceDTO>> GetResignationClearanceByEmployeeID(long EmployeeId, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var resignation = await _resignationClearanceRepository.GetResignationClearanceByEmployeeID(EmployeeId);

                if (resignation == null)
                {
                    return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here
                _logger.LogInformation("Clearance fetched successfully.");
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                }

                var resignation = await _resignationClearanceRepository.GetAllResignationClearanceByCompany(companyID, filter.PageNumber, filter.PageSize, filter.SearchValue);

                if (resignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("clearances fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured:GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }

        }

        //public async Task<BaseResponse> GetPendingResignationClearanceByUserID(RequesterInfo requester, long userID)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var PendingResignation = await _resignationClearanceRepository.GetPendingResignationClearanceByUserID(userID);

        //        if (PendingResignation == null)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "PendingResignation not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = PendingResignation;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "PendingResignation fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetPendingResignationClearanceByUserID(long userID) ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: GetPendingResignationClearanceByUserID(long userID) ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}

        //public async Task<BaseResponse> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();




        //        var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ID);
        //        if (resignation == null)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Resignation Details Cannot be Found.";
        //            return response;
        //        }
        //        var approvedResignationResp = await _resignationClearanceRepository.ApprovePendingResignationClearance(request.userID, request.ID);

        //        if (approvedResignationResp < 0)
        //        {
        //            switch (approvedResignationResp)
        //            {
        //                case -1:
        //                    response.ResponseMessage = "Resignation not found";
        //                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //                    break;
        //                case -2:
        //                    response.ResponseMessage = "UnAthorized";
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
        //        _logger.LogError($"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}

        //public async Task<BaseResponse> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();




        //        var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ID);
        //        if (resignation == null)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Resignation Details Cannot be Found.";
        //            return response;
        //        }
        //        var DisapprovedResignation = await _resignationClearanceRepository.DisapprovePendingResignationClearance(request.userID, request.ID, request.reason);

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
        //                    response.ResponseMessage = "Invalid Approval Status";
        //                    response.ResponseCode = ResponseCode.InvalidApprovalStatus.ToString("D").PadLeft(2, '0');
        //                    break;
        //                default:
        //                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
        //            }

        //            return response;
        //        }

        //        response.Data = resignation;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Disapproved Resignation Clearance.";
        //        return response;
        //    }


        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: DisapprovePendingResignationClearance ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: DisapprovePendingResignationClearance ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
    }
}
