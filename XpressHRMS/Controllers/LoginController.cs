using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.AppContants;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly ISSOservice _iSSOservice;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger, ISSOservice iSSOservice)
        {
            _logger = logger;
            _iSSOservice = iSSOservice;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
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
                if (resp!=null)
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
