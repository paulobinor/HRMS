using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;

namespace hrms_be_backend_business.Logic
{
    public class EmpLocationService : IEmpLocationService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<EmpLocationService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IEmpLocationRepository _EmpLocationRepository;

        public EmpLocationService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<EmpLocationService> logger,
            IEmpLocationRepository EmpLocationRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _EmpLocationRepository = EmpLocationRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateEmpLocation(CreateEmpLocationDTO creatDto, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //validate JobDescription payload here 
                //if (String.IsNullOrEmpty(creatDto.EmployeeTypeName) || creatDto.CompanyID <= 0)
                //|| creatDto.DepartmentID <= 0 ||
                //creatDto.HodID <= 0 || creatDto.UnitID <= 0)
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

                //creatDto.LocationAddress = $"{creatDto.LocationAddress} ({isExistsComp.CompanyName})";

                var isExists = await _EmpLocationRepository.GetEmpLocationByName(creatDto.LocationAddress);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"EmpLocation with name : {creatDto.LocationAddress} already exists.";
                    return response;
                }

                dynamic resp = await _EmpLocationRepository.CreateEmpLocation(creatDto, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var EmpLocation = await _EmpLocationRepository.GetEmpLocationByName(creatDto.LocationAddress);

                    response.Data = EmpLocation;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmpLocation created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating EmployeeType. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateEmpLocation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateEmpLocation ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UpdateEmpLocation(UpdateEmpLocationDTO updateDto, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(updateDto.LocationAddress) || updateDto.CompanyID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var EmpLocation = await _EmpLocationRepository.GetEmpLocationById(updateDto.EmpLocationID);
                if (null == EmpLocation)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Unit";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _EmpLocationRepository.UpdateEmpLocation(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedEmpLocation = await _EmpLocationRepository.GetEmpLocationById(updateDto.EmpLocationID);

                    _logger.LogInformation("EmpLocation updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmpLocation updated successfully.";
                    response.Data = updatedEmpLocation;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating EmpLocation.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateEmpLocationDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateEmpLocationDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteEmpLocation(DeleteEmpLocationDTO deleteDto, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                if (deleteDto.EmpLocationID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default hod cannot be deleted.";
                    return response;
                }

                var Emplocation = await _EmpLocationRepository.GetEmpLocationById(deleteDto.EmpLocationID);
                if (null != Emplocation)
                {
                    dynamic resp = await _EmpLocationRepository.DeleteEmLocation(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedEmpLocation = await _EmpLocationRepository.GetEmpLocationById(deleteDto.EmpLocationID);

                        _logger.LogInformation($"Emplocation with name: {DeletedEmpLocation.LocationAddress} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"EmLocation with name: {DeletedEmpLocation.LocationAddress} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Emplocation.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Emplocation";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteEmLocation ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteEmLocation ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveEmpLocation(RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //update action performed into audit log here

                var Emplocation = await _EmpLocationRepository.GetAllActiveEmLocation();

                if (Emplocation.Any())
                {
                    response.Data = Emplocation;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Emplocation fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Emplocation found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveEmpLocation() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveEmpLocation() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllEmpLocation(RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //update action performed into audit log here

                var EmpLocation = await _EmpLocationRepository.GetAllEmpLocation();

                if (EmpLocation.Any())
                {
                    response.Data = EmpLocation;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmpLocation fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No EmpLocation found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmpLocation() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmpLocation() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmpLocationbyId(long EmpLocationID, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var EmpLocation = await _EmpLocationRepository.GetEmpLocationById(EmpLocationID);

                if (EmpLocation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmpLocation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = EmpLocation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "EmpLocation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmpLocationbyId(long EmpLocationID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetEmpLocationbyId(long EmpLocationID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmpLocationbyCompanyId(long CompanyID, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var EmpLocation = await _EmpLocationRepository.GetAllEmpLocationCompanyId(CompanyID);

                if (EmpLocation == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmpLocation not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = EmpLocation;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "EmpLocation fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmpLocationbyCompanyId(long CompanyID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetEmpLocationbyCompanyId(long CompanyID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


    }
}
