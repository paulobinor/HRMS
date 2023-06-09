using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Branch;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Departments.IRepository;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
using Com.XpressPayments.Data.Repositories.Group;
using Com.XpressPayments.Data.Repositories.HOD;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IAuditLog _audit;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IDepartmentRepository _departmentrepository;
        private readonly IHODRepository _hODRepository;
        private readonly IGroupRepository _GroupRepository;
        private readonly IBranchRepository _branchRepository;
        

        public DepartmentService(IConfiguration configuration, IAccountRepository accountRepository, ILogger<DepartmentService> logger,
            IDepartmentRepository departmentRepository, IAuditLog audit, IMapper mapper, ICompanyRepository companyrepository,
            IHODRepository hODRepository, IGroupRepository groupRepository, IBranchRepository branchRepository)
        {
            _audit = audit;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _accountRepository = accountRepository;
            _departmentrepository = departmentRepository;
            _companyrepository = companyrepository;
            _hODRepository = hODRepository;
            _GroupRepository    = groupRepository;
            _branchRepository = branchRepository;
        }

        public async Task<BaseResponse> CreateDepartment(CreateDepartmentDto DepartmentDto, RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;
                    }
                }
                if (Convert.ToInt32(RoleId) != 1)
                {
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }


                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(DepartmentDto.DepartmentName)  ||
                    String.IsNullOrEmpty(DepartmentDto.Email) )
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var isExistsComp = await _companyrepository.GetCompanyById(DepartmentDto.CompanyId);
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
                        response.ResponseMessage = $"The Company suplied is already deleted, departments cannot be created under it.";
                        return response;
                    }
                }

                //DepartmentDto.DepartmentName = $"{DepartmentDto.DepartmentName} ({isExistsComp.CompanyName})";

                var isExists = await _departmentrepository.GetDepartmentByName(DepartmentDto.DepartmentName);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Department with name : {DepartmentDto.DepartmentName} already exists.";
                    return response;
                }

                dynamic resp = await _departmentrepository.CreateDepartment(DepartmentDto, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var Department = await _departmentrepository.GetDepartmentByName(DepartmentDto.DepartmentName);

                    response.Data = Department;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Department created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating Department. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateDepartment ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateDepartmentBulkUpload(IFormFile payload, RequesterInfo requester)
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

                        string DepartmentName = serviceDetails.Rows[0][0].ToString();
                        string HodName = serviceDetails.Rows[0][1].ToString();
                        string GroupName = serviceDetails.Rows[0][2].ToString();
                        string BranchName = serviceDetails.Rows[0][3].ToString();
                        string Email = serviceDetails.Rows[0][4].ToString();
                        string CompanyName = serviceDetails.Rows[0][5].ToString();


                        if (DepartmentName != "DepartmentName" || HodName != "HodName"
                        || GroupName != "GroupName" || BranchName != "BranchName" || Email != "Email" || CompanyName != "CompanyName")
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                string departmentName = serviceDetails.Rows[row][0].ToString();
                                var hod = await _hODRepository.GetHODByName(serviceDetails.Rows[row][1].ToString());
                                var group = await _GroupRepository.GetGroupByName(serviceDetails.Rows[row][2].ToString());
                                var branch = await _branchRepository.GetBranchByName(serviceDetails.Rows[row][3].ToString());
                                var email =  serviceDetails.Rows[row][4].ToString();
                                var company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][5].ToString());

                                long hodID = hod.HodID;
                                long groupID = group.GroupID;
                                long branchID = branch.BranchID;
                                long companyID = company.CompanyId;


                                var departmentrequest = new CreateDepartmentDto
                                {
                                    DepartmentName = departmentName,
                                    HodID = hodID,
                                    GroupID = groupID,
                                    BranchID = branchID,
                                    Email = email,
                                    CompanyId = companyID,

                                };

                                var departmentrequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateDepartment(departmentrequest, departmentrequester);


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



        public async Task<BaseResponse> UpdateDepartment(UpdateDepartmentDto updateDto, RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(updateDto.DepartmentName)  || updateDto.CompanyId <= 0 
                    || updateDto.DeptId <= 0 || String.IsNullOrEmpty(updateDto.Email))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var Department = await _departmentrepository.GetDepartmentById(updateDto.DeptId);
                if (null == Department)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Department";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _departmentrepository.UpdateDepartment(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedDepartment = await _departmentrepository.GetDepartmentById(updateDto.DeptId);

                    _logger.LogInformation("Department updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Department updated successfully.";
                    response.Data = updatedDepartment;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Department.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateDepartmentDto ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateDepartmentDto ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteDepartment(DeleteDepartmentDto deleteDto, RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                if (deleteDto.DeptId == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default Department cannot be deleted.";
                    return response;
                }

                var Department = await _departmentrepository.GetDepartmentById(deleteDto.DeptId);
                if (null != Department)
                {
                    dynamic resp = await _departmentrepository.DeleteDepartment(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedDepartment = await _departmentrepository.GetDepartmentById(deleteDto.DeptId);

                        _logger.LogInformation($"Department with name: {DeletedDepartment.DepartmentName} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Department with name: {DeletedDepartment.DepartmentName} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Department.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Departmentname";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteDepartment ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteDepartment ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveDepartments(RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                //update action performed into audit log here

                var Departments = await _departmentrepository.GetAllActiveDepartments();

                if (Departments.Any())
                {
                    response.Data = Departments;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Departments fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Departments found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveDepartments() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveDepartments() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllDepartments(RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                //update action performed into audit log here

                var Departments = await _departmentrepository.GetAllDepartments();

                if (Departments.Any())
                {
                    response.Data = Departments;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Departments fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Departments found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllDepartments() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllDepartments() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetDepartmentbyId(long DepartmentId, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                var Department = await _departmentrepository.GetDepartmentById(DepartmentId);

                if (Department == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Department not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Department;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Department fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetDepartmentbyId(long DepartmentId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetDepartmentbyId(long DepartmentId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetDepartmentbyCompanyId(long companyId, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 1)
                {
                    if (Convert.ToInt32(RoleId) != 2)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;

                        }

                    }
                }

                var Dept = await _departmentrepository.GetAllDepartmentsbyCompanyId(companyId);

                if (Dept == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Branch not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Dept;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Branch fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllDepartmentsbyCompanyId(companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetBranchbyCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
