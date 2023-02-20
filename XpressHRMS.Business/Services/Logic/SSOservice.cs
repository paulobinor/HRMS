using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Business.Services.Logic
{
    [Route("api/[controller]")]

    public class SSOservice : ISSOservice
    {
        private readonly ILogger<SSOservice> _logger;
        private readonly IGenericRepository _genericRepository;
        private readonly IAdminUserRepo _adminUserRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditTrailRepository _auditTrailRepository;
        public SSOservice(ILogger<SSOservice> logger, IAuditTrailRepository auditTrailRepository, IGenericRepository genericRepository, IAdminUserRepo adminUserRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _genericRepository = genericRepository;
            _adminUserRepo = adminUserRepo;
            _httpContextAccessor = httpContextAccessor;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse> CreateAdmin(CreateAdminUserLoginDTO payload, string Email)
        {

            try
            {
                var userdetails = new CreateAdminUserLoginDTO()
                {
                    Email = payload.Email,
                    //FirstName = payload.FirstName,
                    //LastName = payload.LastName,
                    CompanyID = payload.CompanyID,
                    CreatedBy = payload.CreatedBy,
                    RoleName = payload.RoleName,
                    Password = EncryptDecrypt.EncryptResult(payload.Password)
                };

                var getemail = new UserLoginDTO()
                {
                    Email = Email,
                    Password = userdetails.Password
                };
                dynamic checkifAdminUserExist = await _adminUserRepo.GetUser(getemail);
                if (checkifAdminUserExist.Count > 0)
                {
                    dynamic login = await _adminUserRepo.CreateAdminUser(userdetails);

                    if (login > 0)
                    {
                        var response = new BaseResponse()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                        return response;
                    }
                    else if (login == -1)
                    {
                        return new BaseResponse()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "User Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse()
                        {
                            ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Internal Server Error",
                            Data = null

                        };
                    }
                }
                else
                {

                    return new BaseResponse()
                    {
                        ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "You do not have access to Create User Admin",
                        Data = null

                    };

                }

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<BaseResponse<UpdateAdminUserLoginDTO>> UpdateAdmin(UpdateAdminUserLoginDTO UpdateAdmin, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (UpdateAdmin.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is Null";
                }

                //if (string.IsNullOrEmpty(UpdateAdmin.Email))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CompanyLogo Name is NULL";
                //}
               
               

                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateAdminUserLoginDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Updated AdminUser",
                        Payload = JsonConvert.SerializeObject(UpdateAdmin),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    int result = await _adminUserRepo.UpdateAdminUser(UpdateAdmin);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateAdminUserLoginDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = UpdateAdmin

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateAdminUserLoginDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = UpdateAdmin

                        };
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateCompany() ===>{ex.Message}");

                return new BaseResponse<UpdateAdminUserLoginDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }
        public async Task<BaseResponse<int>> DeleteAdmin(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin is NULL";
                }
                if (AdminUserID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<int>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Delete AdminUser",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _adminUserRepo.DeleteAdminUser(CompanyID, AdminUserID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Delete Record",
                            Data = null

                        };
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<BaseResponse<int>> DisableAdmin(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin is NULL";
                }
                if (AdminUserID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Admin is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<int>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Disable Admin",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _adminUserRepo.DisableAdminUser(CompanyID, AdminUserID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Disable  Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Disable Record",
                            Data = null

                        };
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<BaseResponse<int>> ActivateAdminUser(int CompanyID, int AdminUserID, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (AdminUserID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || AdminUser is NULL";
                }
                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<int>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };
                }
                else
                {
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Updated AdminUser",
                        Payload = JsonConvert.SerializeObject(AdminUserID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    }; var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    int result = await _adminUserRepo.ActivateAdminUser(CompanyID, AdminUserID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Activated  Successfully",
                            Data = null
                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Activate Record",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BaseResponse<List<GetAllAdminUserLoginDTO>>> GetAllAdminUser()
        {

            try
            {

                var result = await _adminUserRepo.GetAllAdminUser();
                if (result != null)
                {
                    return new BaseResponse<List<GetAllAdminUserLoginDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<GetAllAdminUserLoginDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<GetAllAdminUserLoginDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

        //public async Task<BaseResponse<List<AdminDTO>>> GetAllUsers()
        //{

        //    try
        //    {

        //        var result = await _adminUserRepo.GetUser();
        //        if (result != null)
        //        {
        //            return new BaseResponse<List<AdminDTO>>()
        //            {
        //                ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
        //                ResponseMessage = "Record Retreived Successfully",
        //                Data = result

        //            };
        //        }
        //        else
        //        {
        //            return new BaseResponse<List<AdminDTO>>()
        //            {
        //                ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
        //                ResponseMessage = "No record found",
        //                Data = result

        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<List<AdminDTO>>()
        //        {
        //            ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
        //            ResponseMessage = "Unable to process the operation, kindly contact the support",
        //            Data = null

        //        };
        //    }
        //}



        public async Task<BaseResponse<UserLoginDTO>> AdminLogin(UserLoginDTO payload)
        {

            try
            {
                payload.Email = Encoding.UTF8.GetString(Convert.FromBase64String(payload.Email));
                payload.Password = Encoding.UTF8.GetString(Convert.FromBase64String(payload.Password));
                var adminDetails = new UserLoginDTO()
                {
                    Email = payload.Email.Trim(),
                    Password = EncryptDecrypt.EncryptResult(payload.Password.Trim())



                };

                var result = await _adminUserRepo.LoginAdmin(adminDetails);
                if (result != null)
                {
                    return new BaseResponse<UserLoginDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Saved Successfully",
                        Data = result

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
