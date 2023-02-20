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
    public class HodService : IHodService
    {
        private readonly ILogger<HodService> _logger;
        private readonly IHodRepo _HodRepo;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public HodService(ILogger<HodService> logger, IHodRepo HODRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _HodRepo = HODRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<CreateHodDTO>> CreateHOD(CreateHodDTO createHOD, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                //if (string.IsNullOrEmpty(createHOD.CompanyID))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CompanyID is NULL";
                //}
                if (string.IsNullOrEmpty(createHOD.HODName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || HODName is NULL";
                }
                if (createHOD.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department ID is NULL";
                }

                //if (string.IsNullOrEmpty(createHOD.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}

                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreateHodDTO>()
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
                        Operation = "Creating HOD ",
                        Payload = JsonConvert.SerializeObject(createHOD),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _HodRepo.CreateHOD(createHOD);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateHodDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = createHOD

                        };

                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateHodDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateHodDTO>()
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
                _logger.LogError($"MethodName: CreateHOD() ===>{ex.Message}");
                return new BaseResponse<CreateHodDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<UpdateHodDTO>> UpdateHOD(UpdateHodDTO UpdateHOD, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(UpdateHOD.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CompanyID is NULL";
                }
                if (string.IsNullOrEmpty(UpdateHOD.HODName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || HODName is NULL";
                }
                if (UpdateHOD.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department ID is NULL";
                }

                if (UpdateHOD.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department ID is NULL";
                }

                //if (string.IsNullOrEmpty(UpdateHOD.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}


                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateHodDTO>()
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
                        Operation = "Updated HOD",
                        Payload = JsonConvert.SerializeObject(UpdateHOD),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _HodRepo.UpdateHOD(UpdateHOD);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateHodDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated Successfully",
                            Data = UpdateHOD

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateHodDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = UpdateHOD

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateHOD() ===>{ex.Message}");
                return new BaseResponse<UpdateHodDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<DelHodDTO>> DeleteHOD(DelHodDTO DelHOD, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty (DelHOD.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position is NULL";
                }
                if (DelHOD.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Position is NULL";
                }
                //if (string.IsNullOrEmpty(DelPostion.DeletedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DelHodDTO>()
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
                        Operation = "HOD Position",
                        Payload = JsonConvert.SerializeObject(DelHOD),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _HodRepo.DeleteHOD(DelHOD);
                    if (result > 0)
                    {
                        return new BaseResponse<DelHodDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = DelHOD

                        };
                    }
                    else
                    {
                        return new BaseResponse<DelHodDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Delete Record",
                            Data = DelHOD

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DeleteHOD() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<List<HodDTO>>> GetAllHOD(string CompanyID)
        {

            try
            {
                var result = await _HodRepo.GetAllHOD(CompanyID);
                if (result.Count >0)
                {
                    return new BaseResponse<List<HodDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result
                    };
                }

                else
                {
                    return new BaseResponse<List<HodDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllHOD() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<List<HodDTO>>> GetHODByID(string CompanyID, int HodID, int DepartmentID)
        {

            try
            {

                //var result = await _HodRepo.GetHODByID(CompanyID, HodID, DepartmentID);
                //if (result != null)
                //{
                //    return new BaseResponse<HodDTO>()
                //    {
                //        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                //        ResponseMessage = "Record Retreived Successfully",
                //        Data = result

                    //    };
                    //}
                //else
                //{
                //    return new BaseResponse<HodDTO>()
                //    {
                //        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                //        ResponseMessage = "No record found",
                //        Data = result

                //    };
                //}
                var result = await _HodRepo.GetHODByID(CompanyID, HodID, DepartmentID);
                if (result.Count > 0)
                {
                    return new BaseResponse<List<HodDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result
                    };
                }

                else
                {
                    return new BaseResponse<List<HodDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllHODByID() ===>{ex.Message}");
                return null;

            }
        }

        public async Task<BaseResponse<int>> DisableHOD(DisableHodDTO disable, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (disable.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || HOD ID is NULL";
                }

                if (string.IsNullOrEmpty(disable.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || HOD ID is NULL";
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
                        Operation = "Disable HOD",
                        Payload = JsonConvert.SerializeObject(disable),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _HodRepo.DisableHOD(disable);
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
                _logger.LogError($"MethodName: DisableHOD() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<int>> ActivateHOD(EnableHodDTO enable, string RemoteIpAddress, string RemotePort)
        {

            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (enable.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || HOD ID is NULL";
                }

                if (string.IsNullOrEmpty(enable.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
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
                        Operation = "Activate HOD",
                        Payload = JsonConvert.SerializeObject(enable),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _HodRepo.ActivateHOD(enable);
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
                _logger.LogError($"MethodName: ActivatedHOD() ===>{ex.Message}");
                return null;
            }

        }

    }
}
