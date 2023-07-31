using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Business.Services.Logic
{
     public class GradeService : IGradeService
    {
        private readonly ILogger<GradeService> _logger;
        private readonly IGradeRepository _GradeRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public GradeService(ILogger<GradeService> logger, IGradeRepository GradeRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _GradeRepository = GradeRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<CreateGradeDTO>> CreateGrade(CreateGradeDTO createGrade, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (createGrade.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade ID is NULL";
                }
                if (string.IsNullOrEmpty(createGrade.GradeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position Name is NULL";
                }
              

                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreateGradeDTO>()
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
                        Operation = "Creating Grade ",
                        Payload = JsonConvert.SerializeObject(createGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _GradeRepository.CreateGrade(createGrade);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateGradeDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = createGrade

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateGradeDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = createGrade

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateGradeDTO>()
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
                _logger.LogError($"MethodName: CreateGrade() ===>{ex.Message}");
                return new BaseResponse<CreateGradeDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<UpdateGradeDTO>> UpdateGrade(UpdateGradeDTO UpdateGrade, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (UpdateGrade.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
                }
                if (UpdateGrade.GradeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade ID is NULL";
                }
                if (string.IsNullOrEmpty(UpdateGrade.GradeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade Name is NULL";
                }
              


                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateGradeDTO>()
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
                        Operation = "Updated Position",
                        Payload = JsonConvert.SerializeObject(UpdateGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _GradeRepository.UpdateGrade(UpdateGrade);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateGradeDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = UpdateGrade

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateGradeDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = UpdateGrade

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateGrade() ===>{ex.Message}");
                return new BaseResponse<UpdateGradeDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<DelGradeDTO>> DeleteGrade(DelGradeDTO DelGrade, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (DelGrade.GradeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DelGradeDTO>()
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
                        Operation = "Delete Grade",
                        Payload = JsonConvert.SerializeObject(DelGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.DeleteGrade(DelGrade);
                    if (result > 0)
                    {
                        return new BaseResponse<DelGradeDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = DelGrade

                        };
                    }
                    else
                    {
                        return new BaseResponse<DelGradeDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Delete Record",
                            Data = DelGrade

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DeleteGrade() ===>{ex.Message}");
                return new BaseResponse<DelGradeDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }

        }

        public async Task<BaseResponse<int>> DisableGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {

            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (GradeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade ID is NULL";
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
                        Operation = "Disable Grade",
                        Payload = JsonConvert.SerializeObject(GradeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.DisableGrade(GradeID, CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Disable Successfully",
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
                _logger.LogError($"MethodName: Disable Grade() ===>{ex.Message}");
                return new BaseResponse<int>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }

        }

        public async Task<BaseResponse<int>> ActivateGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (GradeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Grade is NULL";
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
                        Operation = "Updated Grade",
                        Payload = JsonConvert.SerializeObject(GradeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.ActivateGrade(GradeID, CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Activated Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Activate Record",
                            Data = null

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ActivatedPosition() ===>{ex.Message}");

                return new BaseResponse<int>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }

        }

        public async Task<BaseResponse<List<GradeDTO>>> GetAllGrade(int CompanyID)
        {

            try
            {

                var result = await _GradeRepository.GetAllGrades(CompanyID);
                if (result == null)
                {
                    return new BaseResponse<List<GradeDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<GradeDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllGrades() ===>{ex.Message}");
                return new BaseResponse<List<GradeDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }

        }

        public async Task<BaseResponse<GradeDTO>> GetGradeByID(int CompanyID, int GradeID)
        {

            try
            {

                dynamic result = await _GradeRepository.GetGradeByID(CompanyID, GradeID);
                if (result.Count > 0)
                {
                    return new BaseResponse<GradeDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<GradeDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllGradeByID() ===>{ex.Message}");
                return new BaseResponse<GradeDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

    }
}
