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

        public async Task<BaseResponse<CreatePositionDTO>> CreatePosition(CreatePositionDTO createpostion, string RemoteIpAddress, string RemotePort)
        {
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
                //if (string.IsNullOrEmpty(createpostion.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}

                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreatePositionDTO>()
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
                        Operation = "Creating Position ",
                        Payload = JsonConvert.SerializeObject(createpostion),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _PositionRepository.CreatePosition(createpostion);
                    if (result > 0)
                    {
                        return new BaseResponse<CreatePositionDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = createpostion

                        };

                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreatePositionDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreatePositionDTO>()
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
                _logger.LogError($"MethodName: CreatePosition() ===>{ex.Message}");
                return new BaseResponse<CreatePositionDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<UPdatePositionDTO>> UpdatePosition(UPdatePositionDTO UpdatePosition, string RemoteIpAddress, string RemotePort)
        {
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
                //if (string.IsNullOrEmpty(UpdatePosition.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}


                if (!isModelStateValidate)
                {
                    return new BaseResponse<UPdatePositionDTO>()
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
                        Payload = JsonConvert.SerializeObject(UpdatePosition),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _PositionRepository.UpdatePosition(UpdatePosition);
                    if (result > 0)
                    {
                        return new BaseResponse<UPdatePositionDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated Successfully",
                            Data = UpdatePosition

                        };
                    }
                    else
                    {
                        return new BaseResponse<UPdatePositionDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = UpdatePosition

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdatePosition() ===>{ex.Message}");
                return new BaseResponse<UPdatePositionDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<DeletePositionDTO>> DeletePosition(DeletePositionDTO DelPostion, string RemoteIpAddress, string RemotePort)
        {
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
                    return new BaseResponse<DeletePositionDTO>()
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
                        Operation = "Delete Position",
                        Payload = JsonConvert.SerializeObject(DelPostion),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _PositionRepository.DeletePosition(DelPostion);
                    if (result > 0)
                    {
                        return new BaseResponse<DeletePositionDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = DelPostion

                        };
                    }
                    else
                    {
                        return new BaseResponse<DeletePositionDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Delete Record",
                            Data = DelPostion

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DeletePosition() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<int>> DisablePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {
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
                        Operation = "Disable Position",
                        Payload = JsonConvert.SerializeObject(PositionID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _PositionRepository.DisablePosition(PositionID, CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Disabled Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<int>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record failed to Disable",
                            Data = null

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisablePosition() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<int>> ActivatePosition(int PositionID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {

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
                        Operation = "Updated Position",
                        Payload = JsonConvert.SerializeObject(PositionID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _PositionRepository.ActivatePosition(PositionID, CompanyID);
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
                return null;
            }

        }
        public async Task<BaseResponse<List<PositionDTO>>> GetAllPositions(int CompanyID)
        {

            try
            {

                var result = await _PositionRepository.GetAllPositions(CompanyID);
                if (result == null)
                {
                    return new BaseResponse<List<PositionDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<PositionDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllPosition() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<PositionDTO>> GetPositionByID(int CompanyID, int PositionID)
        {

            try
            {

                var result = await _PositionRepository.GetPositionByID(CompanyID, PositionID);
                if (result!=null)
                {
                    return new BaseResponse<PositionDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<PositionDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllPositionByID() ===>{ex.Message}");
                return null;

            }
        }
    }
}
