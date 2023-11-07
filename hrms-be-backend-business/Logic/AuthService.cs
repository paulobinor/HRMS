using AutoMapper;
using ExcelDataReader.Log;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
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
        private readonly PassowordConfig _passwordConfig;
        private readonly JwtConfig _jwt;

        public AuthService(ITokenRefresher tokenRefresher, IUnitOfWork unitOfWork, IOptions<PassowordConfig> passwordConfig, IOptions<JwtConfig> jwt,
             IAuditLog audit, IMapper mapper, IJwtManager jwtManager, IHostingEnvironment hostEnvironment,
             IAccountRepository accountRepository, ILogger<AuthService> logger, ICompanyRepository companyRepository, IMailService mailService)
        {
            _passwordConfig = passwordConfig.Value;
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
            _mailService = mailService;
        }

        public async Task<ExecutedResult<LoginResponse>> Login(LoginModel login, string ipAddress, string port)
        {
            var response = new LoginResponse();
            try
            {

                var email = Encoding.UTF8.GetString(Convert.FromBase64String(login.OfficialMail));
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));
                            

                var repoResponse = await _accountRepository.AuthenticateUser(email, _passwordConfig.MaxNumberOfFailedAttemptsToLogin, DateTime.Now);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<LoginResponse>() { responseMessage = repoResponse, responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var userId = repoResponse.Replace("Success", "");
                var user = await _accountRepository.FindUser(Convert.ToInt64(userId), null, null);

                var isPasswordMatch = Utils.DoesPasswordMatch(user.PasswordHash, Encoding.UTF8.GetString(Convert.FromBase64String(login.Password)));
                if (!isPasswordMatch)
                {
                    var attemptCount = user.LoginFailedAttemptsCount + 1;
                    await _accountRepository.UpdateLastLoginAttempt(attemptCount, user.OfficialMail);

                    if (attemptCount >= _passwordConfig.MaxNumberOfFailedAttemptsToLogin)
                    {
                        return new ExecutedResult<LoginResponse>() { responseMessage = $"You have exceeded number of attempts. your account has been locked. Please contact admin.", responseCode = ResponseCode.NotAuthenticated.ToString("D").PadLeft(2, '0'), data = null };
                    }
                    return new ExecutedResult<LoginResponse>()
                    {
                        responseMessage = $"Invalid Password! You have made {attemptCount} unsuccessful attempt(s). " +
                                               $"The maximum retry attempts allowed is {_passwordConfig.MaxNumberOfFailedAttemptsToLogin}. " +
                                               $"If {_passwordConfig.MaxNumberOfFailedAttemptsToLogin} is exceeded, then you will be locked out of the system",
                        responseCode = ResponseCode.NotAuthenticated.ToString("D").PadLeft(2, '0'),
                        data = null
                    };

                }
                var modules = await _accountRepository.GetAppModulesAssigned(Convert.ToInt64(userId));
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
                    UserStatusCode = user.UserStatusCode,
                    Modules = modules,
                    CompanyId=user.CompanyId,
                };


                var authResponse = await _jwtManager.GenerateJsonWebToken(accessUserVm);

                await _accountRepository.UpdateLoginActivity(user.UserId, ipAddress, authResponse.JwtToken,DateTime.Now);

                var loginResponse = new LoginResponse
                {
                    JwtToken = authResponse.JwtToken,
                    RefreshToken = authResponse.RefreshToken,
                };

                var auditLog = new AuditLogDto
                {
                    userId = user.UserId,
                    actionPerformed = "Login",
                    payload = JsonConvert.SerializeObject(login),
                    response = null,
                    actionStatus = $"Successful:",
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<LoginResponse>() { responseMessage = $"User Logged in Successfully.", responseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), data = loginResponse };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (Login)=====>{ex}");
                return new ExecutedResult<LoginResponse>() { responseMessage = $"Unable to process the operation, kindly contact the support", responseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'), data = null };
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
                var userData = await _accountRepository.FindUser(null, null, AccessToken);
                if (userData == null)
                {
                    _logger.LogError($"AuthService || (GetUserById)  Unable to get user details =====>");
                    return new ExecutedResult<UserFullView>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                }

                return new ExecutedResult<UserFullView>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = userData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (CheckUserAccess)=====>{ex}");
                return new ExecutedResult<UserFullView>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<BaseResponse> Logout(LogoutDto payload, string ipAddress, string port)
        {
            var response = new BaseResponse();
            try
            {
                var email = Encoding.UTF8.GetString(Convert.FromBase64String(payload.OfficialMail));
                var repoRespon = await _accountRepository.LogoutUser(email);
                if (!repoRespon.Contains("Success"))
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = repoRespon;
                    response.Data = null;
                    return response;
                }
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

        public async Task<ExecutedResult<string>> ChangeDefaultPassword(ChangeDefaultPasswordDto payload, string ipAddress, string port)
        {           
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.token))
                {
                    isModelStateValidate = false;
                    validationMessage += "Token is required";
                }
                if (payload.NewPassword == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || New Password is required";
                }               

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(payload.NewPassword));
                if (password.Length < _passwordConfig.CharacterLengthMin)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must be greater than {_passwordConfig.CharacterLengthMin}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                if (password.Length > _passwordConfig.CharacterLengthMax)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must be greater than {_passwordConfig.CharacterLengthMin} and less than {_passwordConfig.CharacterLengthMax}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                var stringEvaluator=StringEvaluator.EvaluateString(password);
                if (stringEvaluator.UpperCaseTotal < _passwordConfig.MustContainUppercase)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainUppercase} upper case", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }              
                if (stringEvaluator.LowerCaseTotal < _passwordConfig.MustContainLowercase)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainUppercase} lower case", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                if (stringEvaluator.SpecialCharacterNumber < _passwordConfig.MustContainSpecialCharacter)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainSpecialCharacter} special character", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                if (stringEvaluator.NumberTotal < _passwordConfig.MustContainNumber)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainNumber} number character", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                             
               // string decryptedToken= EncryptDecrypt.DecryptResult(payload.token); 
                string userId = payload.token.Substring(20);              
                var userDetails =await _accountRepository.GetUserById(Convert.ToInt64(userId));
                var repoResponse = await _accountRepository.ChangePassword(userDetails.UserId, password, userDetails.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = repoResponse, responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
               

                var auditLog = new AuditLogDto
                {
                    userId = userDetails.UserId,
                    actionPerformed = "Login",                   
                    actionStatus = $"Successful:",
                    ipAddress = ipAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = $"Your password has been change successfully.", responseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), data = "Success" };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (ChangeDefaultPassword)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = $"Unable to process the operation, kindly contact the support", responseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'), data = null };
            }
        }
        public async Task<ExecutedResult<string>> ChangePassword(ChangePasswordDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.OldPassword))
                {
                    isModelStateValidate = false;
                    validationMessage += "Old Password is required";
                }
                if (payload.NewPassword == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || New Password is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                if (payload.NewPassword.Length < _passwordConfig.CharacterLengthMin)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must be greater than {_passwordConfig.CharacterLengthMin}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                if (payload.NewPassword.Length > _passwordConfig.CharacterLengthMax)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must be greater than {_passwordConfig.CharacterLengthMin} and less than {_passwordConfig.CharacterLengthMax}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                var stringEvaluator = StringEvaluator.EvaluateString(payload.NewPassword);
                if (stringEvaluator.UpperCaseTotal < _passwordConfig.MustContainUppercase)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainUppercase} upper case", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                if (stringEvaluator.LowerCaseTotal < _passwordConfig.MustContainLowercase)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainUppercase} lower case", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                if (stringEvaluator.SpecialCharacterNumber < _passwordConfig.MustContainSpecialCharacter)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainSpecialCharacter} special character", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }
                if (stringEvaluator.NumberTotal < _passwordConfig.MustContainNumber)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Invalid password, password must contain more than {_passwordConfig.MustContainNumber} number character", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };
                }

                var newPassword = Encoding.UTF8.GetString(Convert.FromBase64String(payload.NewPassword));
                string oldPassword = Encoding.UTF8.GetString(Convert.FromBase64String(payload.OldPassword));

                var isPasswordMatch = Utils.DoesPasswordMatch(accessUser.data
                    .PasswordHash, Encoding.UTF8.GetString(Convert.FromBase64String(payload.OldPassword)));
                if (!isPasswordMatch)
                {
                    return new ExecutedResult<string>() { responseMessage = "Invalid Old Password", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var userDetails = await _accountRepository.GetUserById(Convert.ToInt64(accessUser.data.UserId));
                var repoResponse = await _accountRepository.ChangePassword(userDetails.UserId, payload.NewPassword, userDetails.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = repoResponse, responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }


                var auditLog = new AuditLogDto
                {
                    userId = userDetails.UserId,
                    actionPerformed = "Login",
                    actionStatus = $"Successful:",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = $"Your password has been change successfully.", responseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), data = "Success" };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (ChangeDefaultPassword)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = $"Unable to process the operation, kindly contact the support", responseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'), data = null };
            }
        }     
    }
}
