using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IExitClearanceSetupRepository _exitClearanceSetupRepository;
        private readonly IResignationClearanceApprovalsRepository _resignationClearanceApprovalsRepository;
        private readonly IAuthService _authService;
        private readonly IResignationRepository _resignationRepository;
        private readonly IMailService _mailService;




        public ResignationClearanceService(IConfiguration config, IResignationClearanceRepository resignationClearanceRepository, ILogger<ResignationClearanceService> logger, IAccountRepository accountRepository, IAuthService authService, IResignationRepository resignationRepository, IExitClearanceSetupRepository exitClearanceSetupRepository, IMailService mailService, IResignationClearanceApprovalsRepository resignationClearanceApprovalsRepository)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationClearanceRepository = resignationClearanceRepository;
            _authService = authService;
            _resignationRepository = resignationRepository;
            _exitClearanceSetupRepository = exitClearanceSetupRepository;
            _mailService = mailService;
            _resignationClearanceApprovalsRepository = resignationClearanceApprovalsRepository;
        }


        public async Task<ExecutedResult<string>> SubmitResignationClearance(ResignationClearanceVM payload, string AccessKey, string RemoteIpAddress)
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

                //if (payload.ExitDate > DateTime.Now)
                //    errorMessages = errorMessages + "|Invalid exit date";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToAdmin))
                    errorMessages = errorMessages + "|Items returned is required";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToDepartment))
                    errorMessages = errorMessages + "|Items returned to department is required";
                if (string.IsNullOrWhiteSpace(payload.ItemsReturnedToHr))
                    errorMessages = errorMessages + "|Items returned to Hr is required";


                if (errorMessages.Length > 0)
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                payload.EmployeeID = accessUser.data.EmployeeId;

              
                var resignation = await _resignationRepository.GetResignationByID(payload.ResignationID);

                var resignationClearance = new ResignationClearanceDTO
                {
                    EmployeeID = payload.EmployeeID,
                    CompanyID = payload.CompanyID,
                    Signature = payload.Signature,
                    //ReasonForExit = resignation.ReasonForResignation,
                    ResignationID = resignation.ResignationID,
                    ItemsReturnedToDepartment = payload.ItemsReturnedToDepartment,
                    ItemsReturnedToAdmin = payload.ItemsReturnedToAdmin,
                    CreatedByUserID = accessUser.data.UserId,
                    LoansOutstanding = payload.LoansOutstanding,
                    ItemsReturnedToHr = payload.ItemsReturnedToHr,
                    ExitDate = resignation.LastDayOfWork,
                    DateCreated = payload.DateCreated,

                };


                var resp = await _resignationClearanceRepository.CreateResignationClearance(resignationClearance);

                if (resp < 0 )
                {
                    return new ExecutedResult<string>() { responseMessage = $"{resp}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }
                var createdClearance = await _resignationClearanceRepository.GetResignationClearanceByID(resp);

                //send mail to employee's HOD
                _mailService.SendResignationClearanceApproveMailToApprover(createdClearance.HodEmployeeID, createdClearance.EmployeeID);
      

                return new ExecutedResult<string>() { responseMessage = "Resignation clearance submitted Successfully", responseCode = (00).ToString(), data = null };
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
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };

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
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetResignationClearanceByID(long ID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<ResignationClearanceDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress, DateTime? startDate, DateTime? endDate)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                }

                var resignation = await _resignationClearanceRepository.GetAllResignationClearanceByCompany(companyID, filter.PageNumber, filter.PageSize, filter.SearchValue,accessUser.data.EmployeeId,startDate,endDate);

                if (resignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                

                _logger.LogInformation("clearances fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = (00).ToString(), data = resignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured:GetAllResignationClearanceByCompany(PaginationFilter filter, long companyID, string AccessKey, string RemoteIpAddress) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }

        }

        public async Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetPendingResignationClearanceByEmployeeID(long EmployeeID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            try
            {

                var PendingResignation = await _resignationClearanceRepository.GetPendingResignationClearanceByEmployeeID(EmployeeID);

                if (PendingResignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Resignation fetched Successfully", responseCode = (00).ToString(), data = PendingResignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingResignationClearanceByEmployeeID(long EmployeeID) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<ResignationClearanceDTO>>> GetPendingResignationClearanceByCompanyID(long companyID, string AccessKey, string RemoteIpAddress, PaginationFilter filter, DateTime? startDate, DateTime? endDate)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

            }
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            try
            {

                var PendingResignation = await _resignationClearanceRepository.GetPendingResignationClearanceByCompnayID(companyID, accessUser.data.EmployeeId, filter.PageNumber, filter.PageSize, filter.SearchValue, startDate,endDate);

                if (PendingResignation == null)
                {
                    return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Resignation fetched successfully.");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Resignation fetched Successfully", responseCode = (00).ToString(), data = PendingResignation };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetPendingResignationClearanceByCompanyID(long CompanyID) ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<ResignationClearanceDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }


        public async Task<ExecutedResult<string>> ApprovePendingResignationClearance(ApproveResignationClearanceDTO request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ResignationClearanceID);
                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }


                if (accessUser.data.EmployeeId == resignation.HodEmployeeID && resignation.IsHodApproved == false)
                {

                    var approveResignatonClearanceByHod =  await _resignationClearanceRepository.ApprovePendingResignationClearance(request.employeeID, request.ResignationClearanceID);
                    if (!approveResignatonClearanceByHod.Contains("Success"))
                    {
                        return new ExecutedResult<string>() { responseMessage = $"{approveResignatonClearanceByHod}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                    }

                    //send approval confirmation mail

                    _mailService.SendResignationClearanceApproveConfirmationMail(resignation.EmployeeID, accessUser.data.EmployeeId);


                    //Send mail to departments setup to approve resignation clearance
                    var DepartmentsThatAreNotFinalApprover = await _exitClearanceSetupRepository.GetDepartmentsThatAreNotFinalApproval(resignation.CompanyID);
                    if (DepartmentsThatAreNotFinalApprover.Count() == 0)
                    {
                        var FinalApprovalDepartment = await _exitClearanceSetupRepository.GetDepartmentThatIsFinalApprroval(resignation.CompanyID);
                        _mailService.SendResignationClearanceApproveMailToApprover(FinalApprovalDepartment.HodEmployeeID, resignation.EmployeeID);
                    }
                    else
                    {
                        foreach (var item in DepartmentsThatAreNotFinalApprover)
                        {
                            _mailService.SendResignationClearanceApproveMailToApprover(item.HodEmployeeID, resignation.EmployeeID);

                        }
                    }

                        return new ExecutedResult<string>() { responseMessage = "Resignation clearance approved successfully.", responseCode = (00).ToString(), data = null };
                }
                else
                {
                    if (!resignation.IsHodApproved == false)
                    {
                        //get the exit clearance setup Id by using the hod employee id
                        var ClearanceSetup = await _exitClearanceSetupRepository.GetExitClearanceSetupByHodEmployeeID(accessUser.data.EmployeeId);

                        //Save approval in resignation clearance approvals table
                        var saveApproval = await _resignationClearanceApprovalsRepository.CreateResignationClearanceApprovals(resignation.CompanyID, resignation.ResignationClearanceID, ClearanceSetup.ExitClearanceSetupID, accessUser.data.UserId,ClearanceSetup.DepartmentName);
                        if (!saveApproval.Contains("Success"))
                        {
                            return new ExecutedResult<string>() { responseMessage = $"{saveApproval}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                        }

                        //Get all the departments except the final approval department from exit clearance table
                        var notFinalApprovalDepartments = await _exitClearanceSetupRepository.GetDepartmentsThatAreNotFinalApproval(resignation.CompanyID);

                        //Get the final approval department from exit clearance table
                        var finalApprovalDepartment = await _exitClearanceSetupRepository.GetDepartmentThatIsFinalApprroval(resignation.CompanyID);

                        //Get all the departments that have approved this clearance from the resignation clearance approvals table
                        var approvedClearance = await _resignationClearanceApprovalsRepository.GetAllResignationClearanceApprovalsByResignationClearanceID(resignation.ResignationClearanceID);

                        //check if all the non final approval departments have approved then send approve mail to final approver
                        if (notFinalApprovalDepartments.Count() == approvedClearance.Count())
                        {
                            _mailService.SendResignationClearanceApproveMailToApprover(finalApprovalDepartment.HodEmployeeID, resignation.EmployeeID);
                        }

                        //check if its the final approver that's approving
                        if (finalApprovalDepartment.HodEmployeeID == accessUser.data.EmployeeId)
                        {
                            //approve resignation clearance
                            var approvedResignationClearanceResp = await _resignationClearanceRepository.ApprovePendingResignationClearance(request.employeeID, request.ResignationClearanceID);

                            if (!approvedResignationClearanceResp.Contains("Success"))
                            {
                                return new ExecutedResult<string>() { responseMessage = $"{approvedResignationClearanceResp}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                            }
                        }

                        //send approval confirmation mail
                        _mailService.SendResignationClearanceApproveConfirmationMail(resignation.EmployeeID, accessUser.data.EmployeeId);

                        return new ExecutedResult<string>() { responseMessage = "Resignation clearance approved successfully.", responseCode = (00).ToString(), data = null };
                    }
                    else
                    {
                        return new ExecutedResult<string>() { responseMessage = "This hasnt been acknowledge by the employee's HOD", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                    }

                }

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DisapprovePendingResignationClearance(DisapprovePendingResignationClearanceDTO request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var resignation = await _resignationClearanceRepository.GetResignationClearanceByID(request.ResignationClearanceID);
                if (resignation == null)
                {
                    return new ExecutedResult<string>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //disapprove resignation clearance
                var disapprovedResignationResp = await _resignationClearanceRepository.DisapprovePendingResignationClearance(accessUser.data.EmployeeId, request.ResignationClearanceID,request.reason);

                //Get the final approval department from exit clearance table
                //var finalApprovalDepartment = await _exitClearanceSetupRepository.GetDepartmentThatIsFinalApprroval(resignation.CompanyID);


                if (!disapprovedResignationResp.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{disapprovedResignationResp}", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

                }

                _mailService.SendResignationClearanceDisapproveConfirmationMail(resignation.EmployeeID,accessUser.data.EmployeeId, request.reason);


                return new ExecutedResult<string>() { responseMessage = "Resignation clearance disapproved successfully.", responseCode = (00).ToString(), data = null };

            }


            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ApprovePendingResignationClearance ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }
    }
}
