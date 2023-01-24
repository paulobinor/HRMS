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

        public async Task<BaseResponse> CreateGrade(CreateGradeDTO createGrade, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
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
                        Operation = "Creating Grade ",
                        Payload = JsonConvert.SerializeObject(createGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    dynamic result = await _GradeRepository.CreateGrade(createGrade);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Grade Created Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = createGrade;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "Grade Already Exist";
                        response.ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateGrade() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> UpdateGrade(UpdateGradeDTO UpdateGrade, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
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
                        Operation = "Updated Position",
                        Payload = JsonConvert.SerializeObject(UpdateGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _GradeRepository.UpdateGrade(UpdateGrade);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Updated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = UpdateGrade;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateGrade() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DeleteGrade(DelGradeDTO DelGrade, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

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
                        Operation = "Delete Grade",
                        Payload = JsonConvert.SerializeObject(DelGrade),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.DeleteGrade(DelGrade);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Grade Deleted Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = DelGrade;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DeleteGrade() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DisableGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = GradeID;
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
                        Operation = "Disable Grade",
                        Payload = JsonConvert.SerializeObject(GradeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.DisableGrade(GradeID, CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Disabled Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: Disable Grade() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> ActivateGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = GradeID;
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
                        Operation = "Updated Grade",
                        Payload = JsonConvert.SerializeObject(GradeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _GradeRepository.ActivateGrade(GradeID, CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Activated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = GradeID;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ActivatedPosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> GetAllGrade(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var result = await _GradeRepository.GetAllGrades(CompanyID);
                if (result == null)
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    response.Data = null;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Grade Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.Data = result;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllGrades() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> GetGradeByID(int CompanyID, int GradeID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _GradeRepository.GetGradeByID(CompanyID, GradeID);
                if (result.Count > 0)
                {
                    response.ResponseMessage = "Grade Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.Data = result;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    response.Data = null;
                    return response;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllGradeByID() ===>{ex.Message}");
                return response;

            }
        }

    }
}
