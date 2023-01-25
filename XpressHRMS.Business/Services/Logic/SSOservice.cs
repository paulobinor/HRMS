using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.AppContants;
using XpressHRMS.Data.DTO;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.Response;
using System.Web.Http;
using RestSharp;
using Polly;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using RestSharp.Authenticators;
using System.Threading;
using XpressHRMS.Data.IRepository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace XpressHRMS.Business.Services.Logic
{
    [Route("api/[controller]")]

    public class SSOservice : ISSOservice
    {
        private readonly ILogger<SSOservice> _logger;
        private readonly IGenericRepository _genericRepository;
        private readonly IAdminUserRepo _adminUserRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SSOservice(ILogger<SSOservice> logger, IGenericRepository genericRepository, IAdminUserRepo adminUserRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _genericRepository = genericRepository;
            _adminUserRepo = adminUserRepo;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<BaseResponse> Login(UserLoginDTO user)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string URL = URLConstant.SSOBaseURL + URLConstant.Login;

                var client = new RestClient(URL);
                var validation = await _genericRepository.PostAsync<UserLoginDTO, BaseResponse>(URL, user);
                var S_Response = JsonConvert.DeserializeObject<SSOResponse>(validation);

                if (S_Response.responseCode =="00")
                {
                    response.Data = S_Response.data;
                    response.ResponseMessage = S_Response.responseMessage;
                    response.ResponseCode = S_Response.responseCode;
                    return response;

                }
                else if (S_Response.responseCode=="09")
                {
                     Logout(user);

                }
                else
                {
                    response.Data = S_Response.data;
                    response.ResponseMessage = S_Response.responseMessage;
                    response.ResponseCode = S_Response.responseCode;
                    return response;
                }
                response.Data = S_Response.data;
                response.ResponseMessage = S_Response.responseMessage;
                response.ResponseCode = S_Response.responseCode;
                return response;

            }
            catch (Exception ex)
            {
                return response;

            }

        }

        public void Logout(UserLoginDTO payload)
        {
            try
            {
                UserLogoutDTO user = new UserLogoutDTO();
                byte[] data = Convert.FromBase64String(payload.Email);
                string decodedString = Encoding.UTF8.GetString(data);
                user.Email = decodedString;
                string URLLogout = URLConstant.SSOBaseURL + URLConstant.LogOut;
             
                var client = new RestClient(URLLogout);
                var validation =   _genericRepository.PostAsync<UserLogoutDTO, BaseResponse>(URLLogout, user);
                var S_Response = JsonConvert.DeserializeObject<SSOLogout>(validation.ToString());
                if (S_Response.responseCode=="00")
                {
                    var gobacktologin =  Login(payload);
                }
               

            }
            catch (Exception ex)
            {

            }
        }


        public async Task<BaseResponse> CreateAdmin(CreateAdminUserLoginDTO payload, string Email)
        {
            BaseResponse response = new BaseResponse(); 

            try
            {
                var userdetails = new CreateAdminUserLoginDTO()
                {
                    Email = payload.Email,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    Password = EncryptDecrypt.EncryptResult(payload.Password)
                };

                var getemail = new UserLoginDTO()
                {
                    Email = Email
                };
                dynamic checkifAdminUserExist = await _adminUserRepo.GetAdminUser(getemail);
                if (checkifAdminUserExist.Count>0)
                {
                    dynamic login = await _adminUserRepo.CreateAdminUser(userdetails);

                    if (login > 0)
                    {
                        response.Data = null;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Saved Successfully";
                        return response;
                    }
                    else if (login == -1)
                    {
                        response.Data = null;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "User Already Exist";
                        return response;
                    }
                    else
                    {
                        response.Data = null;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Internal Server Erro";
                        return response;
                    }
                }
                else
                {
                    response.Data = null;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "You do not have access to Create User Admin";
                    return response;
                }

              

                return response;

            }
            catch (Exception ex)
            {

                return response;
            }
        }


        public async Task<BaseResponse> AdminLogin(UserLoginDTO payload)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var adminDetails = new UserLoginDTO()
                {
                    Email = payload.Email,
                    Password = EncryptDecrypt.EncryptResult(payload.Password)
                };

                var result = await _adminUserRepo.LoginAdmin(adminDetails);
                if (result!=null)
                {
                    
                    response.Data = result;
                    return response;
                }
                else
                {
                  
                    response.Data = null;
                    return response;
                }


            }
            catch (Exception)
            {
                return response;
            }
        }

    }

}
