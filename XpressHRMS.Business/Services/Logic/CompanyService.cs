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

        public async Task<BaseResponse<CreateCompanyDTO>> CreateCompany(CreateCompanyDTO payload, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (string.IsNullOrEmpty(payload.CompanyLogo))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CompanyLogo Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.PhoneNumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || PhoneNumber Name is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CompanyAddress is NULL";
                }
                
                if (string.IsNullOrEmpty(payload.Website))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Website is NULL";
                }

               
                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreateCompanyDTO>()
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
                        Operation = "Creating Company ",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _companyRepository.CreateCompany(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateCompanyDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateCompanyDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateCompanyDTO>()
                        {
                            ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Internal Server Error",
                            Data = null

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateComapany() ===>{ex.Message}");
                return new BaseResponse<CreateCompanyDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<UpdateCompanyDTO>> UpdateCompany(UpdateCompanyDTO payload, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                //if (string.IsNullOrEmpty(payload.CompanyLogo))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CompanyLogo Name is NULL";
                //}
                //if (string.IsNullOrEmpty(payload.CompanyName))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Company Name is NULL";
                //}
                //if (string.IsNullOrEmpty(payload.PhoneNumber))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Companyphonenumber Name is NULL";
                //}
                //if (string.IsNullOrEmpty(payload.CompanyAddress))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CompanyAddress is NULL";
                //}

                //if (string.IsNullOrEmpty(payload.Website))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Website is NULL";
                //}

                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is Null";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateCompanyDTO>()
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
                        Operation = "Updated Company",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    int result = await _companyRepository.UpdateCompany(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateCompanyDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateCompanyDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = payload

                        };
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateCompany() ===>{ex.Message}");

                return new BaseResponse<UpdateCompanyDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<int>> DeleteCompany(int CompanyID, string DeletedBy, string RemoteIpAddress, string RemotePort)
        {
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
                        Operation = "Delete Company",
                        Payload = JsonConvert.SerializeObject(CompanyID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _companyRepository.DeleteCompany(CompanyID, DeletedBy);
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
        public async Task<BaseResponse<int>> DisableCompany(int CompanyID, string DisableBy, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is required";
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
                   

                    int result = await _companyRepository.DisableCompany(CompanyID, DisableBy);
                    if (result > 0)
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

                        var audit = _auditTrailRepository.CreateAuditTrail(auditry);
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

        public async Task<BaseResponse<int>> ActivateCompany(int CompanyID, string EnableBy, string RemoteIpAddress, string RemotePort)
        {
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
                    return new BaseResponse<int>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    
                    int result = await _companyRepository.ActivateCompany(CompanyID, EnableBy);
                    if (result > 0)
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

                        var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Activated  Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Activate Record",
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

        public async Task<BaseResponse<List<CompanyDTO>>> GetAllCompanies()
        {

            try
            {

                var result = await _companyRepository.GetAllCompanies();
                if (result!=null)
                {
                    return new BaseResponse<List<CompanyDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<CompanyDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CompanyDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

        public async Task<BaseResponse<CompanyDTO>> GetCompanyByID(int CompanyID)
        {

            try
            {

                var result = await _companyRepository.GetCompanyByID(CompanyID);
                if (result!=null)
                {
                    return new BaseResponse<CompanyDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<CompanyDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<CompanyDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }
    }
}
