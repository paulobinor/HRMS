using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;

namespace hrms_be_backend_business.Logic
{
    public class GradeLeaveService : IGradeLeaveService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<GradeLeaveService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IGradeLeaveRepo _GradeLeaveRepo;
        private readonly IAuthService _authService;


        public GradeLeaveService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<GradeLeaveService> logger,
            IGradeLeaveRepo GradeLeaveRepo, IAuditLog audit, ICompanyRepository companyrepository, IAuthService authService)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _GradeLeaveRepo = GradeLeaveRepo;
            _companyrepository = companyrepository;
            _authService = authService;
        }

        public async Task<BaseResponse> CreateGradeLeave(CreateGradeLeaveDTO creatDto, string AccessKey, string RemoteIpAddress)
        {

            var response = new BaseResponse();
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {

                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "User information cannot be found.";
                return response;

            }
            try
            {
                //validate JobDescription payload here 
                if (creatDto.LeaveTypeId <= 0 || creatDto.CompanyID <= 0 || creatDto.GradeID <= 0)

                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var isExistsComp = await _companyrepository.GetCompanyById(creatDto.CompanyID);
                if (null == isExistsComp)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid Company suplied.";
                    return response;
                }
                else
                {
                    if (isExistsComp.IsDeleted)
                    {
                        response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"The Company suplied is already deleted, JobDescription cannot be created under it.";
                        return response;
                    }
                }
                creatDto.CreatedByUserID = accessUser.data.UserId;
                dynamic resp = await _GradeLeaveRepo.CreateGradeLeave(creatDto);

                if (resp.Contains("Success"))
                {
                    //update action performed into audit log here

                    var LeaveType = await _GradeLeaveRepo.GetGradeLeaveById(creatDto.LeaveTypeId);

                    response.Data = LeaveType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating LeaveType. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateGradeLeave ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateGradeLeave ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UpdateGradeLeave(UpdateGradeLeaveDTO updateDto)
        {
            var response = new BaseResponse();
            try
            {
               // string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                //var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
                //if (null == requesterInfo)
                //{
                //    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Requester information cannot be found.";
                //    return response;
                //}

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                //validate DepartmentDto payload here 
                if (updateDto.CompanyID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var LeaveType = await _GradeLeaveRepo.GetGradeLeaveById(updateDto.GradeLeaveID);
                if (null == LeaveType)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Grade Leave";
                    response.Data = null;
                    return response;
                }

                var resp = await _GradeLeaveRepo.UpdateGradeLeave(updateDto);
                if (resp != null)
                {
                    //update action performed into audit log here

                 //   var updatedLeaveType = await _GradeLeaveRepo.GetGradeLeaveById(updateDto.GradeLeaveID);

                    _logger.LogInformation("GradeLeave updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave updated successfully.";
                    response.Data = resp;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Grade Leave.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateGradeLeaveDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateLeaveTypeDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteGradeLeave(DeleteGradeLeaveDTO deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                //string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                //var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
                //if (null == requesterInfo)
                //{
                //    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Requester information cannot be found.";
                //    return response;
                //}

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                //if (deleteDto.GradeLeaveID == 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"System Default Grade Leave cannot be deleted.";
                //    return response;
                //}

                var LeaveType = await _GradeLeaveRepo.GetGradeLeaveById(deleteDto.GradeLeaveID);
                if (null != LeaveType)
                {
                    var resp = await _GradeLeaveRepo.DeleteGradeLeave(deleteDto);
                    if (resp != null)
                    {
                        //update action performed into audit log here

                      //  var DeletedLeaveType = await _GradeLeaveRepo.GetGradeLeaveById(deleteDto.GradeLeaveID);

                        _logger.LogInformation($"LeaveType with name: {resp.GradeLeaveID} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"LeaveType with name: {resp.GradeLeaveID} Deleted successfully.";
                        response.Data = resp;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Grade Leave.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Grade Leave ";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteLeaveType ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveGradeLeave(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                //string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                //var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
                //if (null == requesterInfo)
                //{
                //    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Requester information cannot be found.";
                //    return response;
                //}


                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                //update action performed into audit log here

                var LeaveType = await _GradeLeaveRepo.GetAllActiveGradeLeave();

                if (LeaveType.Any())
                {
                    response.Data = LeaveType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Grade Leave found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGradeLeave() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllGradeLeave() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllGradeLeave(string AccessKey, string RemoteIpAddress)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {

                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User information cannot be found.";
                    return response;

                }


                //update action performed into audit log here

                var LeaveType = await _GradeLeaveRepo.GetAllGradeLeave();

                if (LeaveType.Any())
                {
                    response.Data = LeaveType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Grade Leave found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGradeLeave() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllGradeLeave() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetGradeLeaveById(long GradeLeaveID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                //string requesterUserId = requester.UserId.ToString();
                //string RoleId = requester.RoleId.ToString();

                //var ipAddress = requester.IpAddress.ToString();
                //var port = requester.Port.ToString();

                //var requesterInfo = await _accountRepository.FindUser(null,requesterUserEmail,null);
                //if (null == requesterInfo)
                //{
                //    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Requester information cannot be found.";
                //    return response;
                //}


                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //        return response;

                //    }

                //}

                var LeaveType = await _GradeLeaveRepo.GetGradeLeaveById(GradeLeaveID);

                if (LeaveType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Grade Leave fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetGradeLeaveById(long GradeLeaveID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetGradeLeaveById(long GradeLeaveID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetGradeLeavebyCompanyId(long companyId, string AccessKey, string RemoteIpAddress)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {

                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User information cannot be found.";
                    return response;

                }


                var LeaveType = await _GradeLeaveRepo.GetAllGradeLeaveCompanyId(companyId);

                if (LeaveType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Grade Leave not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Grade Leave fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGradeLeaveCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllGradeLeaveCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


    }
}
