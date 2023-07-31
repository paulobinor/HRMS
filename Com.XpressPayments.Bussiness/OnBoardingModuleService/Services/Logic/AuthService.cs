using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories;
using Com.XpressPayments.Data.Repositories.Branch;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Departments.IRepository;
using Com.XpressPayments.Data.Repositories.EmployeeType;
using Com.XpressPayments.Data.Repositories.EmploymentStatus;
using Com.XpressPayments.Data.Repositories.Grade;
using Com.XpressPayments.Data.Repositories.Group;
using Com.XpressPayments.Data.Repositories.HMO;
using Com.XpressPayments.Data.Repositories.HOD;
using Com.XpressPayments.Data.Repositories.JobDescription;
using Com.XpressPayments.Data.Repositories.Position;
using Com.XpressPayments.Data.Repositories.Unit;
using Com.XpressPayments.Data.Repositories.UnitHead;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
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
        private readonly IWebHostEnvironment _hostEnvironment;
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
        private int MaxNumberOfFailedAttemptsToLogin;
        private int MinutesBeforeResetAfterFailedAttemptsToLogin;
        private int CharacterLengthMax;
        private int CharacterLengthMin;
        private int MustContainUppercase;
        private int MustContainLowercase;
        private int MustContainNumber;
      

        public AuthService(ITokenRefresher tokenRefresher, IUnitOfWork unitOfWork, IConfiguration configuration,
             IAuditLog audit, IMapper mapper, IJwtManager jwtManager, IWebHostEnvironment hostEnvironment,
             IAccountRepository accountRepository, ILogger<AuthService> logger, IUnitRepository unitRepository, IUnitHeadRepository unitHeadRepository,
             IHODRepository HODRepository, IGradeRepository GradeRepository, IEmployeeTypeRepository EmployeeTypeRepository, IPositionRepository PositionRepository,
                IBranchRepository branchRepository, IEmploymentStatusRepository EmploymentStatusRepository, IGroupRepository groupRepository,
                IJobDescriptionRepository jobDescriptionRepository, IDepartmentRepository departmentrepository, ICompanyRepository companyRepository, IRolesRepo rolesRepo)
        {
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
            _GradeRepository    = GradeRepository;
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
                //var Un = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(login.Email)));
                //var pw = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(login.Password)));

                var email = Encoding.UTF8.GetString(Convert.FromBase64String(login.OfficialMail));
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));

                var hashPassword = Utils.HashPassword(password);

                var decodedLogin = new LoginModel { OfficialMail = email, Password = hashPassword };

                var payload = JsonSerializer.Serialize(decodedLogin);

                var user = await _accountRepository.FindUser(email);

                if (user == null)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Invalid login redentials";
                    return response;
                }

                if (user.IsLogin && user.LoggedInWithIPAddress == ipAddress)
                {
                    response.ResponseCode = ResponseCode.InvalidPassword.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"You are already logged in on this PC.";
                    return response;
                }

                if (user.IsLogin && user.LoggedInWithIPAddress != ipAddress)
                {
                    response.ResponseCode = ResponseCode.InvalidPassword.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"You are already logged in on another PC.";
                    return response;
                }

                var systemConfig = await _unitOfWork.SystemConfiguration();

                foreach (var config in systemConfig)
                {
                    if (config.Name.ToLower() == "maxnumberoffailedattemptstologin")
                        MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                }


                if (user != null)
                {
                    if (user.IsDeactivated)
                    {
                        response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You have been deactivated please contact admin";

                        var LogDeactivatedAccount = new AuditLogDto
                        {
                            userId = user.UserId,
                            actionPerformed = "Login",
                            payload = payload,
                            response = JsonSerializer.Serialize(response),
                            actionStatus = $"Unauthorized: {response.ResponseMessage}",
                            ipAddress = ipAddress
                        };

                        await _audit.LogActivity(LogDeactivatedAccount);

                        return response;

                    }

                    if (user.LoginFailedAttemptsCount >= MaxNumberOfFailedAttemptsToLogin
                        && user.LastLoginAttemptAt.HasValue
                        && DateTime.Now < user.LastLoginAttemptAt.Value.AddMinutes(MinutesBeforeResetAfterFailedAttemptsToLogin))
                    {
                        response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You account was blocked, please contact admin";

                        var LogBlockedAccount = new AuditLogDto
                        {
                            userId = user.UserId,
                            actionPerformed = "Login",
                            payload = payload,
                            response = JsonSerializer.Serialize(response),
                            actionStatus = $"Unauthorized: {response.ResponseMessage}",
                            ipAddress = ipAddress
                        };

                        await _audit.LogActivity(LogBlockedAccount);

                        return response;
                    }


                    if (user.IsActive)
                    {
                        var isPasswordMatch = Utils.DoesPasswordMatch(user.PasswordHash, Encoding.UTF8.GetString(Convert.FromBase64String(login.Password)));
                        if (isPasswordMatch)
                        {
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
                                payload = payload,
                                response = JsonSerializer.Serialize(response),
                                actionStatus = $"Successful: {response.ResponseMessage}",
                                ipAddress = ipAddress
                            };
                            await _audit.LogActivity(auditLog);

                            return response;
                        }

                        var attemptCount = user.LoginFailedAttemptsCount + 1;
                        await _unitOfWork.UpdateLastLoginAttempt(attemptCount, user.officialMail);

                        if (attemptCount >= MaxNumberOfFailedAttemptsToLogin)
                        {
                            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"You have exceeded number of attempts. your account has been locked. Please contact admin.";

                            var LogLockedAccount = new AuditLogDto
                            {
                                userId = user.UserId,
                                actionPerformed = "Login",
                                payload = payload,
                                response = JsonSerializer.Serialize(response),
                                actionStatus = $"Failed: {response.ResponseMessage}",
                                ipAddress = ipAddress
                            };

                            await _audit.LogActivity(LogLockedAccount);

                            return response;
                        }

                        response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Invalid Password! You have made {attemptCount} unsuccessful attempt(s). " +
                                                   $"The maximum retry attempts allowed is {MaxNumberOfFailedAttemptsToLogin}. " +
                                                   $"If {MaxNumberOfFailedAttemptsToLogin} is exceeded, then you will be locked out of the system";

                        var LogInvalidPw = new AuditLogDto
                        {
                            userId = user.UserId,
                            actionPerformed = "Login",
                            payload = payload,
                            response = JsonSerializer.Serialize(response),
                            actionStatus = $"Failed: {response.ResponseMessage}",
                            ipAddress = ipAddress
                        };
                        await _audit.LogActivity(LogInvalidPw);

                        return response;
                    }
                    response.ResponseCode = ResponseCode.AuthorizationError.ToString("D");
                    response.ResponseMessage = "User is inactive";

                    var LogInactiveUserTrail = new AuditLogDto
                    {
                        userId = user.UserId,
                        actionPerformed = "Login",
                        payload = payload,
                        response = JsonSerializer.Serialize(response),
                        actionStatus = $"Failed: {response.ResponseMessage}",
                        ipAddress = ipAddress
                    };
                    await _audit.LogActivity(LogInactiveUserTrail);


                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: Login ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: Login ==> {ex.Message}";
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

                if ( requester.RoleId == 2 & userDto.RoleId <= 2)
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
                        || BranchName != "BranchName" || EmploymentStatusName != "EmploymentStatusName"  || JobDescriptionName != "JobDescriptionName"
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


                //if (String.IsNullOrEmpty(updateDto.FirstName) || String.IsNullOrEmpty(updateDto.LastName) ||
                //    String.IsNullOrEmpty(updateDto.OfficialMail) || String.IsNullOrEmpty(updateDto.PhoneNumber) ||
                //    updateDto.RoleId <= 0 || updateDto.RoleId > 3 || updateDto.CompanyId <= 0 || updateDto.DeptId <= 0)
                //{
                //    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Please ensure all required fields are entered.";
                //    return response;
                //}

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
                var user = await _accountRepository.FindUser(request.Email);
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

                        _accountRepository.SendEmail(user.Email, user.FirstName, password, "Password Reset", _hostEnvironment.ContentRootPath, "", port);

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
                        MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "characterlengthmax")
                        CharacterLengthMax = config.Value;
                    if (config.Name.ToLower() == "characterlengthmin")
                        CharacterLengthMin = config.Value;
                    if (config.Name.ToLower() == "mustcontainuppercase")
                        MustContainUppercase = config.Value;
                    if (config.Name.ToLower() == "mustcontainlowercase")
                        MustContainLowercase = config.Value;
                    if (config.Name.ToLower() == "mustcontainnumber")
                        MustContainNumber = config.Value;
                }

                if (user.LoginFailedAttemptsCount >= MaxNumberOfFailedAttemptsToLogin)
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
                    var hasMinChars = new Regex(@".{" + CharacterLengthMin + ",}");
                    var hasMaxChars = new Regex(@".{" + CharacterLengthMax + ",}");

                    bool isValidatedMin = hasMinChars.IsMatch(newPassword);
                    bool isValidatedMax = hasMaxChars.IsMatch(newPassword);
                    if (isValidatedMin && !isValidatedMax)
                    {
                        if (MustContainNumber == 1) // true
                        {
                            if (!hasNumber.IsMatch(newPassword))
                            {
                                response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Password must contain a numeric value";
                                return response;
                            }
                        }
                        if (MustContainUppercase == 1) // true
                        {
                            if (!hasUpperChar.IsMatch(newPassword))
                            {
                                response.ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Password must contain upper case character";
                                return response;
                            }
                        }
                        if (MustContainLowercase == 1) // true
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
                        response.ResponseMessage = $"Password must be between {CharacterLengthMin} and {CharacterLengthMax} characters";
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
                        MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                }

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetAllUsers();
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {

                        var usermap = _mapper.Map<UserViewModel>(user);
                        usermap.IsLockedOut = user.LoginFailedAttemptsCount >= MaxNumberOfFailedAttemptsToLogin;
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

      
        public async Task<BaseResponse> GetAllUsersPendingApproval(long CompanyId,RequesterInfo requester)
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
                || roleId == ApplicationConstant.HrAdmin )
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrHead)
            {
                if (roleId == ApplicationConstant.GeneralUser )
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrAdmin)
            {
                if (roleId == ApplicationConstant.GeneralUser )
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
                        MaxNumberOfFailedAttemptsToLogin = config.Value;
                    if (config.Name.ToLower() == "minutesbeforeresetafterfailedattemptstologin")
                        MinutesBeforeResetAfterFailedAttemptsToLogin = config.Value;
                }

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetAllUsersbyDeptId(DepartmentId);
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {

                        var usermap = _mapper.Map<UserViewModel>(user);
                        usermap.IsLockedOut = user.LoginFailedAttemptsCount >= MaxNumberOfFailedAttemptsToLogin;
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
