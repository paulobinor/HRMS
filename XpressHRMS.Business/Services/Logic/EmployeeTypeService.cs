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
    public class EmployeeTypeService : IEmployeeTypeService
    {
        private readonly ILogger<EmployeeTypeService> _logger;
        private readonly IEmployeeTypeRepository _EmployeeTypeRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public EmployeeTypeService(ILogger<EmployeeTypeService> logger, IEmployeeTypeRepository EmployeeTypeRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _EmployeeTypeRepository = EmployeeTypeRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (createEmployeeType.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || CompanyID is NULL";
                }
                if (string.IsNullOrEmpty(createEmployeeType.EmployeeTypeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeTypeName is NULL";
                }
                //if (string.IsNullOrEmpty(createEmployeeType.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}

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
                        Payload = JsonConvert.SerializeObject(createEmployeeType),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit=_auditTrailRepository.CreateAuditTrail(auditry);

                    dynamic result = await _EmployeeTypeRepository.CreateEmployeeType(createEmployeeType);
                    if (result > 0)
                    {
                        response.ResponseMessage = "EmployeeTypeName Created Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = createEmployeeType;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "EmployeeTypeName Already Exist";
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
                _logger.LogError($"MethodName: CreateEmployeeTypeName() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (UpdateEmployeeType.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
                }
                if (UpdateEmployeeType.EmployeeTypeID< 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeType ID is NULL";
                }
                if (string.IsNullOrEmpty(UpdateEmployeeType.EmployeeTypeName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeType is NULL";
                }
                //if (string.IsNullOrEmpty(UpdateEmployeeType.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || CreatedBy is NULL";
                //}


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
                        Operation = "Updated EmployeeType",
                        Payload = JsonConvert.SerializeObject(UpdateEmployeeType),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _EmployeeTypeRepository.UpdateEmployeeType(UpdateEmployeeType);
                    if (result > 0)
                    {
                        response.ResponseMessage = "EmployeeType Updated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = UpdateEmployeeType;
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
                _logger.LogError($"MethodName: UpdateEmployeeType() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DeleteEmployeeType(DelEmployeeTypeDTO DelEmployeeType, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {
              
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (DelEmployeeType.EmployeeTypeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeType is NULL";
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
                        Operation = "Delete EmployeeType",
                        Payload = JsonConvert.SerializeObject(DelEmployeeType),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _EmployeeTypeRepository.DeleteEmployeeType(DelEmployeeType);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Position Deleted Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = DelEmployeeType;
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
                _logger.LogError($"MethodName: DeleteEmployeeType() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> DisableEmployeeType(int EmployeeTypeID, int  CompanyID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {
               

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (EmployeeTypeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeType ID is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = EmployeeTypeID;
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
                        Payload = JsonConvert.SerializeObject(EmployeeTypeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _EmployeeTypeRepository.DisableEmployeeType(EmployeeTypeID, CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "EmployeeType Disabled Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
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
                _logger.LogError($"MethodName: DisableEmployeeType() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> ActivateEmployeeType(int EmployeeTypeID, int CompanyID, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (EmployeeTypeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmployeeType is NULL";
                }
                if (!isModelStateValidate)
                {
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = EmployeeTypeID;
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
                        Operation = "Updated EmployeeType",
                        Payload = JsonConvert.SerializeObject(EmployeeTypeID),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);


                    int result = await _EmployeeTypeRepository.ActivateEmployeeType(EmployeeTypeID , CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "EmployeeType Activated Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.Data = EmployeeTypeID;
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
                _logger.LogError($"MethodName: ActivatedEmployeeType() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> GetAllEmployeeType(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await _EmployeeTypeRepository.GetAllEmployeeType(CompanyID);
                if (result == null)
                {
                    response.ResponseMessage = "Internal Server Error";
                    response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
                    response.Data = null;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "EmployeeType Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.Data = result;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllEmployeeType() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> GetEmployeeTypeByID(int CompanyID, int PositionID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _EmployeeTypeRepository.GetEmployeeTypeByID(CompanyID,PositionID);
                if (result.Count > 0)
                {
                    response.ResponseMessage = "EmployeeType Retrieved Successfully";
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
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
                _logger.LogError($"MethodName: GetAllEmployeeTypeByID() ===>{ex.Message}");
                return response;

            }
        }


    }
}
