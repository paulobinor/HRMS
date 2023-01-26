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
    public class BankService : IBankService
    {
        private readonly ILogger<BankService> _logger;
        private readonly IBankRepository _bankRepository;

        public BankService(ILogger<BankService> logger, IBankRepository bankRepository)
        {
            _logger = logger;
            _bankRepository = bankRepository;

        }
        public async Task<BaseResponse> CreateBank(CreateBankDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BankName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank is NULL";
                }
                if (string.IsNullOrEmpty(payload.CbnCode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank code is Null";
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
                    int result = await _bankRepository.CreateBank(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Bank Created Successfully";
                        //response.ResponseCode ="00";
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        //response.ResponseMessage = "Bank Already Exist";
                        //response.ResponseCode = "-1";
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        //response.ResponseMessage = "Internal Server Error";
                        //response.ResponseCode = "01";
                        response.Data = null;
                        return response;
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateBank() ===>{ex.Message}");
                return response;

            }

        }


        public async Task<BaseResponse> UpdateBank(UpdateBankDTO payload)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";
                if (string.IsNullOrEmpty(payload.BankName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank is NULL";
                }
                if (string.IsNullOrEmpty(payload.CbnCode))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Bank code is Null";
                }
                if (payload.BankID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Kindly select a bank";
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
                    int result = await _bankRepository.UpdateBank(payload);
                    if (result > 0)
                    {
                        //response.ResponseMessage = "Bank Updated Successfully";
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
                _logger.LogError($"MethodName: UpdateBank() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> GetAllBanks()
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _bankRepository.GetAllBank();
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Banks Retrieved Successfully";
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


        public async Task<BaseResponse> GetBankByID(int bankID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _bankRepository.GetBankById(bankID);
                if (result.Count > 0)
                {
                    //response.ResponseMessage = "Bank Retrieved Successfully";
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
