using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using Newtonsoft.Json;



namespace XpressHRMS.Business.Services.Logic
{
    public class BranchService : IBranchService
    {

        private readonly ILogger<BranchService> _logger;
        private readonly IBranchRepository _branchRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public BranchService(ILogger<BranchService> logger, IAuditTrailRepository auditTrailRepository, IBranchRepository branchRepository)
        {
            _logger = logger;
            _branchRepository = branchRepository;
            _auditTrailRepository = auditTrailRepository;

        }
        public async Task<BaseResponse<CreateBranchDTO>> CreateBranch(CreateBranchDTO payload)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BranchName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Branch Name is NULL";
                }

                if (string.IsNullOrEmpty(payload.Address))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Address is NULL";
                }

                //if (payload.CompanyID < 0)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Country Is Null";
                //}
                if (payload.StateID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || state Is Null";
                }
                if (payload.LgaID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Lga Is Null";
                }
                //if (payload.CompanyID < 0)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Company Is Null";
                //}
                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreateBranchDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };


                }
                else
                {
                    int result = await _branchRepository.CreateBranch(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateBranchDTO>()
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
                _logger.LogError($"MethodName: CreateBranch() ===>{ex.Message}");

                return new BaseResponse<CreateBranchDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }


        public async Task<BaseResponse<UpdateBranchDTO>> UpdateBranch(UpdateBranchDTO UpdateBranch, string RemoteIpAddress, string RemotePort)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(UpdateBranch.CompanyID))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Branch Name is NULL";
                }

                if (UpdateBranch.BranchID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Country Is Null";
                }



                if (!isModelStateValidate)
                {
                    return new BaseResponse<UpdateBranchDTO>()
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
                        Operation = "Updated Branch",
                        Payload = JsonConvert.SerializeObject(UpdateBranch),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _branchRepository.UpdateBranch(UpdateBranch);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated Successfully",
                            Data = UpdateBranch

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateBranchDTO>()
                        {
                            ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Failed to Update Record",
                            Data = UpdateBranch

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateBranch() ===>{ex.Message}");
                return new BaseResponse<UpdateBranchDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<DeleteBranchDTO>> DeleteBranch(DeleteBranchDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.BranchID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
                }
                //if (payload.CompanyID < 0)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Company is NULL";
                //}
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DeleteBranchDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _branchRepository.DeleteBranch(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<DeleteBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<DeleteBranchDTO>()
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

        public async Task<BaseResponse<List<BranchDTO>>> GetAllBranches(int CompanyID)
        {

            try
            {

                dynamic result = await _branchRepository.GetAllBranches(CompanyID);
                if (result.Count > 0)
                {
                    return new BaseResponse<List<BranchDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<BranchDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return new BaseResponse<List<BranchDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }


        public async Task<BaseResponse<BranchDTO>> GetBranchByID(int BranchID, string CompanyID)
        {

            try
            {

                var result = await _branchRepository.GetBranchByID(BranchID, CompanyID);
                if (result!=null)
                {
                    return new BaseResponse<BranchDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<BranchDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return new BaseResponse<BranchDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

        public async Task<BaseResponse<DisBranchDTO>> DisableBranch(DisBranchDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.BranchID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
                }
                //if (payload.CompanyID < 0)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Company is NULL";
                //}
                if (!isModelStateValidate)
                {
                    return new BaseResponse<DisBranchDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _branchRepository.DisableBranch(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<DisBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<DisBranchDTO>()
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

        public async Task<BaseResponse<EnBranchDTO>> EnableBranch(EnBranchDTO payload)
        {
            try
            {
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.BranchID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is NULL";
                }
                //if (payload.CompanyID < 0)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Company is NULL";
                //}
                if (!isModelStateValidate)
                {
                    return new BaseResponse<EnBranchDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _branchRepository.ActivateBranch(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<EnBranchDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Deleted  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<EnBranchDTO>()
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
    }
}
