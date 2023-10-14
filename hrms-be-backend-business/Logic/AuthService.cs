using AutoMapper;
using ExcelDataReader;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static iText.Kernel.Pdf.Colorspace.PdfSpecialCs;

namespace hrms_be_backend_business.Logic
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtManager _jwtManager;
        private readonly ITokenRefresher _tokenRefresher;
        private readonly IAuditLog _audit;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitHeadRepository _unitHeadRepository;
        private readonly IHODRepository _HODRepository;
        private readonly IGradeRepository _GradeRepository;
        private readonly IEmployeeTypeRepository _EmployeeTypeRepository;
        private readonly IPositionRepository _PositionRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IEmploymentStatusRepository _EmploymentStatusRepository;
        private readonly IGroupRepository _GroupRepository;
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly IRolesRepo _rolesRepo;
        private readonly IDepartmentRepository _departmentrepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly AppConfig _appConfig;
        private readonly JwtConfig _jwt;

        public AuthService(ITokenRefresher tokenRefresher, IUnitOfWork unitOfWork, IConfiguration configuration, IOptions<AppConfig> appConfig, IOptions<JwtConfig> jwt,
             IAuditLog audit, IMapper mapper, IJwtManager jwtManager, IHostingEnvironment hostEnvironment,
             IAccountRepository accountRepository, ILogger<AuthService> logger, IUnitRepository unitRepository, IUnitHeadRepository unitHeadRepository,
             IHODRepository HODRepository, IGradeRepository GradeRepository, IEmployeeTypeRepository EmployeeTypeRepository, IPositionRepository PositionRepository,
                IBranchRepository branchRepository, IEmploymentStatusRepository EmploymentStatusRepository, IGroupRepository groupRepository,
                IJobDescriptionRepository jobDescriptionRepository, IDepartmentRepository departmentrepository, ICompanyRepository companyRepository, IRolesRepo rolesRepo)
        {
            _appConfig = appConfig.Value;
            _jwt = jwt.Value;
            _tokenRefresher = tokenRefresher;
            _unitOfWork = unitOfWork;
            _audit = audit;
            _mapper = mapper;
            _jwtManager = jwtManager;
            _logger = logger;
            _accountRepository = accountRepository;
            _hostEnvironment = hostEnvironment;
            _unitRepository = unitRepository;
            _unitHeadRepository = unitHeadRepository;
            _HODRepository = HODRepository;
            _GradeRepository = GradeRepository;
            _EmployeeTypeRepository = EmployeeTypeRepository;
            _PositionRepository = PositionRepository;
            _branchRepository = branchRepository;
            _EmploymentStatusRepository = EmploymentStatusRepository;
            _GroupRepository = groupRepository;
            _rolesRepo = rolesRepo;
            _jobDescriptionRepository = jobDescriptionRepository;
            _departmentrepository = departmentrepository;
            _companyrepository = companyRepository;
        }
        public async Task<LoginResponse> Login(LoginModel login, string ipAddress, string port)
        {
            var response = new LoginResponse();
            try
            {

                var email = Encoding.UTF8.GetString(Convert.FromBase64String(login.OfficialMail));
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));

                var hashPassword = Utils.HashPassword(password);

                var repoResponse = await _accountRepository.AuthenticateUser(email, _appConfig.MaxNumberOfFailedAttemptsToLogin, DateTime.Now);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = ResponseCode.InvalidPassword.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"{repoResponse}";
                    response.Data = null;
                    return response;
                }
                var user = await _accountRepository.FindUser(email);
                if (user.LoginFailedAttemptsCount >= _appConfig.MaxNumberOfFailedAttemptsToLogin
                        && user.LastLoginAttemptAt.HasValue
                        && DateTime.Now < user.LastLoginAttemptAt.Value.AddMinutes(_appConfig.MinutesBeforeResetAfterFailedAttemptsToLogin))
                {
                    response.ResponseCode = ResponseCode.InvalidPassword.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "You account was blocked, please contact admin";

                    return response;
                }

                var isPasswordMatch = Utils.DoesPasswordMatch(user.PasswordHash, Encoding.UTF8.GetString(Convert.FromBase64String(login.Password)));
                if (!isPasswordMatch)
                {
                    var attemptCount = user.LoginFailedAttemptsCount + 1;
                    await _unitOfWork.UpdateLastLoginAttempt(attemptCount, user.officialMail);

                    if (attemptCount >= _appConfig.MaxNumberOfFailedAttemptsToLogin)
                    {
                        response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"You have exceeded number of attempts. your account has been locked. Please contact admin.";
                        return response;
                    }

                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid Password! You have made {attemptCount} unsuccessful attempt(s). " +
                                               $"The maximum retry attempts allowed is {_appConfig.MaxNumberOfFailedAttemptsToLogin}. " +
                                               $"If {_appConfig.MaxNumberOfFailedAttemptsToLogin} is exceeded, then you will be locked out of the system";

                    return response;
                }




                var authResponse = await _jwtManager.GenerateJsonWebToken(user);

                await _unitOfWork.UpdateUserLoginActivity(user.UserId, ipAddress, authResponse.JwtToken);

                var mapped = _mapper.Map<UserViewModel>(user);


                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "User Logged in Successfully";
                response.JwtToken = authResponse.JwtToken;
                response.RefreshToken = authResponse.RefreshToken;
                response.Data = mapped;

                var auditLog = new AuditLogDto
                {
                    userId = user.UserId,
                    actionPerformed = "Login",
                    payload = JsonConvert.SerializeObject(login),
                    response = null,
                    actionStatus = $"Successful: {response.ResponseMessage}",
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AuthService => Login || {ex}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Unable to process the operation, kindly contact the support";
                response.Data = null;
                return response;
            }
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port)
        {
            var response = new RefreshTokenResponse();
            try
            {
                var token = await _tokenRefresher.Refresh(refresh);

                //update action performed into audit log here

                if (token == null)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Invalid refresh token";
                    return response;
                }

                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Token refreshed successfully";
                response.JwtToken = token.JwtToken;
                response.RefreshToken = token.RefreshToken;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : Refresh ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : Refresh ==> {ex.Message}";
                return response;
            }
        }
        public async Task<ExecutedResult<User>> CheckUserAccess(string AccessToken, string IpAddress)
        {
            try
            {
                //var validationResponse = await AccessTokenValidation.ValidateToken(AccessToken, _jwt.Key, _jwt.Issuer, _jwt.Audience);
                //if (!validationResponse.Identity.IsAuthenticated)
                //{
                //    _logger.LogError($"AuthService || (validationResponse)  Unable to valiate token=====>{validationResponse}");
                //    return new ExecutedResult<User>() { responseMessage = "Unathorized User", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                //}
                //var userIdAuth = claim.Where(x => x.Type == ClaimTypes.Sid).FirstOrDefault();

                var userAccess = await _accountRepository.VerifyUser(AccessToken, IpAddress, DateTime.Now);
                if (!userAccess.Contains("Success"))
                {
                    _logger.LogError($"AuthService || (VerifyUser)  Unable to verify access =====>{userAccess}");
                    return new ExecutedResult<User>() { responseMessage = "Unathorized User", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var userData = await _accountRepository.GetUserByToken(AccessToken);
                if (userData == null)
                {
                    _logger.LogError($"AuthService || (GetUserById)  Unable to get user details =====>");
                    return new ExecutedResult<User>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }
                if (userData.IsDeactivated)
                {
                    _logger.LogError($"AuthService || User has been deactivated");
                    return new ExecutedResult<User>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }
                //if (userData.LoggedInWithIPAddress != IpAddress)
                //{
                //    _logger.LogError($"AuthService || User IP Address does not match");
                //    return new ExecutedResult<User>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                //}

                return new ExecutedResult<User>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = userData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (CreateMerchantUser)=====>{ex}");
                return new ExecutedResult<User>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<BaseResponse> Logout(LogoutDto logout, string ipAddress, string port)
        {
            var response = new BaseResponse();
            try
            {
                //update action performed into audit log here

                await _unitOfWork.UpdateLogout(Encoding.UTF8.GetString(Convert.FromBase64String(logout.OfficialMail)));

                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Logged out successfully";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: Logout ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: Logout ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateUser(CreateUserDto userDto, RequesterInfo requester)
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

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


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

                //validate CreateUserDto payload here 
                if (String.IsNullOrEmpty(userDto.FirstName) || String.IsNullOrEmpty(userDto.LastName) ||
                    String.IsNullOrEmpty(userDto.OfficialMail) || String.IsNullOrEmpty(userDto.PhoneNumber) ||
                    userDto.RoleId <= 0 || userDto.RoleId > 4 || userDto.CompanyId <= 0 || userDto.DepartmentId <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                //if (requester.RoleId == 1 & userDto.DepartmentId != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your user role is not authorized to create a new user with the selected Department.";
                //    return response;
                //}

                if (requester.RoleId == 2 & userDto.RoleId <= 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your user role is not authorized to create a new user with the selected role.";
                    return response;
                }

                //var isExists = await _accountRepository.FindUser(userDto.Email);
                //if (null != isExists)
                //{
                //    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "User with email already exists.";
                //    return response;
                //}

                var isExistsNew = await _accountRepository.GetUserByCompany(userDto.OfficialMail, (int)userDto.CompanyId);
                if (null != isExistsNew)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"User with OfficialMail : {userDto.OfficialMail} already exists for this Company.";
                    return response;
                }

                var resp = await _accountRepository.AddUser(userDto, Convert.ToInt32(createdbyUserId), createdbyUserEmail);

                if (resp != null && resp.IsCreated > 0)
                {
                    //update action performed into audit log here

                    var user = await _accountRepository.FindUser(userDto.Email);

                    response.Data = user;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User created successfully.";
                    return response;
                }

                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating user. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        public async Task<BaseResponse> CreateUserBulkUpload(IFormFile payload, RequesterInfo requester)
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

                        string FirstName = serviceDetails.Rows[0][0].ToString();
                        string MiddleName = serviceDetails.Rows[0][1].ToString();
                        string LastName = serviceDetails.Rows[0][2].ToString();
                        string Email = serviceDetails.Rows[0][3].ToString();
                        string DOB = serviceDetails.Rows[0][4].ToString();
                        string ResumptionDate = serviceDetails.Rows[0][5].ToString();
                        string OfficialMail = serviceDetails.Rows[0][6].ToString();
                        string PhoneNumber = serviceDetails.Rows[0][7].ToString();
                        string UnitName = serviceDetails.Rows[0][8].ToString();
                        string UnitHeadName = serviceDetails.Rows[0][9].ToString();
                        string HODName = serviceDetails.Rows[0][10].ToString();
                        string GradeName = serviceDetails.Rows[0][11].ToString();
                        string EmployeeTypeName = serviceDetails.Rows[0][12].ToString();
                        string PositionName = serviceDetails.Rows[0][13].ToString();
                        string BranchName = serviceDetails.Rows[0][14].ToString();
                        string EmploymentStatusName = serviceDetails.Rows[0][15].ToString();

                        string JobDescriptionName = serviceDetails.Rows[0][17].ToString();
                        string RoleName = serviceDetails.Rows[0][18].ToString();
                        string DepartmentName = serviceDetails.Rows[0][19].ToString();
                        string CompanyName = serviceDetails.Rows[0][20].ToString();


                        if (FirstName != "FirstName" || MiddleName != "MiddleName"
                        || LastName != "LastName" || Email != "Email" || DOB != "DOB" || ResumptionDate != "ResumptionDate"
                        || OfficialMail != "OfficialMail" || PhoneNumber != "PhoneNumber" || UnitName != "UnitName" || UnitHeadName != "UnitHeadName"
                        || HODName != "HODName" || GradeName != "GradeName" || EmployeeTypeName != "EmployeeTypeName" || PositionName != "PositionName"
                        || BranchName != "BranchName" || EmploymentStatusName != "EmploymentStatusName" || JobDescriptionName != "JobDescriptionName"
                        || RoleName != "RoleName" || DepartmentName != "DepartmentName" || CompanyName != "CompanyName")
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                string firstName = serviceDetails.Rows[row][0].ToString();
                                string middleName = serviceDetails.Rows[row][1].ToString();
                                string lastName = serviceDetails.Rows[row][2].ToString();
                                string email = serviceDetails.Rows[row][3].ToString();
                                string dOB = serviceDetails.Rows[row][4].ToString();
                                string resumptionDate = serviceDetails.Rows[row][5].ToString();
                                string officialMail = serviceDetails.Rows[row][6].ToString();
                                string phoneNumber = serviceDetails.Rows[row][7].ToString();
                                var unitName = await _unitRepository.GetUnitByName(serviceDetails.Rows[row][8].ToString());
                                var unitHeadName = await _unitHeadRepository.GetUnitHeadByUnitHeadName(serviceDetails.Rows[row][9].ToString());
                                var hODName = await _HODRepository.GetHODByName(serviceDetails.Rows[row][10].ToString());
                                var gradeName = await _GradeRepository.GetGradeByName(serviceDetails.Rows[row][11].ToString());
                                var employeeTypeName = await _EmployeeTypeRepository.GetEmployeeTypeByName(serviceDetails.Rows[row][12].ToString());
                                var positionName = await _PositionRepository.GetPositionByName(serviceDetails.Rows[row][13].ToString());
                                var branchName = await _branchRepository.GetBranchByName(serviceDetails.Rows[row][14].ToString());
                                var employmentStatusName = await _EmploymentStatusRepository.GetEmpLoymentStatusByName(serviceDetails.Rows[row][15].ToString());

                                var jobDescriptionName = await _jobDescriptionRepository.GetJobDescriptionByName(serviceDetails.Rows[row][17].ToString());
                                var roleName = await _rolesRepo.GetRolesByName(serviceDetails.Rows[row][18].ToString());
                                var departmentName = await _departmentrepository.GetDepartmentByName(serviceDetails.Rows[row][19].ToString());
                                var companyName = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][20].ToString());

                                long unitID = unitName.UnitID;
                                long unitHeadID = unitHeadName.UnitHeadID;
                                long hODID = hODName.HodID;
                                long gradeID = gradeName.GradeID;
                                long employeeTypeID = employeeTypeName.EmployeeTypeID;
                                long positionID = positionName.PositionID;
                                long branchID = branchName.BranchID;
                                long employmentStatusID = employmentStatusName.EmploymentStatusID;

                                long jobDescriptionID = jobDescriptionName.JobDescriptionID;
                                long roleID = roleName.RoleId;
                                long departmentID = departmentName.DeptId;
                                long companyID = companyName.CompanyId;


                                var userrequest = new CreateUserDto
                                {
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    Email = email,
                                    DOB = dOB,
                                    ResumptionDate = resumptionDate,
                                    OfficialMail = OfficialMail,
                                    PhoneNumber = phoneNumber,
                                    UnitID = unitID,

                                    GradeID = gradeID,
                                    EmployeeTypeID = employeeTypeID,

                                    BranchID = branchID,
                                    EmploymentStatusID = employmentStatusID,

                                    JobDescriptionID = jobDescriptionID,
                                    RoleId = roleID,
                                    DepartmentId = departmentID,
                                    CompanyId = companyID,


                                };

                                var userrequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateUser(userrequest, userrequester);


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

        
        public async Task<BaseResponse> CreateUserBulkUploadTwo(IFormFile payload, long companyID, RequesterInfo requester)
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
                        var dataTable = GenerateUserDataTableHeader();
                        var createUserList = new List<CreateUserDto>();
                        string FirstName = serviceDetails.Rows[0][0].ToString();
                        string MiddleName = serviceDetails.Rows[0][1].ToString();
                        string LastName = serviceDetails.Rows[0][2].ToString();
                        string Email = serviceDetails.Rows[0][3].ToString();
                        string DOB = serviceDetails.Rows[0][4].ToString();
                        string ResumptionDate = serviceDetails.Rows[0][5].ToString();
                        string OfficialMail = serviceDetails.Rows[0][6].ToString();
                        string PhoneNumber = serviceDetails.Rows[0][7].ToString();
                        string StaffID = serviceDetails.Rows[0][8].ToString();
                        string UnitName = serviceDetails.Rows[0][9].ToString();
                        string GradeName = serviceDetails.Rows[0][10].ToString();
                        string EmployeeTypeName = serviceDetails.Rows[0][11].ToString();
                        string BranchName = serviceDetails.Rows[0][12].ToString();
                        string EmploymentStatusName = serviceDetails.Rows[0][13].ToString();
                        string RoleName = serviceDetails.Rows[0][14].ToString();
                        string DepartmentName = serviceDetails.Rows[0][15].ToString();
                        string CompanyName = serviceDetails.Rows[0][16].ToString();


                        if (FirstName != "FirstName" || MiddleName != "MiddleName"
                        || LastName != "LastName" || Email != "Email" || DOB != "DOB" || ResumptionDate != "ResumptionDate"
                        || OfficialMail != "OfficialMail" || PhoneNumber != "PhoneNumber" || StaffID != "StaffID" || UnitName != "UnitName" || GradeName != "GradeName" || EmployeeTypeName != "EmployeeTypeName"
                        || BranchName != "BranchName" || EmploymentStatusName != "EmploymentStatusName"
                        || RoleName != "RoleName" || DepartmentName != "DepartmentName" || CompanyName != "CompanyName")
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            var company = await _companyrepository.GetCompanyById(companyID);
                            if (company == null || company.IsDeleted)
                            {
                                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Company not found";
                                return response;
                            }
                            var compnayUnitsTask = _unitRepository.GetAllUnitCompanyId(companyID);
                            var departmentsTask = _departmentrepository.GetAllDepartmentsbyCompanyId(companyID);
                            var unitsTask = _unitRepository.GetAllUnitCompanyId(companyID);
                            var gradsTask = _GradeRepository.GetAllGradeCompanyId(companyID);
                            var employeTypesTask = _EmployeeTypeRepository.GetAllEmployeeTypeCompanyId(companyID);
                            var employeStatusTask = _EmploymentStatusRepository.GetAllEmploymentStatusCompanyId(companyID);
                            var rolesTask = _rolesRepo.GetAllRoles();
                            var branchesTask = _branchRepository.GetAllBranchbyCompanyId(companyID);



                            await Task.WhenAll(compnayUnitsTask,departmentsTask,unitsTask,gradsTask,
                            employeTypesTask,employeStatusTask, rolesTask,branchesTask);

                            // You can access the results directly
                            var companyUnits = compnayUnitsTask.Result;
                            var departments = departmentsTask.Result;
                            var units = unitsTask.Result;
                            var grades = gradsTask.Result;
                            var employeeTypes = employeTypesTask.Result;
                            var employmentStatuses = employeStatusTask.Result;
                            var roles = rolesTask.Result;
                            var branches = branchesTask.Result;
                            string patternEmail = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {
                                long unitID = 0;
                                long gradeID = 0;
                                long employeeTypeID = 0;
                                long branchID = 0;
                                long employmentStatusID = 0;
                                long roleID = 0;
                                long departmentID = 0;

                                string rowError = string.Empty;
                                string firstName = serviceDetails.Rows[row][0].ToString();
                                string middleName = serviceDetails.Rows[row][1].ToString();
                                string lastName = serviceDetails.Rows[row][2].ToString();
                                string email = serviceDetails.Rows[row][3].ToString();
                                string dOB = serviceDetails.Rows[row][4].ToString();
                                string resumptionDate = serviceDetails.Rows[row][5].ToString();
                                string officialMaildata = serviceDetails.Rows[row][6].ToString();
                                string phoneNumber = serviceDetails.Rows[row][7].ToString();
                                string staffID = serviceDetails.Rows[row][8].ToString();
                                var unitName = serviceDetails.Rows[row][9].ToString();
                                var gradeName = serviceDetails.Rows[row][10].ToString();
                                var employeeTypeName = serviceDetails.Rows[row][11].ToString();
                                var branchName = serviceDetails.Rows[row][12].ToString();
                                var employmentStatusName = serviceDetails.Rows[row][13].ToString();
                                var roleName = serviceDetails.Rows[row][14].ToString();
                                var departmentName = serviceDetails.Rows[row][15].ToString();
                                var companyName = serviceDetails.Rows[row][16].ToString();

                                if (string.IsNullOrEmpty(firstName))
                                    rowError = $"{rowError} First name is required.";
                                else if(string.IsNullOrEmpty(lastName))
                                    rowError = $"{rowError} Last name is required.";
                                else if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, patternEmail))
                                    rowError = $"{rowError} Invalid email supplied.";
                                else if (string.IsNullOrEmpty(officialMaildata) || !Regex.IsMatch(officialMaildata, patternEmail))
                                    rowError = $"{rowError} Invalid official supplied.";
                                else if (string.IsNullOrEmpty(phoneNumber))
                                    rowError = $"{rowError} Phone number is required.";
                                else if(string.IsNullOrEmpty(staffID))
                                    rowError = $"{rowError} StaffID is required.";
                                else if (string.IsNullOrEmpty(employeeTypeName))
                                    rowError = $"{rowError} Employee type is required.";
                                else if (string.IsNullOrEmpty(departmentName))
                                    rowError = $"{rowError} Department name is required.";
                                else if (string.IsNullOrEmpty(companyName))
                                    rowError = $"{rowError} Company name is required.";

                                if (!string.IsNullOrEmpty(unitName))
                                {
                                    var unit = units.FirstOrDefault(m => m.UnitName == unitName.Trim());

                                    if (unit == null)
                                        rowError = $"{rowError} Unit {unitName} doesn't exist.";
                                    else
                                        unitID = unit.UnitID;
                                }


                                if (!string.IsNullOrEmpty(gradeName))
                                {
                                    var grade = grades.FirstOrDefault(m => m.GradeName == gradeName.Trim());
                                    if (grade == null)
                                        rowError = $"{rowError} Grade {gradeName} doesn't exist.";
                                    else
                                        gradeID = grade.GradeID;
                                }

                                if (!string.IsNullOrEmpty(employeeTypeName))
                                {
                                    var employeeType = employeeTypes.FirstOrDefault(m => m.EmployeeTypeName == employeeTypeName.Trim());
                                    if (employeeType == null)
                                        rowError = $"{rowError} employee Type {employeeTypeName} doesn't exist.";
                                    else
                                        employeeTypeID = employeeType.EmployeeTypeID;
                                }


                                if (!string.IsNullOrEmpty(branchName))
                                {
                                    var branch = branches.FirstOrDefault(m => m.BranchName == branchName.Trim());
                                    if (branch == null)
                                        rowError = $"{rowError} branch {branchName} doesn't exist.";
                                    else
                                        branchID = branch.BranchID;
                                }

                                if (!string.IsNullOrEmpty(employmentStatusName))
                                {
                                    var employeeStatus = employmentStatuses.FirstOrDefault(m => m.EmploymentStatusName == employmentStatusName.Trim());
                                    if (employeeStatus == null)
                                        rowError = $"{rowError} employee status name {employmentStatusName} doesn't exist.";
                                    else
                                        employmentStatusID = employeeStatus.EmploymentStatusID;
                                }


                                if (!string.IsNullOrEmpty(departmentName))
                                {
                                    var department = departments.FirstOrDefault(m => m.DepartmentName == departmentName.Trim());
                                    if (department == null)
                                        rowError = $"{rowError} department {departmentName} doesn't exist.";
                                    else
                                        departmentID = department.DeptId;
                                }


                                if (!string.IsNullOrEmpty(roleName))
                                {
                                    var role = roles.FirstOrDefault(m => m.RoleName == roleName.Trim());
                                    if (role == null)
                                        rowError = $"{rowError} role {roleName} doesn't exist.";
                                    else
                                        roleID = role.RoleId;
                                }

                                if(companyName.ToUpper().Trim() != company.CompanyName.ToUpper().Trim())
                                    rowError = $"{rowError} Company name {companyName} is different from the selected company.";

                                if (rowError.Length > 0)
                                {
                                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                                    response.ResponseMessage = $"Error on excel row {row} - {rowError}";
                                    return response;
                                }

                                

                                var userrequest = new CreateUserDto
                                {
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    Email = email,
                                    DOB = dOB,
                                    ResumptionDate = resumptionDate,
                                    OfficialMail = officialMaildata,
                                    PhoneNumber = phoneNumber,
                                    UnitID = unitID,
                                    GradeID = gradeID,
                                    EmployeeTypeID = employeeTypeID,
                                    BranchID = branchID,
                                    EmploymentStatusID = employmentStatusID,
                                    RoleId = roleID,
                                    DepartmentId = departmentID,
                                    CompanyId = companyID
                                    
                                };

                                createUserList.Add(userrequest);
                                dataTable.Rows.Add(new object[] { userrequest.FirstName.Trim(), staffID.Trim(), userrequest.MiddleName.Trim(), userrequest.LastName.Trim(), userrequest.Email.Trim(), userrequest.DOB.Trim(), userrequest.ResumptionDate.Trim(), userrequest.OfficialMail.Trim(), userrequest.PhoneNumber.Trim(), userrequest.UnitID, userrequest.GradeID, userrequest.EmployeeTypeID, userrequest.BranchID, userrequest.EmploymentStatusID, userrequest.RoleId, userrequest.DepartmentId, userrequest.CompanyId });

                            }

                            var resp = await _accountRepository.AddUserBulk(dataTable, requester, company.LastStaffNumber, createUserList.Count, companyID);

                            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Users created successfully";
                            response.Data = null;
                            return response;
                        }

                    }

                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Error reading file Or file is empty";
                    response.Data = null;
                    return response;

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

        public async Task<BaseResponse> UpdateUser(UpdateUserDto updateDto, RequesterInfo requester)
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

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

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


                if (requester.RoleId == 2 & updateDto.RoleId <= 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"You user role is not authorized to edit this user with the slected role.";
                    return response;
                }


                var user = await _accountRepository.FindUser(updateDto.OfficialMail);
                if (null != user)
                {
                    dynamic resp = await _accountRepository.UpdateUser(updateDto, Convert.ToInt32(requesterUserId), requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var updatedUser = await _accountRepository.FindUser(updateDto.OfficialMail);
                        var mapped = _mapper.Map<UserViewModel>(updatedUser);

                        _logger.LogInformation("User updated successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "User updated successfully.";
                        response.Data = mapped;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while updating user.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Username";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateUserDto ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateUserDto ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> SendEmailForPasswordChange(RequestPasswordChange request, string ipAddress, string port)
        {
            var response = new BaseResponse();

            try
            {
                var user = await _accountRepository.FindUser(request.officialMail);
                if (null == user)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Invalid email. Please provide a valid email";
                    return response;
                }
                if (user.IsApproved)
                {
                    var password = Utils.RandomPassword();

                    var resp = await _accountRepository.ChangePassword(user.UserId, password);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _accountRepository.SendEmail(user.officialMail, user.FirstName, password, "Password Reset", _hostEnvironment.ContentRootPath, "", port);

                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "A link has been sent to your email to reset your password";
                        return response;
                    }

                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "An error occured. Please contact admin.";
                    return response;
                }

                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "User Account is either not approved yet or inactive. Pls contact System Admin.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : SendEmailForPasswordChange ==> {ex.Message}";
                response.Data = null;
                return response;
            }


        }

        public async Task<BaseResponse> ChangePassword(ChangePasswordViewModel changePassword, string ipAddress, string port)
        {
            var response = new BaseResponse();

            //var npw = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString("Password1234")));
            string email = Encoding.UTF8.GetString(Convert.FromBase64String(changePassword.officialMail));
            string oldPassword = Encoding.UTF8.GetString(Convert.FromBase64String(changePassword.OldPassword));
            string newPassword = Encoding.UTF8.GetString(Convert.FromBase64String(changePassword.NewPassword));

            try
            {
                var user = await _accountRepository.FindUser(email);
                if (null == user)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Invalid email. Please provide a valid email";
                    return response;
                }

                var systemConfig = await _unitOfWork.SystemConfiguration();

                foreach (var config in systemConfig)
                {
                    if (config.Name.ToLower() == "maxnumberoffailedattemptstologin")
                        _appConfig.MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        _appConfig.MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "characterlengthmax")
                        _appConfig.CharacterLengthMax = config.Value;
                    if (config.Name.ToLower() == "characterlengthmin")
                        _appConfig.CharacterLengthMin = config.Value;
                    if (config.Name.ToLower() == "mustcontainuppercase")
                        _appConfig.MustContainUppercase = config.Value;
                    if (config.Name.ToLower() == "mustcontainlowercase")
                        _appConfig.MustContainLowercase = config.Value;
                    if (config.Name.ToLower() == "mustcontainnumber")
                        _appConfig.MustContainNumber = config.Value;
                }

                if (user.LoginFailedAttemptsCount >= _appConfig.MaxNumberOfFailedAttemptsToLogin)
                {
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "You account was blocked, please contact admin";
                    return response;
                }

                var isPasswordMatch = Utils.DoesPasswordMatch(user.PasswordHash, oldPassword);
                if (isPasswordMatch)
                {
                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    var hasLowerChar = new Regex(@"[a-z]+");
                    var hasMinChars = new Regex(@".{" + _appConfig.CharacterLengthMin + ",}");
                    var hasMaxChars = new Regex(@".{" + _appConfig.CharacterLengthMax + ",}");

                    bool isValidatedMin = hasMinChars.IsMatch(newPassword);
                    bool isValidatedMax = hasMaxChars.IsMatch(newPassword);
                    if (isValidatedMin && !isValidatedMax)
                    {
                        if (_appConfig.MustContainNumber == 1) // true
                        {
                            if (!hasNumber.IsMatch(newPassword))
                            {
                                response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Password must contain a numeric value";
                                return response;
                            }
                        }
                        if (_appConfig.MustContainUppercase == 1) // true
                        {
                            if (!hasUpperChar.IsMatch(newPassword))
                            {
                                response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Password must contain upper case character";
                                return response;
                            }
                        }
                        if (_appConfig.MustContainLowercase == 1) // true
                        {
                            if (!hasLowerChar.IsMatch(newPassword))
                            {
                                response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Password must contain lower case character";
                                return response;
                            }
                        }

                        if (!user.IsActive)
                            user.IsActive = true;


                        var resp = await _accountRepository.ChangePassword(user.UserId, newPassword);
                        if (resp > 0)
                        {
                            //update action performed into audit log here

                            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = "Password changed successfully.";
                            return response;
                        }

                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "An error occured while updating your password. Please contact admin.";
                        return response;
                    }
                    else
                    {
                        response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Password must be between {_appConfig.CharacterLengthMin} and {_appConfig.CharacterLengthMax} characters";
                        return response;
                    }
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Old password is not valid";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ChangePassword ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ChangePassword ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> GetAllUsers(RequesterInfo requester)
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

                var systemConfig = await _unitOfWork.SystemConfiguration();

                foreach (var config in systemConfig)
                {
                    if (config.Name.ToLower() == "maxnumberoffailedattemptstologin")
                        _appConfig.MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        _appConfig.MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                }

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetAllUsers();
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {

                        var usermap = _mapper.Map<UserViewModel>(user);
                        usermap.IsLockedOut = user.LoginFailedAttemptsCount >= _appConfig.MaxNumberOfFailedAttemptsToLogin;
                        if (usermap.IsLockedOut)
                        {
                            usermap.UserStatus = "LockedOut";
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.LOCKEDOUT);
                        }
                        else if (usermap.IsDeactivated)
                        {
                            usermap.UserStatus = "Deactivated";
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.DEACTIVATED);
                        }
                        else
                            usermap.UserStatus = user.IsApproved == true ? "Approved" : user.IsApproved == false && user.IsDisapproved == true ? "Disapproved" : "Pending";

                        if (usermap.UserStatus == "Approved")
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.APPROVED);
                        else if (usermap.UserStatus == "Disapproved")
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.DISAPPROVED);
                        else
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.PENDING);

                        mappeduser.Add(usermap);
                    }
                }
                else
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No Users found.";
                    response.Data = null;
                    return response;
                }

                response.Data = mappeduser;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Users fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }


        public async Task<BaseResponse> GetAllUsersPendingApproval(long CompanyId, RequesterInfo requester)
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

                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    if (Convert.ToInt32(RoleId) != 4)
                //    {   
                //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //            return response;

                //    }
                //}

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetUsersPendingApproval(CompanyId);
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {
                        var usermap = _mapper.Map<UserViewModel>(user);
                        usermap.UserStatus = "Pending";
                        usermap.UserStatusId = Convert.ToInt32(UserStatus.PENDING);
                        mappeduser.Add(usermap);
                    }
                }
                else
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No Users found.";
                    response.Data = null;
                    return response;
                }

                response.Data = mappeduser;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Users fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetUsersPendingApproval ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetUsersPendingApproval ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public Tuple<bool, bool> checkPermission(int roleId, int roleId2)
        {
            bool checkCanCreateAndRead = false;
            bool canApprove = false;


            //logically check the role of those that are creating and the created
            if (roleId2 == ApplicationConstant.SuperAdmin)
            {
                if (roleId == ApplicationConstant.HrHead
                || roleId == ApplicationConstant.HrAdmin)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrHead)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrAdmin)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = false;
                }
            }

            return new Tuple<bool, bool>(checkCanCreateAndRead, canApprove);

        }

        public async Task<BaseResponse> ApproveUser(ApproveUserDto approveUser, RequesterInfo requester)
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

                var UserInfo = await _accountRepository.FindUser(approveUser.officialMail);

                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                if (null == UserInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User information cannot be found.";
                    return response;
                }



                Tuple<bool, bool> checkRole = checkPermission(UserInfo.RoleId, requesterInfo.RoleId);


                if (!checkRole.Item2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }


                //if (Convert.ToInt32(RoleId) != 1 || Convert.ToInt32(RoleId) != 4)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                var user = await _accountRepository.FindUser(approveUser.officialMail);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot approve this User because User was created by you.";
                        return response;
                    }
                    //string defaultPass = Utils.RandomPassword();
                    string defaultPass = ApplicationConstant.DefaultPassword;
                    dynamic resp = await _accountRepository.ApproveUser(Convert.ToInt32(requesterUserId), defaultPass, user.officialMail);

                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.officialMail} approved successfully.");
                        _accountRepository.SendEmail(user.officialMail, user.FirstName, defaultPass, "Activation Email", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.officialMail} approved successfully. Activation has been sent to the users email.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while approving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> DisapproveUser(DisapproveUserDto disapproveUser, RequesterInfo requester)
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

                var UserInfo = await _accountRepository.FindUser(disapproveUser.officialMail);

                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                Tuple<bool, bool> checkRole = checkPermission(UserInfo.RoleId, requesterInfo.RoleId);


                if (!checkRole.Item2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 1 || Convert.ToInt32(RoleId) != 4)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                var user = await _accountRepository.FindUser(disapproveUser.officialMail);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot disapprove this User because User was created by you.";
                        return response;
                    }




                    dynamic resp = await _accountRepository.DeclineUser(Convert.ToInt32(requesterUserId), user.officialMail, disapproveUser.DisapprovedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"Employee with email: {user.officialMail} disapproved successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Employee with email: {user.officialMail} disapproved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while disapproving the Employee.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid Employee. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveEmp ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapproveEmp ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> DeactivateUser(DeactivateUserDto deactivateUser, RequesterInfo requester)
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



                //Tuple<bool, bool> checkRole = checkPermission(requesterInfo.RoleId, requesterInfo.RoleId);


                //if (!checkRole.Item2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                if (Convert.ToInt32(RoleId) > 1)
                {
                    if (Convert.ToInt32(RoleId) > 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;
                    }

                }

                var user = await _accountRepository.FindUser(deactivateUser.OfficialMail);
                if (user != null)
                {
                    dynamic resp = await _accountRepository.DeactivateUser(Convert.ToInt32(requesterUserId), user.officialMail, deactivateUser.DeactivatedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.officialMail} deactivated successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.officialMail} deactivated successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deactivating the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DeactivateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DeactivateUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ReactivateUser(ReactivateUserDto reactivateUser, RequesterInfo requester)
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


                //Tuple<bool, bool> checkRole = checkPermission(requesterInfo.RoleId, requesterInfo.RoleId);


                //if (!checkRole.Item2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                if (Convert.ToInt32(RoleId) > 1)
                {
                    if (Convert.ToInt32(RoleId) > 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;
                    }

                }

                var user = await _accountRepository.FindUser(reactivateUser.OfficialMail);
                if (user != null)
                {
                    string defaultPass = Utils.RandomPassword();
                    dynamic resp = await _accountRepository.ReactivateUser(Convert.ToInt32(requesterUserId), user.officialMail, reactivateUser.ReactivatedComment, defaultPass);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.officialMail} reactivated successfully.");
                        _accountRepository.SendEmail(user.officialMail, user.FirstName, defaultPass, "Re-Activation Email", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.officialMail} reactivated successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while reactivating the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ReactivateUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ReactivateUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> UnblockAccount(UnblockAccountDto unblockUser, RequesterInfo requester)
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

                var user = await _accountRepository.FindUser(unblockUser.OfficialMail);
                if (user != null)
                {
                    string defaultPass = Utils.RandomPassword();
                    dynamic resp = await _accountRepository.UnblockUser(Convert.ToInt32(requesterUserId), defaultPass, user.Email);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.officialMail} unblocked successfully.");
                        _accountRepository.SendEmail(user.officialMail, user.FirstName, defaultPass, "Unblock Account", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"A link has been sent to User with email: {user.officialMail} to reset password";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while unblocking the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : unblockUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : unblockUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllUsersbyDeptId(long DepartmentId, RequesterInfo requester)
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

                var systemConfig = await _unitOfWork.SystemConfiguration();

                foreach (var config in systemConfig)
                {
                    if (config.Name.ToLower() == "maxnumberoffailedattemptstologin")
                        _appConfig.MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        _appConfig.MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                }

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetAllUsersbyDeptId(DepartmentId);
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {

                        var usermap = _mapper.Map<UserViewModel>(user);
                        usermap.IsLockedOut = user.LoginFailedAttemptsCount >= _appConfig.MaxNumberOfFailedAttemptsToLogin;
                        if (usermap.IsLockedOut)
                        {
                            usermap.UserStatus = "LockedOut";
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.LOCKEDOUT);
                        }
                        else if (usermap.IsDeactivated)
                        {
                            usermap.UserStatus = "Deactivated";
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.DEACTIVATED);
                        }
                        else
                            usermap.UserStatus = user.IsApproved == true ? "Approved" : user.IsApproved == false && user.IsDisapproved == true ? "Disapproved" : "Pending";

                        if (usermap.UserStatus == "Approved")
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.APPROVED);
                        else if (usermap.UserStatus == "Disapproved")
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.DISAPPROVED);
                        else
                            usermap.UserStatusId = Convert.ToInt32(UserStatus.PENDING);

                        mappeduser.Add(usermap);
                    }
                }
                else
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No Users found.";
                    response.Data = null;
                    return response;
                }

                response.Data = mappeduser;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Users fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : GetAllUsers ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }


        private DataTable GenerateUserDataTableHeader()
        {
            System.Data.DataTable table = new System.Data.DataTable();

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "FirstName",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "StaffID",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "MiddleName",
                DataType = typeof(string)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "LastName",
                DataType = typeof(string)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "Email",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "DOB",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "ResumptionDate",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "OfficialMail",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "PhoneNumber",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "UnitID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "GradeID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "EmployeeTypeID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "BranchID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "EmploymentStatusID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "RoleId",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "DepartmentId",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "CompanyId",
                DataType = typeof(long)
            });


            return table;
        }

        //public async Task<BaseResponse> GetUserbyRoleId(long RoleId, RequesterInfo requester)
        //{
        //    BaseResponse response = new BaseResponse();

        //    try
        //    {
        //        string requesterUserEmail = requester.Username;
        //        string requesterUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }

        //        if (Convert.ToInt32(RoleId) > 2)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
        //            return response;
        //        }

        //        var Department = await _departmentrepository.GetDepartmentById(DepartmentId);

        //        if (Department == null)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Department not found.";
        //            response.Data = null;
        //            return response;
        //        }

        //        //update action performed into audit log here

        //        response.Data = Department;
        //        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "Department fetched successfully.";
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: GetDepartmentbyId(long DepartmentId) ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: GetDepartmentbyId(long DepartmentId) ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
    }
}
