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

namespace XpressHRMS.Business.Services.Logic
{
    public class BranchService : IBranchService
    {

        private readonly ILogger<BranchService> _logger;
        private readonly IBranchRepository _branchRepository;

        public BranchService(ILogger<BranchService> logger, IBranchRepository branchRepository)
        {
            _logger = logger;
            _branchRepository = branchRepository;

        }
        public async Task<BaseResponse> CreateBranch(CreateBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();
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

                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Country Is Null";
                }
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
                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company Is Null";
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
                    int result = await _branchRepository.CreateBranch(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Branch Created Successfully";
                        //response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        //response.ResponseMessage = "Branch Already Exist";
                        //response.ResponseCode = ResponseCode.Already_Exist.ToString();
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        //response.ResponseMessage = "Internal Server Error";
                        //response.ResponseCode = ResponseCode.InternalServer.ToString();
                        response.Data = null;
                        return response;
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateBranch() ===>{ex.Message}");
                return response;

            }

        }


        public async Task<BaseResponse> UpdateBranch(UpdateBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();
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

                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Country Is Null";
                }
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
                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company Is Null";
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
                    int result = await _branchRepository.UpdateBranch(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "branch Updated Successfully";
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
                _logger.LogError($"MethodName: UpdateBranch() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> DeleteBranch(DeleteBranchDTO payload)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";
                if (payload.BranchID < 0)
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
                    int result = await _branchRepository.DeleteBranch(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Branch Deleted Successfully";
                        //response.ResponseCode = ResponseCode.Ok.ToString();
                        response.Data = payload;
                        return response;
                    }
                    else
                    {
                        //response.ResponseMessage = "Failed to delete record";
                        //response.ResponseCode = ResponseCode.InternalServer.ToString();
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

        public async Task<BaseResponse> GetAllBranches(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _branchRepository.GetAllBranches(CompanyID);
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Branch Retrieved Successfully";
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


        public async Task<BaseResponse> GetBranchByID(DeleteBranchDTO payload)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _branchRepository.GetBranchByID(payload);
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Branch Retrieved Successfully";
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
