using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private string _connectionString;
        private readonly ILogger<AccountRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _frontendUrl;
        private readonly string _loginUrl;

        public AccountRepository(IConfiguration configuration, ILogger<AccountRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _frontendUrl = configuration["Frontend:Url"];
            _loginUrl = configuration["Frontend:LoginUrl"];
        }

        public async Task<User> FindUser(string email)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.FETCH);
                    param.Add("@Email", email);

                    var userDetails = await _dapper.QueryFirstOrDefaultAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: FindUser(string email) ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<User> FindUser(long userId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 21);
                    param.Add("@UserIdFind", userId);

                    var userDetails = await _dapper.QueryFirstOrDefaultAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: FindUser(string email) ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<CreateUserResponse> AddUser(CreateUserDto user, int createdbyUserId, string createdbyuseremail)
        {
            try
            {
                var response = new CreateUserResponse();
                //var defaultPassword = Utils.RandomPassword();
                //var passwordHash = BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.ADDUSER);
                   
                    param.Add("@FirstName", user.FirstName == null ? "" : user.FirstName.ToString().Trim());
                    param.Add("@MiddleName", user.MiddleName == null ? "" : user.MiddleName.ToString().Trim());
                    param.Add("@LastName", user.LastName == null ? "" : user.LastName.ToString().Trim());
                    param.Add("@UserEmail", user.Email == null ? "" : user.Email.ToString().Trim());
                    param.Add("@DOB", user.DOB == null ? "" : user.DOB.ToString().Trim());
                    param.Add("@ResumptionDate", user.ResumptionDate == null ? "" : user.ResumptionDate.ToString().Trim());
                    param.Add("@OfficialMail", user.OfficialMail == null ? "" : user.OfficialMail.ToString().Trim());
                    param.Add("@PhoneNumber", user.PhoneNumber == null ? "" : user.PhoneNumber.ToString().Trim());
                    //param.Add("@PasswordHash", passwordHash);
                    param.Add("@UnitID", user.UnitID);
               
                    param.Add("@GradeID", user.GradeID);
                    param.Add("@EmployeeTypeID", user.EmployeeTypeID);
                
                    param.Add("@BranchID", user.BranchID);
                    param.Add("@EmploymentStatusID", user.EmploymentStatusID);
                    param.Add("@GroupID", user.GroupID);
                    param.Add("@JobDescriptionID", user.JobDescriptionID);
                    param.Add("@RoleId", user.RoleId);

                    param.Add("@DeptId", user.DepartmentId);
                    param.Add("@CompanyId", user.CompanyId);

                    //add this parameter to sp for create user
                    param.Add("@CreatedByUserId", createdbyUserId);
                    param.Add("@CreatedByUserEmail", createdbyuseremail);
                    
       

                    response = await _dapper.QueryFirstOrDefaultAsync<CreateUserResponse>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: AddUser(CreateUserDto user) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateUser(UpdateUserDto user, int updatedbyUserId, string updatedbyuseremail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UPDUSER);
                    param.Add("@UserToupdate", user.Email);
                    param.Add("@FirstNameUpdate", user.FirstName == null ? "" : user.FirstName.ToString());
                    param.Add("@MiddleNameUpdate", user.MiddleName == null ? "" : user.MiddleName.ToString());
                    param.Add("@LastNameUpdate", user.LastName == null ? "" : user.LastName.ToString());
                    param.Add("@OfficialMail", user.OfficialMail == null ? "" : user.OfficialMail.ToString().Trim());
                    param.Add("@PhoneNumberUpdate", user.PhoneNumber == null ? "" : user.PhoneNumber.ToString().Trim());
                    param.Add("@DOB", user.Email == null ? "" : user.DOB.ToString().Trim());
                    param.Add("@ResumptionDate", user.ResumptionDate == null ? "" : user.DOB.ToString().Trim());
                    param.Add("@UnitID", user.UnitID);
                   
                    param.Add("@GradeID", user.GradeID);
                    param.Add("@EmployeeTypeID", user.EmployeeTypeID);
                  
                    param.Add("@BranchID", user.BranchID);
                    param.Add("@EmploymentStatusID", user.EmploymentStatusID);
                    param.Add("@GroupID", user.GroupID);
                    param.Add("@JobDescriptionID", user.JobDescriptionID);
                    param.Add("@RoleIdUpdate", user.RoleId);
                    param.Add("@UpdatedByUserId", updatedbyUserId);

                    //add this parameter to sp for update user
                    param.Add("@UpdatedByUserEmail", updatedbyUserId);

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateUser(UpdateUserDto user, int updatedbyUserId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.GETALLUSERS);
                    //param.Add("@OrgIdGet", orgId);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUsers() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllActiveUsers()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 23);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveUsers() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetUsersPendingApproval()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.USERSPENDINGAPPROVAL);
                    //param.Add("@OrgIdGetPending", orgId);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetUsersPendingApproval() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersbyDeptId(long DeptId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.GETALLUSERSBYDEPTID);
                    param.Add("@DeptIdGet", DeptId);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUsersbuDeptId() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersbyCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 22);
                    param.Add("@CompanyIdGet", companyId);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUsersbuDeptId() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersbyRoleID(long RoleId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.GETUSERBYROLEID);
                    param.Add("@RoleId", RoleId);

                    var userDetails = await _dapper.QueryAsync<User>(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllUsersbyRoleID() ===>{ex.Message}");
                throw;
            }
        }


        public async Task<dynamic> ApproveUser(long approvedByuserId, string defaultPass, string userEmail)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultPass, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.APPROVEUSER);
                    param.Add("@PasswordHashApprove", hashdefaultPassword);
                    param.Add("@UserIdApprove", approvedByuserId);
                    param.Add("@UserEmailApprove", userEmail);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveUser(long approvedByuserId, string defaultPass, string userEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeclineUser(long disapprovedByuserId, string userEmail, string comment)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.DISAPPROVEUSER);
                    param.Add("@UserIdDisapprove", disapprovedByuserId);
                    param.Add("@UserEmailDisapprove", userEmail);
                    param.Add("@DisapprovedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeclineUser(long disapprovedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeactivateUser(long deactivatedByuserId, string userEmail, string comment)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.DEACTIVATEUSER);
                    param.Add("@DeactivatedByUserId", deactivatedByuserId);
                    param.Add("@UserEmailDeactivate", userEmail);
                    param.Add("@DeactivatedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeactivateUser(long deactivatedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ReactivateUser(long reactivatedByuserId, string userEmail, string comment, string defaultpass)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultpass, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.REACTIVATEUSER);
                    param.Add("@PasswordHashReactivate", hashdefaultPassword);
                    param.Add("@ReactivatedByUserId", reactivatedByuserId);
                    param.Add("@UserEmailReactivate", userEmail);
                    param.Add("@ReactivatedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ReactivateUser(long reactivatedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UnblockUser(long unblockedByuserId, string defaultPassword, string userEmail)
        {
            try
            {
                var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.UNBLOCKUSER);
                    param.Add("@PasswordHashUnblock", hashdefaultPassword);
                    param.Add("@LastModifiedByUserIdUnblock", unblockedByuserId);
                    param.Add("@UserEmailToUnblock", userEmail);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UnblockUser(long unblocedByuserId, string userEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ChangePassword(long userId, string newPassword)
        {
            try
            {
                var hashNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.CHANGEPASSWORD);
                    param.Add("@PasswordHashModifr", hashNewPassword);
                    param.Add("@UserIdModifr", userId);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ChangePassword(long userId, string newPassword) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> MapUserToDepartment(string email, long deptId, long CompId, int updatedbyUserId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", Account.MAPUSERTOdEPT);
                    param.Add("@UserEmailMap", email);
                    param.Add("@DeptIdMap", deptId);
                    param.Add("@CompanyIdMap", CompId);
                    param.Add("@MapUpdatedByUserId", updatedbyUserId);

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_UserAuthandLogin, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: MapUserToDepartment(string email, long deptId, int updatedbyUserId) ===>{ex.Message}");
                throw;
            }
        }

        public void SendEmail(string recipientEmail, string firtname, string defaultPass, string subject, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {
            string emailAddress = _configuration["Smtp:EmailAddress"];
            string smtpAdress = _configuration["Smtp:Host"];
            int smtpPort = Convert.ToInt16(_configuration["Smtp:Port"]);
            string password = _configuration["Smtp:Password"];
            //string sender = _configuration["Smtp:Sender"];

            //var sampleEmail = "yusufsunkanmi3@gmail.com";

            string message = string.Empty;
            MimeMessage mailBody = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Xpress HRMS", emailAddress);
            mailBody.From.Add(from);

            MailboxAddress to = new MailboxAddress("User", recipientEmail);
            //MailboxAddress to = new MailboxAddress("User", sampleEmail);
            mailBody.To.Add(to);

            mailBody.Subject = subject;

            if (subject.ToLower().Contains("unblock"))
            {
                message = ComposeEmailToUnblockAccount(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("password"))
            {
                message = ComposeEmailForPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("re-activation"))
            {
                message = ComposeEmailForReactivationPasswordChange(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("otp"))
            {
                message = ComposeEmailForOTP(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("participation"))
            {
                message = ComposeEmailForSurveyParticipation(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else if (subject.ToLower().Contains("longer"))
            {
                message = ComposeEmailForNoSurveyParticipation(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }
            else
            {
                message = ComposeSignUpMail(firtname, defaultPass, recipientEmail, wwwRootPath, ip, port, appKey, channel);
            }

            BodyBuilder bodyBuilder = new BodyBuilder();
            _logger.LogInformation($"{subject} for {recipientEmail} with Message==> {message}");

            bodyBuilder.HtmlBody = message;
            mailBody.Body = bodyBuilder.ToMessageBody();

            try
            {
                SmtpClient client = new SmtpClient();
                client.Connect(smtpAdress, smtpPort);
                client.Authenticate(emailAddress, password);
                client.Send(mailBody);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: SendEmail ===>{ex.Message}");
                throw;
            }
        }
        public string ComposeSignUpMail(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SignUp.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>Thanks for registering on our platform. Please click on the link below to activate your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

                return body;
            }
            else
            {
                templatePath = $"{wwwRootPath}/EmailHandler/SignUpWithToken.html";
                message = $"Dear {firstname}," +
                          $"<p>Thanks for registering on our platform. Use the following OTP to complete your Sign Up procedures.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{OTP}", defaultPass);
                body = body.Replace("{MailContent}", message);

                return body;
            }
        }
        public string ComposeEmailForPasswordChange(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>We recieved a password reset request for your account. Please click on the reset password button below to reset your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
        public string ComposeEmailForReactivationPasswordChange(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>Your account has been re-activted. Please click on the reset password button below to reset your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
        public string ComposeEmailForOTP(string firstname, string otp, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SendOtp.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>Your request is now in progress. Your order confirmation OTP is : {otp}.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{otp}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }
        
        public string ComposeEmailToUnblockAccount(string firstname, string defaultPass, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _frontendUrl;
                templatePath = $"{wwwRootPath}/EmailHandler/ResetPassword.html";
                if (appKey == null)
                {
                    qryStr = $"?k={defaultPass}&a={email}";
                }
                else
                {
                    qryStr = $"?k={defaultPass}&a={email}&appkey={appKey}";
                }
                message = $"Dear {firstname}," +
                          $"<p>We recieved request to unblock your account. Please click on the reset password button below to reset your password and unbock your account.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}{qryStr}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }


        public string ComposeEmailForSurveyParticipation(string firstname, string survProcessName, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _loginUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SurveyParticipation.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>You have been added as a participant in Survey Name : {survProcessName}. Kindly login to the survey system to take survey.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{link}", $"{clientUrl}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }

        public string ComposeEmailForNoSurveyParticipation(string firstname, string survProcessName, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        {

            string message = string.Empty;
            string body = string.Empty;
            string templatePath = string.Empty;

            if (null == channel)
            {
                string qryStr = string.Empty;
                string clientUrl = _loginUrl;
                //string clientUrl = ip;
                //string clientUrl = $"http://{ip}:{port}/";
                templatePath = $"{wwwRootPath}/EmailHandler/SurveyParticipationRemoved.html";

                qryStr = $"?k={email}";

                message = $"Dear {firstname}," +
                          $"<p>You are no longer a participant in Survey Name : {survProcessName}. Kindly contact HR for enquiries if any.</p>";

                using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
                {
                    body = reader.ReadToEnd();
                }

                //body = body.Replace("{link}", $"{clientUrl}");
                body = body.Replace("{MailContent}", message);

            }
            return body;
        }

        //public string ComposeEmailForApproval(string firstname, string survProcessName, string email, string wwwRootPath, string ip, string port, string appKey = null, string channel = null)
        //{

        //    string message = string.Empty;
        //    string body = string.Empty;
        //    string templatePath = string.Empty;

        //    if (null == channel)
        //    {
        //        string qryStr = string.Empty;
        //        string clientUrl = _loginUrl;
        //        //string clientUrl = ip;
        //        //string clientUrl = $"http://{ip}:{port}/";
        //        templatePath = $"{wwwRootPath}/EmailHandler/SurveyParticipation.html";

        //        qryStr = $"?k={email}";

        //        message = $"Dear {firstname}," +
        //                  $"<p>You have been added as a participant in Survey Name : {survProcessName}. Kindly login to the survey system to take survey.</p>";

        //        using (StreamReader reader = new StreamReader(Path.Combine(templatePath)))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        body = body.Replace("{link}", $"{clientUrl}");
        //        body = body.Replace("{MailContent}", message);

        //    }
        //    return body;
        //}
    }
}
