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
    public class PositionService : IPositionService
    {
        private readonly ILogger<PositionService> _logger;
        private readonly IPositionRepository _PositionRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public PositionService(ILogger<PositionService> logger, IPositionRepository PositionRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _PositionRepository = PositionRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse> CreatePosition(CreatePositionDTO createpostion, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (createpostion.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CompanyID is NULL";
                }
                if (string.IsNullOrEmpty(createpostion.PositionName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || PositionName is NULL";
                }
                if (string.IsNullOrEmpty(createpostion.CreatedBy))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CreatedBy is NULL";
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
                        Operation = "Creating Position ",
                        Payload = JsonConvert.SerializeObject(createpostion),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    //var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    dynamic result = await _PositionRepository.CreatePosition(createpostion);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Created Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = createpostion;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "position Already Exist";
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
                _logger.LogError($"MethodName: CreatePosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> UpdatePosition(UPdatePositionDTO UpdatePosition, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (UpdatePosition.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
                }
                if (UpdatePosition.PositionID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position ID is NULL";
                }
                if (string.IsNullOrEmpty(UpdatePosition.PositionName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || PositionName is NULL";
                }
                if (string.IsNullOrEmpty(UpdatePosition.CreatedBy))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CreatedBy is NULL";
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
                        Payload = JsonConvert.SerializeObject(UpdatePosition),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _PositionRepository.UpdatePosition(UpdatePosition);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Updated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = UpdatePosition;
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
                _logger.LogError($"MethodName: UpdatePosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DeletePosition(DeletePositionDTO DelPostion, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {
               
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (DelPostion.PositionID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position is NULL";
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
                        Operation = "Delete Position",
                        Payload = JsonConvert.SerializeObject(DelPostion),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _PositionRepository.DeletePosition(DelPostion);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Deleted Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = DelPostion;
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
                _logger.LogError($"MethodName: DeletePosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DisablePosition(int PositionID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {
               

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (PositionID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position ID is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = PositionID;
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
                        Operation = "Disable Position",
                        Payload = JsonConvert.SerializeObject(PositionID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _PositionRepository.DisablePosition(PositionID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Disabled Successfully";
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
                _logger.LogError($"MethodName: DisablePosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> ActivatePosition(int PositionID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (PositionID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = PositionID;
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
                        Payload = JsonConvert.SerializeObject(PositionID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    int result = await _PositionRepository.ActivatePosition(PositionID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Activated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = PositionID;
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
                _logger.LogError($"MethodName: ActivatedPosition() ===>{ex.Message}");
                return response;
            }

        }
        public async Task<BaseResponse> GetAllPositions()
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var result = await _PositionRepository.GetAllPositions();
                if (result == null)
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString();
                    response.Data = null;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Positions Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString();
                    response.Data = result;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllPosition() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> GetPositionByID(int CompanyID, int PositionID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _PositionRepository.GetPositionByID(CompanyID, PositionID);
                if (result.Count > 0)
                {
                    response.ResponseMessage = "Position Retrieved Successfully";
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

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllPositionByID() ===>{ex.Message}");
                return response;

            }
        }
    }
}
