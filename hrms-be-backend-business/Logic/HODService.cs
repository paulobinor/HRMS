using ExcelDataReader;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class HODService : IHODService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<HODService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IHODRepository _hodRepository;

        public HODService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<HODService> logger,
            IHODRepository hodRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _hodRepository = hodRepository;
            _companyrepository = companyrepository;
        }


        public async Task<BaseResponse> CreateHOD(CreateHodDTO HodDto, RequesterInfo requester)
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
                if (HodDto.UserId <= 0 || HodDto.CompanyID <= 0 )
                    //String.IsNullOrEmpty(DepartmentDto.Email) || String.IsNullOrEmpty(DepartmentDto.ContactPhone))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var isExistsComp = await _companyrepository.GetCompanyById(HodDto.CompanyID);
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

                //HodDto.HODName = $"{HodDto.HODName} ({isExistsComp.CompanyName})";

                //var isExists = await _hodRepository.GetHODByCompany(HodDto.HODName, (int)HodDto.CompanyID);
                //if (null != isExists)
                //{
                //    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Department with name : {HodDto.HODName} already exists for this Company.";
                //    return response;
                //}

                dynamic resp = await _hodRepository.CreateHOD(HodDto, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var hod = await _hodRepository.GetHODByUserId(HodDto.UserId);

                    response.Data = hod;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HOD created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating HOD. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateHOD ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateHOD ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateHODBulkUpload(IFormFile payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (payload == null || payload.Length <= 0)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "No file for Upload";
                    return response;
                }
                else if (!Path.GetExtension(payload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "File not an Excel Format";
                    return response;
                }
                else
                {
                    var stream = new MemoryStream();
                    await payload.CopyToAsync(stream);

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    var reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    int rowCount = ds.Tables[0].Rows.Count;
                    DataTable serviceDetails = ds.Tables[0];

                    int k = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        string HODName = serviceDetails.Rows[0][0].ToString();
                        string CompanyName = serviceDetails.Rows[0][1].ToString();


                        if (HODName != "HODName" 
                        || CompanyName != "CompanyName")

                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                var hODName =await _hodRepository.GetHODByName(serviceDetails.Rows[row][0].ToString());
                                var company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][1].ToString());

                                long userId= hODName.UserId; 
                                long companyID = company.CompanyId;


                                var hodrequest = new CreateHodDTO
                                {
                                    UserId = userId,
                                    CompanyID = companyID


                                };

                                var hodrequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateHOD(hodrequest, hodrequester);


                                if (resp.ResponseCode == "00")
                                {
                                    k++;
                                }
                                else
                                {
                                    errorOutput.Append($"Row {row} failed due to {resp.ResponseMessage}" + "\n");
                                }
                            }
                        }

                    }



                    if (k == rowCount - 1)
                    {
                        response.ResponseCode = "00";
                        response.ResponseMessage = "All record inserted successfully";



                        return response;
                    }
                    else
                    {
                        response.ResponseCode = "02";
                        response.ResponseMessage = errorOutput.ToString();



                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }


        public async Task<BaseResponse> UpdateHOD(UpdateHodDTO updateDto, RequesterInfo requester)
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

                //validate DepartmentDto payload here 
                if (updateDto.UserId <= 0 || updateDto.CompanyID <= 0
                    || updateDto.HodID <= 0 )
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var Department = await _hodRepository.GetHODById(updateDto.HodID);
                if (null == Department)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified HOD";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _hodRepository.UpdateHOD(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedDepartment = await _hodRepository.GetHODById(updateDto.HodID);

                    _logger.LogInformation("Hod updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Hod updated successfully.";
                    response.Data = updatedDepartment;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Hod.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateHodDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateHodDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteHOD(DeleteHodDTO deleteDto, RequesterInfo requester)
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

                if (deleteDto.HodID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default hod cannot be deleted.";
                    return response;
                }

                var hod = await _hodRepository.GetHODById(deleteDto.HodID);
                if (null != hod)
                {
                    dynamic resp = await _hodRepository.DeleteHOD(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedHOD = await _hodRepository.GetHODById(deleteDto.HodID);

                        _logger.LogInformation($"Hod with name: {DeletedHOD.UserId} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"HOD with name: {DeletedHOD.UserId} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Hod.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Hod";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: Hod ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveHOD(RequesterInfo requester)
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

                //update action performed into audit log here

                var hod = await _hodRepository.GetAllActiveHODs();

                if (hod.Any())
                {
                    response.Data = hod;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Hod fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Hod found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveHODs() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveHODs() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllHOD(RequesterInfo requester)
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

                //update action performed into audit log here

                var hod = await _hodRepository.GetAllHOD();

                if (hod.Any())
                {
                    response.Data = hod;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "hod fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No hod found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllHOD() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllHOD() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetHODbyId(long HodID, RequesterInfo requester)
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

                var hod = await _hodRepository.GetHODById(HodID);

                if (hod == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Hod not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = hod;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Hod fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetHODById(long HodID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetHODById(long HodID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetHODbyCompanyId(long companyId, RequesterInfo requester)
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

                var HOD = await _hodRepository.GetAllHODCompanyId(companyId);

                if (HOD == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HOD not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = HOD;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "HOD fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllHODCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllHODCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }




    }
}
