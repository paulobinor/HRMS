using Microsoft.Extensions.Logging;
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
    [Route("api/[controller]")]
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(ILogger<DepartmentService> logger, IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;

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
                if (payload.HODEmployeeID < 0)
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

                if (payload.HODEmployeeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Kindly select the Head of Department";
                }
                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is Null";
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
                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
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


        //public async Task<BaseResponse> DisableDepartment(DeleteDepartmentDTO payload)
        //{
        //    try
        //    {
        //        BaseResponse response = new BaseResponse();
        //        bool isModelStateValidate = true;
        //        string validationMessage = "";
        //        if (payload.DepartmentID < 0)
        //        {
        //            isModelStateValidate = false;
        //            validationMessage += "  || Department is NULL";
        //        }
        //        if (!isModelStateValidate)
        //        {
        //            response.ResponseMessage = validationMessage;
        //            response.ResponseCode = ResponseCode.ValidationError.ToString();
        //            response.Data = null;
        //            return response;

        //        }
        //        else
        //        {
        //            int result = await _departmentRepository.DisableDepartment(payload.DepartmentID, payload.CompanyID);
        //            if (result > 0)
        //            {
        //                //response.ResponseMessage = "Department Disabled Successfully";
        //                //response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //                response.Data = payload;
        //                return response;
        //            }
        //            else
        //            {
        //                //response.ResponseMessage = "Internal Server Error";
        //                //response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
        //                response.Data = null;
        //                return response;
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public async Task<BaseResponse> ActivateDepartment(DeleteDepartmentDTO payload)
        //{
        //    try
        //    {
        //        BaseResponse response = new BaseResponse();


        //        int result = await _departmentRepository.ActivateDepartment(payload.DepartmentID, payload.CompanyID);
        //        if (result > 0)
        //        {
        //            //response.ResponseMessage = "Department Activated Successfully";
        //            //response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            response.Data = payload;
        //            return response;
        //        }
        //        else
        //        {
        //            //response.ResponseMessage = "Internal Server Error";
        //            //response.ResponseCode = ResponseCode.InternalServer.ToString("D").PadLeft(2, '0');
        //            response.Data = null;
        //            return response;
        //        }

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }

        //}
        public async Task<BaseResponse<List<GetDepartmentDTO>>> GetAllDepartments(int CompanyID)
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


        public async Task<BaseResponse<GetDepartmentDTO>> GetAllDepartmentByID(int CompanyID, int DepartmentID)
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

    }
}
