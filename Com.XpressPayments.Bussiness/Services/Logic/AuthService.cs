using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
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
        private int MaxNumberOfFailedAttemptsToLogin;
        private int MinutesBeforeResetAfterFailedAttemptsToLogin;
        private int CharacterLengthMax;
        private int CharacterLengthMin;
        private int MustContainUppercase;
        private int MustContainLowercase;
        private int MustContainNumber;

        public AuthService(ITokenRefresher tokenRefresher, IUnitOfWork unitOfWork, IConfiguration configuration,
             IAuditLog audit, IMapper mapper, IJwtManager jwtManager, IWebHostEnvironment hostEnvironment,
             IAccountRepository accountRepository, ILogger<AuthService> logger)
        {
            _tokenRefresher = tokenRefresher;
            _unitOfWork = unitOfWork;
            _audit = audit;
            _mapper = mapper;
            _jwtManager = jwtManager;
            _logger = logger;
            _accountRepository = accountRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<LoginResponse> Login(LoginModel login, string ipAddress, string port)
        {
            var response = new LoginResponse();
            try
            {
                //var Un = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(login.Email)));
                //var pw = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(login.Password)));

                var email = Encoding.UTF8.GetString(Convert.FromBase64String(login.Email));
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));

                var hashPassword = Utils.HashPassword(password);

                var decodedLogin = new LoginModel { Email = email, Password = hashPassword };

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
                        await _unitOfWork.UpdateLastLoginAttempt(attemptCount, user.Email);

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

                await _unitOfWork.UpdateLogout(Encoding.UTF8.GetString(Convert.FromBase64String(logout.Email)));

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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //validate CreateUserDto payload here 
                if (String.IsNullOrEmpty(userDto.FirstName) || String.IsNullOrEmpty(userDto.LastName) ||
                    String.IsNullOrEmpty(userDto.Email) || String.IsNullOrEmpty(userDto.PhoneNumber) ||
                    userDto.RoleId <= 0 || userDto.RoleId > 3 || userDto.CompanyId <= 0  || userDto.DepartmentId <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                if ( requester.RoleId == 2 & userDto.RoleId <= 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your user role is not authorized to create a new user with the selected role.";
                    return response;
                }

                var isExists = await _accountRepository.FindUser(userDto.Email);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User with email already exists.";
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                
                if (String.IsNullOrEmpty(updateDto.FirstName) || String.IsNullOrEmpty(updateDto.LastName) ||
                    String.IsNullOrEmpty(updateDto.Email) || String.IsNullOrEmpty(updateDto.PhoneNumber) ||
                    updateDto.RoleId <= 0 || updateDto.RoleId > 3 || updateDto.CompanyId <= 0 || updateDto.DeptId <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                if (requester.RoleId == 2 & updateDto.RoleId <= 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"You user role is not authorized to edit this user with the slected role.";
                    return response;
                }


                var user = await _accountRepository.FindUser(updateDto.Email);
                if (null != user)
                {
                    dynamic resp = await _accountRepository.UpdateUser(updateDto, Convert.ToInt32(requesterUserId), requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var updatedUser = await _accountRepository.FindUser(updateDto.Email);
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
            string email = Encoding.UTF8.GetString(Convert.FromBase64String(changePassword.Email));
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

        public async Task<BaseResponse> GetAllUsersPendingApproval(RequesterInfo requester)
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

                var mappeduser = new List<UserViewModel>();
                var users = await _accountRepository.GetUsersPendingApproval();
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

                var user = await _accountRepository.FindUser(approveUser.Email);
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
                    dynamic resp = await _accountRepository.ApproveUser(Convert.ToInt32(requesterUserId), defaultPass, user.Email);

                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} approved successfully.");
                        _accountRepository.SendEmail(user.Email, user.FirstName, defaultPass, "Activation Email", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} approved successfully.";
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

                var user = await _accountRepository.FindUser(disapproveUser.Email);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot disapprove this User because User was created by you.";
                        return response;
                    }
                    dynamic resp = await _accountRepository.DeclineUser(Convert.ToInt32(requesterUserId), user.Email, disapproveUser.DisapprovedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} disapproved successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} disapproved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while disapproving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}";
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


                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(deactivateUser.Email);
                if (user != null)
                {
                    dynamic resp = await _accountRepository.DeactivateUser(Convert.ToInt32(requesterUserId), user.Email, deactivateUser.DeactivatedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} deactivated successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} deactivated successfully.";
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(reactivateUser.Email);
                if (user != null)
                {
                    string defaultPass = Utils.RandomPassword();
                    dynamic resp = await _accountRepository.ReactivateUser(Convert.ToInt32(requesterUserId), user.Email, reactivateUser.ReactivatedComment, defaultPass);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} reactivated successfully.");
                        _accountRepository.SendEmail(user.Email, user.FirstName, defaultPass, "Re-Activation Email", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} reactivated successfully.";
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

                var user = await _accountRepository.FindUser(unblockUser.Email);
                if (user != null)
                {
                    string defaultPass = Utils.RandomPassword();
                    dynamic resp = await _accountRepository.UnblockUser(Convert.ToInt32(requesterUserId), defaultPass, user.Email);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} unblocked successfully.");
                        _accountRepository.SendEmail(user.Email, user.FirstName, defaultPass, "Unblock Account", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"A link has been sent to User with email: {user.Email} to reset password";
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

    }
}
