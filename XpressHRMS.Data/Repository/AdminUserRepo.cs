using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class AdminUserRepo : IAdminUserRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<AdminUserRepo> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDapperGeneric _dapperr;
        public AdminUserRepo(IConfiguration configuration, ILogger<AdminUserRepo> logger, IDapperGeneric dapperr)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
            _dapperr = dapperr;
        }

        public async Task<dynamic> CreateAdminUser(CreateAdminUserLoginDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@Email", payload.Email /*== null ? "" : payload.Email.ToString().Trim()*/);
                    param.Add("@Password", payload.Password /*== null ? "" : payload.Password.ToString().Trim()*/);
                    param.Add("@CompanyID", payload.CompanyID /*== null ? "" : payload.CompanyID.ToString().Trim()*/);  
                    param.Add("@CreatedBy", payload.CreatedBy /*== null ? "" : payload.CreatedBy.ToString().Trim()*/);
                    param.Add("@RoleName", payload.RoleName);

                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateAdminUser(CreateAdminUserLoginDTO payload) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateAdminUser(UpdateAdminUserLoginDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@AdminUserID", payload.AdminUserID);
                    param.Add("@CompanyID", payload.CompanyID);
                    //param.Add("@Email", payload.Email);
                    param.Add("@RoleName", payload.RoleName);

                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteAdminUser(int CompanyID, int AdminUserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", CompanyID);
                    param.Add("@AdminUserID", AdminUserID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> ActivateAdminUser(int CompanyID, int AdminUserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@CompanyID", CompanyID);
                    param.Add("@AdminUserID", AdminUserID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> DisableAdminUser(int CompanyID, int AdminUserID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@CompanyID", CompanyID);
                    param.Add("@AdminUserID", AdminUserID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<List<GetAllAdminUserLoginDTO>> GetAllAdminUser()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    //param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@Status", 8);
                    var AdminUser = await _dapperr.GetAll<GetAllAdminUserLoginDTO>("Sp_AdminUser", param, commandType: CommandType.StoredProcedure);
                    return AdminUser;
                    //return await _dapper.GetAll<CompanyDTO>("Sp_Company", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<AdminDTO> LoginAdmin(UserLoginDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@Email", payload.Email);
                    param.Add("@Password", payload.Password);
                    //param.Add("@RoleName", payload.RoleName);
                    //param.Add("@DateCreated", payload.DateCreated);
                    var login = await _dapper.QueryFirstAsync<AdminDTO>("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return login;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<AdminDTO>> GetUser(UserLoginDTO payload)
        {
            try
            {
                //IEnumerable<AdminDTO> response = new  List<AdminDTO>();
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", 3);
                    param.Add("@Email", payload.Email.Trim());
                    //param.Add("@Password", payload.Password.Trim());
                    // response = await _dapper.QueryAsync<AdminDTO>("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    //return response;
                    var response = await _dapper.QueryAsync<AdminDTO>("Sp_AdminUser", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }



        //public async Task<dynamic> ChangePassword(long userId, string newPassword)
        //{
        //    try
        //    {
        //        var hashNewPassword = En
        //            //BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt

        //        using (SqlConnection _dapper = new SqlConnection(_connectionString))
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("@Status", Account.CHANGEPASSWORD);
        //            param.Add("@PasswordHashModifr", hashNewPassword);
        //            param.Add("@UserIdModifr", userId);

        //            dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

        //            return resp;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: ChangePassword(long userId, string newPassword) ===>{ex.Message}");
        //        throw;
        //    }
        //}

        //public void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip = null, string port = null, string appKey = null, string channel = null)
        //{
        //    string emailAddress = _configuration["Smtp:EmailAddress"];
        //    string smtpAdress = _configuration["Smtp:Host"];
        //    int smtpPort = Convert.ToInt16(_configuration["Smtp:Port"]);
        //    string password = _configuration["Smtp:Password"];



        //    string message = string.Empty;
        //    MimeMessage mailBody = new MimeMessage();

        //    MailboxAddress from = new MailboxAddress("Ecashier", emailAddress);
        //    mailBody.From.Add(from);

        //    MailboxAddress to = new MailboxAddress("User", recipientEmail);
        //    //MailboxAddress to = new MailboxAddress("User", sampleEmail);
        //    mailBody.To.Add(to);

        //    mailBody.Subject = subject;

        //    if (subject.ToLower().Contains("unblock"))
        //    {
        //        message = ComposeEmailToUnblockAccount(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("password"))
        //    {
        //        message = ComposeEmailForPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("re-activation"))
        //    {
        //        message = ComposeEmailForReactivationPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("otp"))
        //    {
        //        message = ComposeEmailForOTP(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("customer"))
        //    {
        //        message = ComposeCustomerCreatedMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("organization"))
        //    {
        //        message = ComposeOrgCreatedMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else if (subject.ToLower().Contains("failed"))
        //    {
        //        message = ComposeOrgDisapprovedMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }
        //    else
        //    {
        //        message = ComposeSignUpMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
        //    }

        //    BodyBuilder bodyBuilder = new BodyBuilder();
        //    _logger.LogInformation($"{subject} for {recipientEmail} with Message==> {message}");

        //    bodyBuilder.HtmlBody = message;
        //    mailBody.Body = bodyBuilder.ToMessageBody();

        //    try
        //    {
        //        SmtpClient client = new SmtpClient();
        //        client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
        //        //client.Connect("smtp.Office365.com", 587, SecureSocketOptions.StartTls);
        //        client.Connect(smtpAdress, smtpPort, SecureSocketOptions.StartTls);
        //        client.Authenticate(emailAddress, password);
        //        client.Send(mailBody);
        //        client.Disconnect(true);
        //        client.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"MethodName: SendEmail ===>{ex.Message}");
        //        throw;
        //    }
        //}

        //public string ComposeSignUpMail(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        //{
        //    //base 64 encrypt email and password
        //    var pp = Encoding.UTF8.GetBytes(defaultPass);
        //    var qq = Encoding.UTF8.GetBytes(email);

        //    defaultPass = Convert.ToBase64String(pp);
        //    email = Convert.ToBase64String(qq);


        //    string message = string.Empty;
        //    string body = string.Empty;
        //    string templatePath = string.Empty;

        //    if (null == channel)
        //    {
        //        string qryStr = string.Empty;
        //        string clientUrl = _configuration["Smtp:FrontEndUrl"];
        //        //string clientUrl = $"http://{ip}:{port}/";
        //        templatePath = $"{wwwRootPath}/EmailHandler/SignUp.html";
        //        if (appKey == null)
        //        {
        //            qryStr = $"?k={defaultPass}&a={email}";
        //        }
        //        else
        //        {
        //            qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
        //        }
        //        message = $"Dear {firstname}," +
        //                  $"<p>Thanks for registering on our platform. Please click on the link below to activate your account.</p>";

        //        using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        body = body.Replace("{link}", $"{clientUrl}{qryStr}");
        //        body = body.Replace("{MailContent}", message);

        //        return body;
        //    }
        //    else
        //    {
        //        templatePath = $"{wwwRootPath}/EmailHandler/SignUpWithToken.html";
        //        message = $"Dear {firstname}," +
        //                  $"<p>Thanks for registering on our platform. Use the following OTP to complete your Sign Up procedures.</p>";

        //        using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        body = body.Replace("{OTP}", defaultPass);
        //        body = body.Replace("{MailContent}", message);

        //        return body;
        //    }
        //}
        //public string ComposeEmailForPasswordChange(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        //{
        //    //base 64 encrypt email and password
        //    var pp = Encoding.UTF8.GetBytes(defaultPass);
        //    var qq = Encoding.UTF8.GetBytes(email);

        //    defaultPass = Convert.ToBase64String(pp);
        //    email = Convert.ToBase64String(qq);

        //    string message = string.Empty;
        //    string body = string.Empty;
        //    string templatePath = string.Empty;

        //    if (null == channel)
        //    {
        //        string qryStr = string.Empty;
        //        string clientUrl = _configuration["Smtp:FrontEndUrl"];
        //        // string clientUrl = ip;
        //        //string clientUrl = $"http://{ip}:{port}/";
        //        //templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
        //        if (appKey == null)
        //        {
        //            qryStr = $"?k={defaultPass}&a={email}";
        //        }
        //        else
        //        {
        //            qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
        //        }
        //        message = $"Dear {firstname}," +
        //                  $"<p>We recieved a password reset request for your account. Please click on the reset password button below to reset your account.</p>";

        //        using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        body = body.Replace("{link}", $"{clientUrl}{qryStr}");
        //        body = body.Replace("{MailContent}", message);

        //    }
        //    return body;
        //}



    }
}
