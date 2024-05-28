using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.Design;

namespace hrms_be_backend_business.Logic
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<LeaveTypeService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ILeaveTypeRepository _LeaveTypeRepository;

        public LeaveTypeService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<LeaveTypeService> logger,
            ILeaveTypeRepository LeaveTypeRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _LeaveTypeRepository = LeaveTypeRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateLeaveType(CreateLeaveTypeDTO creatDto)
        {
            var response = new BaseResponse();
            try
            {
                //string createdbyUserEmail = email;
               // string createdbyUserId = userFullView.UserId.ToString();
               // string RoleId = userFullView.RoleId.ToString();

              //  var ipAddress = userFullView.IpAddress.ToString();
              //  var port = userFullView.Port.ToString();

                //var requesterInfo = await _accountRepository.FindUser(null,createdbyUserEmail,null);
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

                //validate JobDescription payload here 
                //if (String.IsNullOrEmpty(creatDto.LeaveTypeName) || creatDto.CompanyID <= 0 || creatDto.MaximumLeaveDurationDays <= 0 ||
                //    String.IsNullOrEmpty(creatDto.Gender))
                //{
                //    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Please ensure all required fields are entered.";
                //    return response;
                //}

                //var isExistsComp = await _companyrepository.GetCompanyById(creatDto.CompanyID);
                //if (null == isExistsComp)
                //{
                //    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Invalid Company suplied.";
                //    return response;
                //}
                //else
                //{
                //    if (isExistsComp.IsDeleted)
                //    {
                //        response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"The Company suplied is already deleted, JobDescription cannot be created under it.";
                //        return response;
                //    }
                //}

                //creatDto.GradeName = $"{creatDto.GradeName} ({isExistsComp.CompanyName})";

                var isExists = await _LeaveTypeRepository.GetLeaveTypeByCompany(creatDto.LeaveTypeName, creatDto.CompanyID);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"LeaveType with name : {creatDto.LeaveTypeName} already exists for this Company.";
                    return response;
                }

                dynamic resp = await _LeaveTypeRepository.CreateLeaveType(creatDto);
                if (resp != null)
                {
                    //update action performed into audit log here

                    //var LeaveType = await _LeaveTypeRepository.GetLeaveTypeByName(creatDto.LeaveTypeName);

                    response.Data = resp;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating LeaveType. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateLeaveType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateLeaveType ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UpdateLeaveType(UpdateLeaveTypeDTO updateDto)
        {
            var response = new BaseResponse();
            try
            {
                

                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(updateDto.LeaveTypeName) || updateDto.CompanyID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var LeaveType = await _LeaveTypeRepository.GetLeaveTypeById(updateDto.LeaveTypeId);
                if (null == LeaveType)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified LeaveType";
                    response.Data = null;
                    return response;
                }

                var resp = await _LeaveTypeRepository.UpdateLeaveType(updateDto);
                if (resp != null)
                {
                    //update action performed into audit log here

                    var updatedLeaveType = await _LeaveTypeRepository.GetLeaveTypeById(updateDto.LeaveTypeId);

                    _logger.LogInformation("LeaveType updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType updated successfully.";
                    response.Data = updatedLeaveType;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating LeaveType.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateLeaveTypeDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateLeaveTypeDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteLeaveType(DeleteLeaveTypeDTO deleteDto)
        {
            var response = new BaseResponse();
            try
            {
                if (deleteDto.LeaveTypeId == 1)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {
                //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //        response.ResponseMessage = $"System Default LeaveType cannot be deleted.";
                //        return response;

                //    }
                //}
                var LeaveType = await _LeaveTypeRepository.GetLeaveTypeById(deleteDto.LeaveTypeId);
                if (null != LeaveType)
                {
                    var resp = await _LeaveTypeRepository.DeleteLeaveType(deleteDto);
                    if (resp != null)
                    {
                        //update action performed into audit log here

                        var DeletedLeaveType = await _LeaveTypeRepository.GetLeaveTypeById(deleteDto.LeaveTypeId);

                        _logger.LogInformation($"LeaveType deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"LeaveType deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting LeaveType.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified LeaveType";
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

        public async Task<BaseResponse> GetAllActiveLeaveType()
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

                var LeaveType = await _LeaveTypeRepository.GetAllActiveLeaveType();

                if (LeaveType.Any())
                {
                    response.Data = LeaveType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No LeaveType found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllLeaveType() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllLeaveType() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> GetAllLeaveType()
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

                var LeaveType = await _LeaveTypeRepository.GetAllLeaveType();

                if (LeaveType.Any())
                {
                    response.Data = LeaveType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No LeaveType found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllLeaveType() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllLeaveType() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> GetLeaveTypeById(long LeaveTypeId)
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

                var LeaveType = await _LeaveTypeRepository.GetLeaveTypeById(LeaveTypeId);

                if (LeaveType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "LeaveType fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetLeaveTypeById(long LeaveTypeId ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetLeaveTypeById(long LeaveTypeId  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetLeavebyCompanyId(long companyId)
        {
            _logger.LogInformation($"Received request to get leave type for CompanyId: {companyId}");
            BaseResponse response = new BaseResponse();

            try
            {
                var LeaveType = await _LeaveTypeRepository.GetAllLeaveTypeCompanyId(companyId);

                if (LeaveType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "LeaveType not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = LeaveType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "LeaveType fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllLeaveTypeCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllLeaveTypeCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


    }
}
