using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;

namespace hrms_be_backend_business.Logic
{
    public class ReviwerService : IReviwerService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<ReviwerService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IReviwerRepository _ReviwerRepository;
      

        public ReviwerService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<ReviwerService> logger,
            IReviwerRepository ReviwerRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _ReviwerRepository = ReviwerRepository;
            _companyrepository = companyrepository;
            
        }

        public async Task<BaseResponse> CreateReviwer(CreateReviwerDTO create, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(createdbyUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                //validate DepartmentDto payload here 
                //if (String.IsNullOrEmpty(GroupDto.GroupName) || GroupDto.CompanyID <= 0)

                //{
                //    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Please ensure all required fields are entered.";
                //    return response;
                //}

                var isExistsComp = await _companyrepository.GetCompanyById(create.CompanyID);
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
                        response.ResponseMessage = $"The Company suplied is already deleted, HOD cannot be created under it.";
                        return response;
                    }
                }

                //GroupDto.GroupName = $"{GroupDto.GroupName} ({isExistsComp.CompanyName})";

                var isExists = await _ReviwerRepository.GetReviwerByCompany(create.UserId, (int)create.CompanyID);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Group with name : {create.UserId} already exists for this Company.";
                    return response;
                }

                dynamic resp = await _ReviwerRepository.CreateReviwer(create, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var Reviwer = await _ReviwerRepository.GetReviwerByName(create.UserId);

                    response.Data = Reviwer;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Reviwer created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating Reviwer. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateReviwer ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateReviwer ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> DeleteReviwer(DeleteReviwerDTO deleteDto, RequesterInfo requester)
        {
            var response = new BaseResponse();
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

                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                if (deleteDto.ReviwerID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default Reviwer cannot be deleted.";
                    return response;
                }

                var Reviwer = await _ReviwerRepository.GetReviwerById(deleteDto.ReviwerID);
                if (null != Reviwer)
                {
                    dynamic resp = await _ReviwerRepository.DeleteReviwer(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedReviwer = await _ReviwerRepository.GetReviwerById(deleteDto.ReviwerID);

                        _logger.LogInformation($"Group with name: {DeletedReviwer.UserId} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Group with name: {DeletedReviwer.UserId} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Reviwer.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Reviwer";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeletedReviwer ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeletedReviwer ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetReviwerbyId(long UserId, RequesterInfo requester)
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


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var Reviwer = await _ReviwerRepository.GetReviwerById(UserId);

                if (Reviwer == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Reviwer not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Reviwer;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reviwer fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetReviwerById(UserId)==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetReviwerById(UserId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
        public async Task<BaseResponse> GetReviwerbyCompanyId(long companyId, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 2)
                {

                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;
                    }

                }

                var Reviwer = await _ReviwerRepository.GetAllReviwerCompanyId(companyId);

                if (Reviwer == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Reviwer not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Reviwer;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Reviwer fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllReviwerCompanyId(companyId)==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllReviwerCompanyId(companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
