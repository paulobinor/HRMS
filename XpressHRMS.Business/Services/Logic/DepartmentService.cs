﻿using Microsoft.Extensions.Logging;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(ILogger<DepartmentService> logger, IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;

        }

        public async Task<BaseResponse> CreateDepartment(DepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    int result = await _departmentRepository.CreateDepartment(payload);
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
                _logger.LogError($"MethodName: CreateDepartment() ===>{ex.Message}");
                return response;

            }

        }


        public async Task<BaseResponse> UpdateDepartment(UpdateDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();
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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    int result = await _departmentRepository.UpdateDepartment(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Department Updated Successfully";
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
                _logger.LogError($"MethodName: UpdateDepartment() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> DeleteDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();

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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    int result = await _departmentRepository.DeleteDepartment(payload.DepartmentID, payload.CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Department Deleted Successfully";
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
                _logger.LogError($"MethodName: DeleteDepartment() ===>{ex.Message}");
                return response;
            }

        }


        public async Task<BaseResponse> DisableDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.DepartmentID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department is NULL";
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
                    int result = await _departmentRepository.DisableDepartment(payload.DepartmentID, payload.CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Department Disabled Successfully";
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
                _logger.LogError($"MethodName: DisableDepartment() ===>{ex.Message}");
                return response;
            }

        }

        public async Task<BaseResponse> ActivateDepartment(DeleteDepartmentDTO payload)
        {
            BaseResponse response = new BaseResponse();

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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    int result = await _departmentRepository.ActivateDepartment(payload.DepartmentID, payload.CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Department Activated Successfully";
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

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ActivateDepartment() ===>{ex.Message}");

                return response;
            }

        }

        public async Task<BaseResponse> GetAllDepartment(int CompanyID)
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
                    response.Data = null;
                    return response;

                }
                else
                {
                    var result =await _departmentRepository.GetAllDepartment(CompanyID);
                    if (result!=null)
                    {
                        response.ResponseMessage = "Department Retrieved Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = result;
                        return response;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllDepartment() ===>{ex.Message}");
                return response;
            }
        }

        public async Task<BaseResponse> GetDepartmentByID(int CompanyID, int DepartmentID)
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
                    response.Data = null;
                    return response;

                }
                else
                {
                    var result = await _departmentRepository.GetAllDepartmentByID(DepartmentID,CompanyID);
                    if (result != null)
                    {
                        response.ResponseMessage = "Department Retrieved Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = result;
                        return response;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllDepartment() ===>{ex.Message}");
                return response;
            }
        }



    }
}
