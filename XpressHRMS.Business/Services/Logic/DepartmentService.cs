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
    //[Route("api/[controller]")]
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;
        public DepartmentService(ILogger<DepartmentService> logger, IDepartmentRepository departmentRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<DepartmentDTO>>CreateDepartment(CreateDepartmentDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.DepartmentName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (payload.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Kindly select the Head of Department for" + " " + payload.DepartmentName;
                }
                if (!isModelStateValidate)
                {
                    
                    return new BaseResponse<DepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };


                }
                else
                {
                    int result = await _departmentRepository.CreateDepartment(payload);
                    if (result > 0)
                    {

                        return new BaseResponse<DepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };

                    }
                    else if (result == -1)
                    {

                        return new BaseResponse<DepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {

                        return new BaseResponse<DepartmentDTO>()
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
                _logger.LogError($"MethodName: CreateDepartment() ===>{ex.Message}");
                return new BaseResponse<DepartmentDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }


        public async Task<BaseResponse<UpdateDepartmentDTO>>UpdateDepartment(UpdateDepartmentDTO payload)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.DepartmentName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }

                if (payload.HodID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Kindly select the Head of Department";
                }
                if (string.IsNullOrEmpty(payload.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _departmentRepository.UpdateDepartment(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateDepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateDepartmentDTO>()
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
                _logger.LogError($"MethodName: UpdateDepartment() ===>{ex.Message}");

                return new BaseResponse<UpdateDepartmentDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<DeleteDepartmentDTO>> DeleteDepartment(DeleteDepartmentDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department is NULL";
                }
                if (string.IsNullOrEmpty(payload.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DeleteDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _departmentRepository.DeleteDepartment(payload.DepartmentID, payload.CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<DeleteDepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<DeleteDepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to  Delete Record",
                            Data = payload

                        };
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        
        public async Task<BaseResponse<List<GetDepartmentDTO>>> GetAllDepartments(string CompanyID)
        {

            try
            {

               var result = await _departmentRepository.GetAllDepartment(CompanyID);
                if (result.Count>0)
                {

                    return new BaseResponse<List<GetDepartmentDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
               
                else
                {
                    return new BaseResponse<List<GetDepartmentDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetDepartment() ===>{ex.Message}");

                return new BaseResponse<List<GetDepartmentDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }


        public async Task<BaseResponse<GetDepartmentDTO>> GetAllDepartmentByID(string CompanyID, int DepartmentID)
        {

            try
            {

                dynamic result = await _departmentRepository.GetAllDepartmentByID(DepartmentID, CompanyID);
                if (result.Count > 0)
                {
                    return new BaseResponse<GetDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<GetDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return new BaseResponse<GetDepartmentDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

        public async Task<BaseResponse<DisDepartmentDTO>> DisableDepartment(DisDepartmentDTO Disdepartment, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (Disdepartment.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is required";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DisDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {


                    int result = await _departmentRepository.DisableDepartment(Disdepartment);
                    if (result > 0)
                    {
                        var auditry = new AuditTrailReq
                        {
                            AccessDate = DateTime.Now,
                            AccessedFromIpAddress = RemoteIpAddress,
                            AccessedFromPort = RemotePort,
                            UserId = 3,
                            Operation = "Disable Department",
                            Payload = JsonConvert.SerializeObject(Disdepartment),
                            Response = ((int)ResponseCode.Ok).ToString().ToString()
                        };

                        var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                        return new BaseResponse<DisDepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Disable  Successfully",
                            Data = null



                        };
                    }
                    else
                    {
                        return new BaseResponse<DisDepartmentDTO>()
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

        public async Task<BaseResponse<DisDepartmentDTO>> ActivateDepartment(EnDepartmentDTO enable, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (enable.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DisDepartmentDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {

                    int result = await _departmentRepository.ActivateDepartment(enable);
                    if (result > 0)
                    {
                        var auditry = new AuditTrailReq
                        {
                            AccessDate = DateTime.Now,
                            AccessedFromIpAddress = RemoteIpAddress,
                            AccessedFromPort = RemotePort,
                            UserId = 3,
                            Operation = "Updated Department",
                            Payload = JsonConvert.SerializeObject(enable),
                            Response = ((int)ResponseCode.Ok).ToString().ToString()
                        };

                        var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                        return new BaseResponse<DisDepartmentDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Activated  Successfully",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<DisDepartmentDTO>()
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

    }
}
