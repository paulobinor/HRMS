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
using XpressHRMS.Data.Enums;

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




        public async Task<BaseResponse<CreateAdminUserLoginDTO>> CreateAdmin(CreateAdminUserLoginDTO payload, string Email)
        {

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
                if (checkifAdminUserExist.Count > 0)
                {
                    dynamic login = await _adminUserRepo.CreateAdminUser(userdetails);

                    if (login > 0)
                    {
                        return new BaseResponse<CreateAdminUserLoginDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (login == -1)
                    {
                        return new BaseResponse<CreateAdminUserLoginDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "User Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateAdminUserLoginDTO>()
                        {
                            ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Internal Server Error",
                            Data = null

                        };
                    }
                }
                else
                {

                    return new BaseResponse<CreateAdminUserLoginDTO>()
                    {
                        ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "You do not have access to Create User Admin",
                        Data = null

                    };

                   
                }



                return null;

            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public async Task<BaseResponse<UserLoginDTO>> AdminLogin(UserLoginDTO payload)
        {

            try
            {
                var adminDetails = new UserLoginDTO()
                {
                    Email = payload.Email,
                    Password = EncryptDecrypt.EncryptResult(payload.Password)
                };

                var result = await _adminUserRepo.LoginAdmin(adminDetails);
                if (result != null)
                {
                    return new BaseResponse<UserLoginDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Saved Successfully",
                        Data = payload

                    };

                    
                }
                else
                {
                    return new BaseResponse<UserLoginDTO>()
                    {
                        ResponseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Failed to Login",
                        Data = null

                    };
                  
                }


            }
            catch (Exception)
            {
                return null;
            }
        }

    }

}
