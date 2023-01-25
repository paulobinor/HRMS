using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.AppContants;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Controllers
{
    [Route("api/[controller]")]

    public class LoginController : ControllerBase
    {
        private readonly ISSOservice _iSSOservice;
        private IConfiguration _config;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger, ISSOservice iSSOservice, IConfiguration config)
        {
            _logger = logger;
            _iSSOservice = iSSOservice;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            BaseResponseLogin response = new BaseResponseLogin();
            try
            {
                UserLoginDTO user = new UserLoginDTO();               
                user.Email = Email;
                user.Password = Password;

                var resp = await _iSSOservice.AdminLogin(user);
                if (resp.Data!=null)
                {
                       var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };


                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _config["JWT:ValidIssuer"],
                        audience: _config["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(2),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Login Successfully";
                    response.jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
                    response.Data = resp.Data;                  
                    return Ok(response);


                }
                else
                {
                    response.ResponseCode = "01";
                    response.ResponseMessage = "Invalid Login";
                    response.Data = resp.Data;
                    return Ok(response);

                }
            }
            catch (Exception Ex)
            {
                return Ok(response);
            }
        }





        [HttpPost("CreateAdmin")]

        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminUserLoginDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                //Xpress@Admin
                //Admin@2023
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string email = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    response.Data = null;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Kindly Login";
                    return Ok(response);
                }

                var resp = await _iSSOservice.CreateAdmin(payload, email);
                if (resp != null)
                {
                    response.Data = resp.Data;
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(response);
            }
        }
    }
}
