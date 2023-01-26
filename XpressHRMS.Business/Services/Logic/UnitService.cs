using Microsoft.Extensions.Logging;
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
    public class UnitService : IUnitService
    {
        private readonly ILogger<UnitService> _logger;
        private readonly IUnitRepository _unitRepository;

        public UnitService(ILogger<UnitService> logger, IUnitRepository unitRepository)
        {
            _logger = logger;
            _unitRepository = unitRepository;

        }

        public async Task<BaseResponse> CreateUnit(CreateUnitDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.UnitName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit Name is NULL";
                }
                if (payload.HODEmployeeID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Kindly select the Head of Department for" + " " + payload.UnitName;
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
                    int result = await _unitRepository.CreateUnit(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Unit Created Successfully";
                        response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "Unit Already Exist";
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


        public async Task<BaseResponse> UpdateUnit(UpdateUnitDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.UnitName))
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
                    int result = await _unitRepository.UpdateUnit(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Unit Updated Successfully";
                        //response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else
                    {
                        //response.ResponseMessage = "Failed to Updated record";
                        //response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateUnit() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> DeleteUnit(DeleteUnitDTO payload)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.UnitID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
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
                    int result = await _unitRepository.DeleteUnit(payload.UnitID, payload.CompanyID);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Department Deleted Successfully";
                        //response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Failed to delete record";
                        response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<BaseResponse> DisableUnit(DeleteUnitDTO payload)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.UnitID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
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
                    int result = await _unitRepository.DisableUnit(payload.UnitID, payload.CompanyID);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Unit Disabled Successfully";
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

                throw;
            }

        }

        public async Task<BaseResponse> ActivateUnit(DeleteUnitDTO payload)
        {
            try
            {
                BaseResponse response = new BaseResponse();


                int result = await _unitRepository.ActivateUnit(payload.UnitID, payload.CompanyID);
                if (result > 0)
                {
                    response.ResponseMessage = "Unit Activated Successfully";
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

                return response;

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<BaseResponse> GetAllUnits(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _unitRepository.GetAllUnits(CompanyID);
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Units Retrieved Successfully";
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    response.Data = result;
                    return response;
                }
                else
                {
                    //response.ResponseMessage = "No Record Found";
                    //response.ResponseCode = ResponseCode.InternalServer.ToString();
                    response.Data = null;
                    return response;
                }


            }
            catch (Exception)
            {
                return response;
            }
        }


        public async Task<BaseResponse> GetUnitByID(int CompanyID, int UnitID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _unitRepository.GetUnitByID(UnitID, CompanyID);
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Unit Retrieved Successfully";
                    //response.ResponseCode = ResponseCode.Ok.ToString();
                    response.Data = result;
                    return response;
                }
                else
                {
                    //response.ResponseMessage = "No record found";
                    //response.ResponseCode = ResponseCode.InternalServer.ToString();
                    response.Data = null;
                    return response;
                }


            }
            catch (Exception)
            {
                return response;
            }
        }
    }
}
