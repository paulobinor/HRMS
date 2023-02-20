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

    //[ApiController]
    //[Authorize]
    public class LoginController : /*ControllerBase*/ BaseController
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
                if (resp.Data != null)
                {
                    System.Reflection.PropertyInfo value = resp.Data.GetType().GetProperty("CompanyID");
                    string companyid = (string) value.GetValue(resp.Data, null);
                    if (string.IsNullOrEmpty(companyid))
                    {
                        companyid = "0";
                    }

                   
                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.SerialNumber, companyid),
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
            BaseResponse<CreateAdminUserLoginDTO> response = new BaseResponse<CreateAdminUserLoginDTO>();
            try
            {

                //Xpress@Admin
                //Admin@2023
                var claimsIdentity = this.User.Identity as ClaimsIdentity;

                string email = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                string CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                payload.CreatedBy = CreatedBy;
               

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
                    response.ResponseCode = resp.ResponseCode;
                    response.ResponseMessage = resp.ResponseMessage;
                    return Ok(response);

                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(response);
            }
        }

        //[HttpPut("UpdateAdmin")]
        //public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminUserLoginDTO payload)
        //{
        //    string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
        //    try
        //    {
        //        return this.CustomResponse(await _iSSOservice.UpdateAdmin(payload, RemoteIpAddress, RemotePort));

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [HttpDelete("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(int CompanyID, int AdminUserID)
        {
            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _iSSOservice.DeleteAdmin(CompanyID, AdminUserID, RemoteIpAddress, RemotePort));

            }
            catch (Exception e)
            {
                return null;
            }
        }

       
        [HttpPost("DisableAdmin")]
        public async Task<IActionResult> DisableAdmin(int CompanyID, int AdminUserID)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _iSSOservice.DisableAdmin(CompanyID, AdminUserID, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("ActivateAdmin")]
        public async Task<IActionResult> ActivateAdmin(int CompanyID, int AdminUserID)
        {

            string RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            string RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            try
            {
                return this.CustomResponse(await _iSSOservice.ActivateAdminUser(CompanyID, AdminUserID, RemoteIpAddress, RemotePort));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAllAdminUser()
        {
            try
            {
                return this.CustomResponse(await _iSSOservice.GetAllAdminUser());

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
