using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<BaseResponse<CreateEmployeeDTO>> CreateEmployee(CreateEmployeeDTO createEmp, int CompanyID)
        {
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(createEmp.FirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || FirstName is NULL";
                }
                if (string.IsNullOrEmpty(createEmp.LastName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || LastName is Null";
                }
                if (string.IsNullOrEmpty(createEmp.EmailAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || EmailAddress is Null";
                }
                if (string.IsNullOrEmpty(createEmp.PhoneNumber))
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
                    int result = await _EmployeeRepo.CreateEmployee(createEmp, CompanyID);
                    if (result > 0)
                    {
                        return new BaseResponse<CreateEmployeeDTO>()
                        {
                            ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                            ResponseMessage = "Record Saved Successfully",
                            Data = createEmp

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


        //public async Task<BaseResponse<EmployeeUpload>> CreateEmployeeBulk(EmployeeUpload payload, int CompanyID)
        //{
        //    try
        //    {
        //        int rowCount = 0;

        //        bool isModelStateValidate = true;
        //        string validationMessage = "";

        //        if (payload.EmployeeFiles==null)
        //        {
        //            isModelStateValidate = false;
        //            validationMessage += "  || Kindly select a file";
        //        }
                
        //        if (!isModelStateValidate)
        //        {
        //            return new BaseResponse<EmployeeUpload>()
        //            {
        //                ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0'),
        //                ResponseMessage = validationMessage,
        //                Data = null
        //            };
        //        }
        //        else
        //        {


        //            using (var stream = new MemoryStream())
        //            {
        //                await payload.EmployeeFiles.CopyToAsync(stream);

        //                using (var package = new ExcelPackage(stream))
        //                {
        //                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //                    rowCount = worksheet.Dimension.Rows;
        //                    var columnmCount = worksheet.Dimension.Columns;

        //                    if (columnmCount != 5)
        //                    {
        //                        return new BaseResponse<EmployeeUpload>()
        //                        {
        //                            ResponseMessage = "Kindly Upload the appropriate file",
        //                            ResponseCode = ((int)ResponseCode.ProcessingError).ToString(),
        //                            Data = null
        //                        };
        //                    }

        //                    for (int row = 2; row <= rowCount; row++)
        //                    {
        //                        string FirstName = worksheet.Cells[row, 1].Value.ToString().Trim();
        //                        string LastName = worksheet.Cells[row, 2].Value.ToString().Trim();
        //                        string EmailAddress = worksheet.Cells[row, 3].Value.ToString().Trim();
        //                        string PhoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim();
        //                        string staffID= worksheet.Cells[row, 5].Value.ToString().Trim();
        //                        var EmpPayload = new CreateEmployeeDTOBulk
        //                        {
        //                             FirstName=FirstName,
        //                             LastName=LastName,
        //                             EmailAddress=EmailAddress,
        //                             PhoneNumber=PhoneNumber,
        //                             HRTag= staffID
        //                        };

        //                        _EmployeeRepo.CreateEmployeeBulk(EmpPayload, CompanyID);
                               
        //                    }
        //                }
        //            }

                 
        //                return new BaseResponse<EmployeeUpload>()
        //                {
        //                    ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
        //                    ResponseMessage = "Record Saved Successfully",
        //                    Data = payload

        //                };
                    
                   
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"MethodName: CreateEmployee() ===>{ex.Message}");
        //        return new BaseResponse<EmployeeUpload>()
        //        {
        //            ResponseMessage = "Unable to process the operation, kindly contact the support",
        //            ResponseCode = ((int)ResponseCode.Exception).ToString(),
        //            Data = null
        //        };
        //    }

        //}


    }
}
