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
                var plainTextEmail = System.Text.Encoding.UTF8.GetBytes(Email);
                var plainTextPassword = System.Text.Encoding.UTF8.GetBytes(Password);

                var base64Email = System.Convert.ToBase64String(plainTextEmail);
                var base64Password = System.Convert.ToBase64String(plainTextPassword);

                user.Email = base64Email.ToString();
                user.Password = base64Password.ToString();               

                var resp = await _iSSOservice.Login(user);
                //var resp = "d";
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
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    response.jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
                    response.Data = resp.Data;
                    return Ok(response);


                }
                else
                {
                    return Ok(response);

                }
            }
            catch (Exception Ex)
            {
                return Ok(response);
            }
        }



       

        [HttpPost("LogOut")]

        public async Task<IActionResult> LogOut(string Email, string Password)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                UserLoginDTO user = new UserLoginDTO();
                var plainTextEmail = System.Text.Encoding.UTF8.GetBytes(Email);
                var plainTextPassword = System.Text.Encoding.UTF8.GetBytes(Password);

                var base64Email = System.Convert.ToBase64String(plainTextEmail);
                var base64Password = System.Convert.ToBase64String(plainTextPassword);

                user.Email = base64Email.ToString();
                user.Password = base64Password.ToString();
                var resp = await _iSSOservice.Login(user);
                if (resp != null)
                {
                    response.Data = resp.Data;
                    response.ResponseCode = "";
                    response.ResponseMessage = "";
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
