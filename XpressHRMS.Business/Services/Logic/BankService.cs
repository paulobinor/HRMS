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
        public async Task<BaseResponse<CreateBankDTO>> CreateBank(CreateBankDTO payload)
        {
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
                    return new BaseResponse<CreateBankDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _bankRepository.CreateBank(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateBankDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateBankDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateBankDTO>()
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
                _logger.LogError($"MethodName: CreateBank() ===>{ex.Message}");
                return new BaseResponse<CreateBankDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }


        public async Task<BaseResponse<UpdateBankDTO>> UpdateBank(UpdateBankDTO payload)
        {
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
                    return new BaseResponse<UpdateBankDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };

                }
                else
                {
                    int result = await _bankRepository.UpdateBank(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<UpdateBankDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Updated  Successfully",
                            Data = payload

                        };
                    }
                    else
                    {
                        return new BaseResponse<UpdateBankDTO>()
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
                _logger.LogError($"MethodName: UpdateBank() ===>{ex.Message}");

                return new BaseResponse<UpdateBankDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<List<BanksDTO>>> GetAllBanks()
        {

            try
            {

                var result = await _bankRepository.GetAllBank();
                if (result.Count > 0)
                {
                    return new BaseResponse<List<BanksDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<List<BanksDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return new BaseResponse<List<BanksDTO>>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }


        public async Task<BaseResponse<BanksDTO>> GetBankByID(int bankID)
        {

            try
            {

                var result = await _bankRepository.GetBankById(bankID);
                if (result!=null)
                {

                    return new BaseResponse<BanksDTO>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };
                }
                else
                {
                    return new BaseResponse<BanksDTO>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }


            }
            catch (Exception)
            {
                return new BaseResponse<BanksDTO>()
                {
                    ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'),
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    Data = null

                };
            }
        }

    }
}
