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
    public class HospitalProvidersService : IHospitalProvidersService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<HospitalProvidersService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IHospitalProvidersRepository _HospitalProvidersRepository;
        private readonly IHospitalPlanRepository _hospitalPlanRepository;
        private readonly IStateRepository _StateRepository;

        public HospitalProvidersService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<HospitalProvidersService> logger,
            IHospitalProvidersRepository HospitalProvidersRepository, IAuditLog audit, ICompanyRepository companyrepository,
            IHospitalPlanRepository hospitalPlanRepository , IStateRepository stateRepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _HospitalProvidersRepository = HospitalProvidersRepository;
            _companyrepository = companyrepository;
            _hospitalPlanRepository = hospitalPlanRepository;
            _StateRepository = stateRepository;

        }

        public async Task<BaseResponse> CreateHospitalProviders(CreateHospitalProvidersDTO create, RequesterInfo requester)
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
                if (String.IsNullOrEmpty(create.ProvidersNames) || create.CompanyID <= 0)
               
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

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

                //create.ProvidersNames = $"{create.ProvidersNames} ({isExistsComp.CompanyName})";

                var isExists = await _HospitalProvidersRepository.GetHospitalProvidersByCompany(create.ProvidersNames, (int)create.CompanyID);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"HospitalProviders with name : {create.ProvidersNames} already exists for your Company.";
                    return response;
                }

                dynamic resp = await _HospitalProvidersRepository.CreateHospitalProviders(create, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var Providers = await _HospitalProvidersRepository.GetHospitalProvidersByName(create.ProvidersNames);

                    response.Data = Providers;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HospitalProviders created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating HOD. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateHospitalProvidersBulkUpload(IFormFile payload, RequesterInfo requester)
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

                        string ProvidersName = serviceDetails.Rows[0][0].ToString();
                        string State = serviceDetails.Rows[0][1].ToString();
                        string Town1 = serviceDetails.Rows[0][2].ToString();
                        string Town2 = serviceDetails.Rows[0][3].ToString();
                        string Address1 = serviceDetails.Rows[0][4].ToString();
                        string Address2 = serviceDetails.Rows[0][5].ToString();
                        string HospitalPlan = serviceDetails.Rows[0][6].ToString();
                        string CompanyName = serviceDetails.Rows[0][7].ToString();
                        

                        if (ProvidersName != "ProvidersName" || State != "State" || Town1 != "Town1" || Town2 != "Town2"
                         || Address1 != "Address1" || Address2 != "Address2" || HospitalPlan != "HospitalPlan"
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

                                string providersName = serviceDetails.Rows[row][0].ToString();
                                var stateName = await _StateRepository.GetStateByName(serviceDetails.Rows[row][1].ToString());
                                string town1 = serviceDetails.Rows[row][2].ToString();
                                string town2 = serviceDetails.Rows[row][3].ToString();
                                string address1 = serviceDetails.Rows[row][4].ToString();
                                string address2 = serviceDetails.Rows[row][5].ToString();
                                var hospitalPlanName = await _hospitalPlanRepository.GetHospitalPlanByName(serviceDetails.Rows[row][6].ToString());
                                var company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][7].ToString());

                                long stateID = stateName.StateID;
                                long hospitalPlanID = hospitalPlanName.HospitalPlanID;
                                long companyID = company.CompanyId;


                                var providersrequest = new CreateHospitalProvidersDTO
                                {
                                    ProvidersNames = providersName,
                                    StateID = stateID,
                                    HospitalPlanID = hospitalPlanID,
                                    CompanyID = companyID


                                };

                                var providersrequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateHospitalProviders(providersrequest, providersrequester);


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


        public async Task<BaseResponse> UpdateHospitalProviders(UpdateHospitalProvidersDTO updateDto, RequesterInfo requester)
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
                if (String.IsNullOrEmpty(updateDto.ProvidersNames) || updateDto.CompanyID <= 0
                    /*|| updateDto.HodID <= 0*/)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var Providers = await _HospitalProvidersRepository.GetHospitalProvidersById(updateDto.ID);
                if (null == Providers)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified HOD";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _HospitalProvidersRepository.UpdateHospitalProviders(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedProviders = await _HospitalProvidersRepository.GetHospitalProvidersById(updateDto.ID);

                    _logger.LogInformation("Providers updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Providers updated successfully.";
                    response.Data = updatedProviders;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Providers.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteHospitalProviders(DeleteHospitalProvidersDTO deleteDto, RequesterInfo requester)
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

                if (deleteDto.ID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default hod cannot be deleted.";
                    return response;
                }

                var Providers = await _HospitalProvidersRepository.GetHospitalProvidersById(deleteDto.ID);
                if (null != Providers)
                {
                    dynamic resp = await _HospitalProvidersRepository.DeleteHospitalProviders(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedProviders = await _HospitalProvidersRepository.GetHospitalProvidersById(deleteDto.ID);

                        _logger.LogInformation($"HospitalProviders with name: {DeletedProviders.ProvidersNames} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"HospitalProviders with name: {DeletedProviders.ProvidersNames} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting HospitalProviders.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified HospitalProviders";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteHospitalProviders ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteHospitalProviders ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveHospitalProviders(RequesterInfo requester)
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

                var Providers = await _HospitalProvidersRepository.GetAllActiveHospitalProviders();

                if (Providers.Any())
                {
                    response.Data = Providers;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HospitalProviders fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No HospitalProviders found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveHospitalProviders() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveHospitalProviders() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> GetAllHospitalProviders(RequesterInfo requester)
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

                var Providers = await _HospitalProvidersRepository.GetAllHospitalProviders();

                if (Providers.Any())
                {
                    response.Data = Providers;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HospitalProviders fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No HospitalProviders found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllHospitalProviders() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllHospitalProviders() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetHospitalProvidersbyId(long ID, RequesterInfo requester)
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

                var Providers = await _HospitalProvidersRepository.GetHospitalProvidersById(ID);

                if (Providers == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HospitalProviders not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Providers;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "HospitalProviders fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetHospitalProvidersById(long ID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetHospitalProvidersById(long ID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetHospitalProvidersbyCompanyId(long companyId, RequesterInfo requester)
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

                var Providers = await _HospitalProvidersRepository.GetAllHospitalProvidersCompanyId(companyId);

                if (Providers == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "HospitalProviders not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Providers;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "HospitalProviders fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllHospitalProvidersCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllHospitalProvidersCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

    }
}
