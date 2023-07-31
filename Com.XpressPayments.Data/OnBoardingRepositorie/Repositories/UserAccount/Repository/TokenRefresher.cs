﻿using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.UserAccount.Repository
{
    public class TokenRefresher : ITokenRefresher
    {
        private readonly IJwtManager _jwtManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public TokenRefresher(IConfiguration configuration, IJwtManager jwtManager, IAccountRepository accountRepository)
        {
            _jwtManager = jwtManager;
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        public async Task<AuthResponse> Refresh(RefreshTokenModel request)
        {
            //Note: request.email input above is normal email and not base64 string

            string _key = _configuration["Jwt:Key"];
            var authResponse = new AuthResponse();
            var principal = new ClaimsPrincipal();
            SecurityToken validatedToken;
            JwtSecurityToken jwtToken = new JwtSecurityToken();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                principal = tokenHandler.ValidateToken(request.JwtToken, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
                }, out validatedToken);

                jwtToken = validatedToken as JwtSecurityToken;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The token is expired"))
                {
                    if (ex.Message.Contains("The token is expired"))
                    {
                        var rdetails = await _accountRepository.FindUser(request.Email);

                        if (request.RefreshToken != rdetails.RefreshToken)
                        {
                            return new AuthResponse { JwtToken = "", RefreshToken = "", Message = "Invalid Token or Refresh Token was detected" };
                        }

                        return await _jwtManager.RefreshJsonWebToken(rdetails.UserId, principal.Claims.ToArray());
                    }
                }
            }



            if (jwtToken == null || jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token passed");
            }

            var userEmailFromClaim = principal.Identity.Name;
            if (string.IsNullOrWhiteSpace(userEmailFromClaim))
                throw new SecurityTokenException("Invalid token passed");

            var details = await _accountRepository.FindUser(userEmailFromClaim);

            if (details == null)
                return new AuthResponse { JwtToken = "", RefreshToken = "", Message = "Invalid Token passed" };

            if (request.RefreshToken != details.RefreshToken)
            {
                return new AuthResponse { JwtToken = "", RefreshToken = "", Message = "Invalid Token or Refresh Token was detected" };
            }

            return await _jwtManager.RefreshJsonWebToken(details.UserId, principal.Claims.ToArray());
        }


    }
}
