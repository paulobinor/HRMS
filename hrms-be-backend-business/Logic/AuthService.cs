using AutoMapper;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

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
        private readonly ICompanyRepository _companyrepository;
        private readonly IMailService _mailService;
        private readonly PassowordConfig _appConfig;
        private readonly JwtConfig _jwt;

        public AuthService(ITokenRefresher tokenRefresher, IUnitOfWork unitOfWork, IOptions<PassowordConfig> appConfig, IOptions<JwtConfig> jwt,
             IAuditLog audit, IMapper mapper, IJwtManager jwtManager, IHostingEnvironment hostEnvironment,
             IAccountRepository accountRepository, ILogger<AuthService> logger, ICompanyRepository companyRepository, IMailService mailService)
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
            _companyrepository = companyRepository;
            _mailService= mailService;
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
                var userId = repoResponse.Replace("Success", "");
                var user = await _accountRepository.FindUser(Convert.ToInt64(userId),null,null);
                
                var isPasswordMatch = Utils.DoesPasswordMatch(user.PasswordHash, Encoding.UTF8.GetString(Convert.FromBase64String(login.Password)));
                if (!isPasswordMatch)
                {
                    var attemptCount = user.LoginFailedAttemptsCount + 1;
                    await _unitOfWork.UpdateLastLoginAttempt(attemptCount, user.OfficialMail);

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
                var modules=await _accountRepository.GetAppModulesAssigned(Convert.ToInt64(userId));
                var accessUserVm = new AccessUserVm
                {
                    CompanyName = user.CompanyName,
                    FirstName = user.FirstName,
                    FullName = user.LastName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    OfficialMail = user.OfficialMail,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.UserId,
                    UserStatusName = user.UserStatusName,
                    Modules = modules,
                };


                var authResponse = await _jwtManager.GenerateJsonWebToken(accessUserVm);

                await _unitOfWork.UpdateUserLoginActivity(user.UserId, ipAddress, authResponse.JwtToken);

              


                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "User Logged in Successfully";
                response.JwtToken = authResponse.JwtToken;
                response.RefreshToken = authResponse.RefreshToken;
                response.Data = user;

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
        public async Task<ExecutedResult<UserFullView>> CheckUserAccess(string AccessToken, string IpAddress)
        {
            try
            {               

                var userAccess = await _accountRepository.VerifyUser(AccessToken, IpAddress, DateTime.Now);
                if (!userAccess.Contains("Success"))
                {
                    _logger.LogError($"AuthService || (VerifyUser)  Unable to verify access =====>{userAccess}");
                    return new ExecutedResult<UserFullView>() { responseMessage = "Unathorized User", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var userData = await _accountRepository.FindUser(null,null, AccessToken);
                if (userData == null)
                {
                    _logger.LogError($"AuthService || (GetUserById)  Unable to get user details =====>");
                    return new ExecutedResult<UserFullView>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.AuthorizationError).ToString(), data = null };
                }                         

                return new ExecutedResult<UserFullView>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = userData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (CheckUserAccess)=====>{ex}");
                return new ExecutedResult<UserFullView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
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
              

        public async Task<BaseResponse> SendEmailForPasswordChange(RequestPasswordChange request, string ipAddress, string port)
        {
            var response = new BaseResponse();

            try
            {
                var user = await _accountRepository.FindUser(null, request.officialMail,null);
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
                        
                        _mailService.SendEmail(user.OfficialMail, user.FirstName, password, "Password Reset", _hostEnvironment.ContentRootPath, "", port);

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
                var user = await _accountRepository.FindUser(null, email,null);
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

    }
}
