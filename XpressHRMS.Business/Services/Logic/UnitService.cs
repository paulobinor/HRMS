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

        public async Task<BaseResponse<CreateUnitDTO>> CreateUnit(CreateUnitDTO payload)
        {
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
                    return new BaseResponse<CreateUnitDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };


                }
                else
                {
                    int result = await _unitRepository.CreateUnit(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateUnitDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateUnitDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateUnitDTO>()
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
                return null;

            }

        }


        public async Task<BaseResponse<UpdateUnitDTO>> UpdateUnit(UpdateUnitDTO payload)
        {
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
                    return new BaseResponse<UpdateUnitDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _unitRepository.UpdateUnit(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateUnitDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateUnitDTO>()
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
                _logger.LogError($"MethodName: UpdateUnit() ===>{ex.Message}");

                return null;

            }

        }

        public async Task<BaseResponse<DeleteUnitDTO>> DeleteUnit(DeleteUnitDTO payload)
        {
            try
            {
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
                    return new BaseResponse<DeleteUnitDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _unitRepository.DeleteUnit(payload.UnitID, payload.CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<DeleteUnitDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<DeleteUnitDTO>()
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
                return null;
            }

        }


        public async Task<BaseResponse<DeleteUnitDTO>> DisableUnit(DeleteUnitDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.UnitID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DeleteUnitDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _unitRepository.DisableUnit(payload.UnitID, payload.CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<DeleteUnitDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record DisAbled Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<DeleteUnitDTO>()
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

        public async Task<BaseResponse<DeleteUnitDTO>> ActivateUnit(DeleteUnitDTO payload)
        {
            try
            {


                int result = await _unitRepository.ActivateUnit(payload.UnitID, payload.CompanyID);
                if (result > 0)
                {
                    return new BaseResponse<DeleteUnitDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Activated Successfully",
                        Data = payload

                    };
                }
                else
                {
                    return new BaseResponse<DeleteUnitDTO>()
                    {
                        ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Failed to Activate Record",
                        Data = payload

                    };
                }


            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<BaseResponse<List<UnitDTO>>> GetAllUnits(int CompanyID)
        {

            try
            {

                var result = await _unitRepository.GetAllUnits(CompanyID);
                if (result.Count > 0)
                {
                    return new BaseResponse<List<UnitDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<UnitDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<BaseResponse<UnitDTO>> GetUnitByID(int CompanyID, int UnitID)
        {

            try
            {

                var result = await _unitRepository.GetUnitByID(UnitID, CompanyID);
                if (result!=null)
                {
                    return new BaseResponse<UnitDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<UnitDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
