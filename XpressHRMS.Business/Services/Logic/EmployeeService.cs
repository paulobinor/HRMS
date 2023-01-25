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

        public async Task<BaseResponse> CreateEmployee(CreateEmployeeDTO payload)
        {
            BaseResponse response = new BaseResponse();
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
                    response.ResponseMessage = validationMessage;
                    response.ResponseCode = ResponseCode.ValidationError.ToString();
                    response.Data = null;
                    return response;

                }
                else
                {
                    int result = await _EmployeeRepo.CreateEmployee(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Employee Created Successfully";
                        response.ResponseCode = "00";
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "Employee Already Exist";
                        response.ResponseCode = "-1";
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
                _logger.LogError($"MethodName: CreateEmployee() ===>{ex.Message}");
                return response;

            }

        }

    }
}
