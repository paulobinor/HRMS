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
    public  class EmployeeTypeService : IEmployeeTypeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<EmployeeTypeService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IEmployeeTypeRepository _EmployeeTypeRepository;

        public EmployeeTypeService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<EmployeeTypeService> logger,
            IEmployeeTypeRepository EmployeeTypeRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _EmployeeTypeRepository = EmployeeTypeRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> CreateEmployeeType(CraeteEmployeeTypeDTO creatDto, RequesterInfo requester)
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

                //validate JobDescription payload here 
                if (String.IsNullOrEmpty(creatDto.EmployeeTypeName) || creatDto.CompanyID <= 0)
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

                //creatDto.EmployeeTypeName = $"{creatDto.EmployeeTypeName} ({isExistsComp.CompanyName})";

                var isExists = await _EmployeeTypeRepository.GetEmployeeTypeByCompany(creatDto.EmployeeTypeName, (int)creatDto.CompanyID);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"EmployeeType with name : {creatDto.EmployeeTypeName} already exists for this Company.";
                    return response;
                }



                dynamic resp = await _EmployeeTypeRepository.CreateEmployeeType(creatDto, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var EmployeeType = await _EmployeeTypeRepository.GetEmployeeTypeByName(creatDto.EmployeeTypeName);

                    response.Data = EmployeeType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating EmployeeType. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateEmployeeType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateEmployeeType ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateEmployeeTypeBulkUpload(IFormFile payload, RequesterInfo requester)
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

                        string EmployeeTypeName = serviceDetails.Rows[0][0].ToString();
                        string CompanyName = serviceDetails.Rows[0][1].ToString();


                        if (EmployeeTypeName != "EmployeeTypeName" || CompanyName != "CompanyName")
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                string employeeTypeName = serviceDetails.Rows[row][0].ToString();
                                var company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][1].ToString());


                                long companyID = company.CompanyId;


                                var employeeTypeprequest = new CraeteEmployeeTypeDTO
                                {
                                    EmployeeTypeName = EmployeeTypeName,
                                    CompanyID = companyID


                                };

                                var employeeTypeprequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateEmployeeType(employeeTypeprequest, employeeTypeprequester);


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

        public async Task<BaseResponse> UpdateEmployeeType(UpdateEmployeeTypeDTO updateDto, RequesterInfo requester)
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
                if (String.IsNullOrEmpty(updateDto.EmployeeTypeName) || updateDto.CompanyID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var Unit = await _EmployeeTypeRepository.GetEmployeeTypeById(updateDto.EmployeeTypeID);
                if (null == Unit)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Unit";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _EmployeeTypeRepository.UpdateEmployeeType(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedEmployeeType = await _EmployeeTypeRepository.GetEmployeeTypeById(updateDto.EmployeeTypeID);

                    _logger.LogInformation("EmployeeType updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType updated successfully.";
                    response.Data = updatedEmployeeType;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating EmployeeType.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateEmployeeTypeDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateEmployeeTypeDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> DeleteEmployeeType(DeleteEmployeeTypeDTO deleteDto, RequesterInfo requester)
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

                if (deleteDto.EmployeeTypeID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default hod cannot be deleted.";
                    return response;
                }

                var EmployeeType = await _EmployeeTypeRepository.GetEmployeeTypeById(deleteDto.EmployeeTypeID);
                if (null != EmployeeType)
                {
                    dynamic resp = await _EmployeeTypeRepository.DeleteEmployeeType(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedUnit = await _EmployeeTypeRepository.GetEmployeeTypeById(deleteDto.EmployeeTypeID);

                        _logger.LogInformation($"EmployeeType with name: {DeletedUnit.EmployeeTypeName} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"EmployeeType with name: {DeletedUnit.EmployeeTypeName} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting EmployeeType.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified EmployeeType";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteEmployeeType ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteEmployeeType ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveEmployeeType(RequesterInfo requester)
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

                var hod = await _EmployeeTypeRepository.GetAllEmployeeType();

                if (hod.Any())
                {
                    response.Data = hod;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No EmployeeType found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveEmployeeType() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveEmployeeType() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllEmployeeType(RequesterInfo requester)
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

                var EmployeeType = await _EmployeeTypeRepository.GetAllEmployeeType();

                if (EmployeeType.Any())
                {
                    response.Data = EmployeeType;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No EmployeeType found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployeeType() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployeeType() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeeTypeById(long EmployeeTypeID, RequesterInfo requester)
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

                var EmployeeType = await _EmployeeTypeRepository.GetEmployeeTypeById(EmployeeTypeID);

                if (EmployeeType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = EmployeeType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "EmployeeType fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmployeeTypeById(long EmployeeTypeID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetEmployeeTypeById(long EmployeeTypeID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeeTypebyCompanyId(long companyId, RequesterInfo requester)
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

                var Emp = await _EmployeeTypeRepository.GetAllEmployeeTypeCompanyId(companyId);

                if (Emp == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "EmployeeType not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Emp;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "EmployeeType fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

    }
}
