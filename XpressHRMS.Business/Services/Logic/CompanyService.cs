using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize]

    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> _logger;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public CompanyService(ILogger<CompanyService> logger, ICompanyRepository companyRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _companyRepository = companyRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse> CreateCompany(CreateCompanyDTO payload, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (string.IsNullOrEmpty(payload.CompanyLogo))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyLogo))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.Companyphonenumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Companyphonenumber is NULL";
                }
                if (string.IsNullOrEmpty(payload.Email))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Email is NULL";
                }
                if (string.IsNullOrEmpty(payload.MissionStmt))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || MissionStmt is NULL";
                }
                if (string.IsNullOrEmpty(payload.VisionStmt))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || VisionStmt is NULL";
                }
                if (string.IsNullOrEmpty(payload.Website))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Website is NULL";
                }

                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId =3,
                        Operation = "Creating Company ",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    //var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    dynamic result = await _companyRepository.CreateCompany(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Department Created Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "Department Already Exist";
                        response.ResponseCode = ResponseCode.Already_Exist.ToString();
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateComapany() ===>{ex.Message}");
                return response;

            }

        }


        public async Task<BaseResponse> UpdateCompany(UpdateCompanyDTO payload, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.CompanyLogo))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyLogo))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.Companyphonenumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Companyphonenumber is NULL";
                }
                if (string.IsNullOrEmpty(payload.Email))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Email is NULL";
                }
                if (string.IsNullOrEmpty(payload.MissionStmt))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || MissionStmt is NULL";
                }
                if (string.IsNullOrEmpty(payload.VisionStmt))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || VisionStmt is NULL";
                }
                if (string.IsNullOrEmpty(payload.Website))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Website is NULL";
                }
                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is Null";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Updated Company",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _companyRepository.UpdateCompany(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Company Updated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateCompany() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> DeleteCompany(int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Delete Company",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _companyRepository.DeleteCompany(CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Company Deleted Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = CompanyID;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<BaseResponse> DisableCompany(int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = CompanyID;
                    return response;

                }
                else
                {

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Disable Company",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _companyRepository.DisableCompany(CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Company Disabled Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<BaseResponse> ActivateCompany(int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = CompanyID;
                    return response;

                }
                else
                {
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Updated Company",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _companyRepository.ActivateCompany(CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Company Activated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = CompanyID;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }

                }

                return response;

            }
            catch (Exception ex)
            {

                return response;
            }

        }

        public async Task<BaseResponse> GetAllCompanies()
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var result = await _companyRepository.GetAllCompanies();
                if (result==null)
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
                    response.Data = null;
                    return response;

                    
                }
                else
                {
                    response.ResponseMessage = "Company Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.Data = result;
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;

            }
        }

        public async Task<BaseResponse> GetCompanyByID(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _companyRepository.GetCompanyByID(CompanyID);
                if (result.Count > 0)
                {
                    response.ResponseMessage = "Company Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.Data = result;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
                    response.Data = null;
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;

            }
        }
    }
}
