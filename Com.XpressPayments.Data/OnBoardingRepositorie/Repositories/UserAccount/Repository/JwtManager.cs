using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class JwtManager : IJwtManager
    {
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration, IRefreshTokenGenerator refreshTokenGenerator, IUnitOfWork unitOfWork)
        {
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponse> GenerateJsonWebToken(User request)
        {
            string Key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];
            string audience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, request.Email),
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Sid, request.UserId.ToString()),
                new Claim(ClaimTypes.Role, request.RoleId.ToString())            };

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);



            string encodedToken = tokenHandler.WriteToken(token);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _unitOfWork.UpdateRefreshToken(request.UserId, request.Email, refreshToken);


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
