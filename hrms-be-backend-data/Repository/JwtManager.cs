using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class JwtManager : IJwtManager
    {
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtConfig _jwt;

        public JwtManager(IOptions<JwtConfig> jwt, IConfiguration configuration, IRefreshTokenGenerator refreshTokenGenerator, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _jwt = jwt.Value;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        public async Task<AuthResponse> GenerateJsonWebToken(AccessUserVm request)
        {
            string Key = _jwt.Key;
            string issuer = _jwt.Issuer;
            string audience = _jwt.Audience;

            var tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, request.OfficialMail),
                new Claim(ClaimTypes.SerialNumber, request.UserId.ToString()),
                new Claim(ClaimTypes.Name, request.FullName),
                new Claim(ClaimTypes.Sid, request.UserId.ToString()),
                new Claim(ClaimTypes.Role, request.UserStatusCode),               
                new Claim(ClaimTypes.GivenName, request.FirstName),
                new Claim(ClaimTypes.Surname, request.LastName),
                //new Claim(ClaimTypes.MobilePhone, request.PhoneNumber),               
                new Claim(ClaimTypes.Actor, request.UserStatusName),
                new Claim(ClaimTypes.UserData,JsonConvert.SerializeObject(request.Modules)),
            };

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            string encodedToken = tokenHandler.WriteToken(token);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _accountRepository.UpdateRefreshToken(refreshToken, request.UserId);


            var authResponse = new AuthResponse() { JwtToken = encodedToken, RefreshToken = refreshToken };

            return authResponse;
        }

        public async Task<AuthResponse> RefreshJsonWebToken(long userId, Claim[] claims)
        {
            string Key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];
            string audience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);



            string encodedToken = tokenHandler.WriteToken(token);

            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var Email = "";
            await _unitOfWork.UpdateRefreshToken(userId, Email, refreshToken);

            return new AuthResponse { JwtToken = encodedToken, RefreshToken = refreshToken };
        }

    }
}
