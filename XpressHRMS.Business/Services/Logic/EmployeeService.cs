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
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _EmployeeRepo;
        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository EmployeeRepo)
        {
            _logger = logger;
            _EmployeeRepo = EmployeeRepo;

        }

        public async Task<BaseResponse<CreateEmployeeDTO>> CreateEmployee(CreateEmployeeDTO payload)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.FirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || FirstName is NULL";
                }
                if (string.IsNullOrEmpty(payload.LastName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || LastName is Null";
                }
                if (string.IsNullOrEmpty(payload.EmailAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmailAddress is Null";
                }
                if (string.IsNullOrEmpty(payload.PhoneNumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || PhoneNumber is Null";
                }
                if (!isModelStateValidate)
                {
                    return new BaseResponse<CreateEmployeeDTO>()
                    {
                        ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = validationMessage,
                        Data = null
                    };
                }
                else
                {
                    int result = await _EmployeeRepo.CreateEmployee(payload);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateEmployeeDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = payload

                        };
                    }
                    else if (result == -1)
                    {
                        return new BaseResponse<CreateEmployeeDTO>()
                        {
                            ResponseCode = ResponseCode.Already_Exist.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Already Exist",
                            Data = null

                        };
                    }
                    else
                    {
                        return new BaseResponse<CreateEmployeeDTO>()
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
                _logger.LogError($"MethodName: CreateEmployee() ===>{ex.Message}");
                return new BaseResponse<CreateEmployeeDTO>()
                {
                    ResponseMessage = "Unable to process the operation, kindly contact the support",
                    ResponseCode = ((int)ResponseCode.Exception).ToString(),
                    Data = null
                };
            }

        }

    }
}
