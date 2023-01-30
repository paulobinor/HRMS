using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Business.Services.Logic
{
  public  class RoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _auditTrailRepository = auditTrailRepository;

        }


        public async Task<BaseResponse> GetAllRoles(int CompanyID)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                var result = await _roleRepository.GetAllRoles(CompanyID);
                if (result == null)
                {
                    response.ResponseMessage = "Not Found";
                    response.ResponseCode = "02";
                    response.Data = null;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "Roles Retrieved Successfully";
                    response.ResponseCode = "00";
                    response.Data = result;
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;

            }
        }

        public async Task<BaseResponse> GetRolesByID(DeleteRoleDTO payload)
        {
            BaseResponse response = new BaseResponse();

            try
            {

                dynamic result = await _roleRepository.GetRolesByID(payload);
                if (result.Count > 0)
                {
                    response.ResponseMessage = "Roles Retrieved Successfully";
                    response.ResponseCode = "00";
                    response.Data = result;
                    return response;
                }
                else
                {
                    response.ResponseMessage = "No Record Found";
                    response.ResponseCode = "02";
                    response.Data = null;
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;

            }
        }


        public async Task<BaseResponse> CreateRole(CreateRoleDTO payload, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";


                if (string.IsNullOrEmpty(payload.RoleName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department Name is NULL";
                }
                //if (string.IsNullOrEmpty(payload.CreatedBy))
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Created By is NULL";
                //}

                if (payload.CompanyID<0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
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
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Creating Company ",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _roleRepository.CreateRole(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Role Created Successfully";
                        response.ResponseCode = "00";
                        response.Data = payload;
                        return response;
                    }
                    else if (result == -1)
                    {
                        response.ResponseMessage = "Department Already Exist";
                        response.ResponseCode ="11";
                        response.Data = null;
                        return response;
                    }
                    else
                    {
                        response.ResponseMessage = "Internal Server Error";
                        response.ResponseCode = "08";
                        response.Data = null;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateRole() ===>{ex.Message}");
                return response;
            }

        }


        public async Task<BaseResponse> UpdateRole(UpdateRoleDTO payload, string RemoteIpAddress, string RemotePort)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.RoleName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Role Name is NULL";
                }
                if (payload.RoleID<0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Role ID is NULL";
                }
                if (payload.CompanyID < 0)                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company ID is NULL";
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
                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Updated Role",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };


                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);
                    dynamic result = await _roleRepository.UpdateRole(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Role Updated Successfully";
                        response.ResponseCode = "00";
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
                _logger.LogError($"MethodName: UpdateRole() ===>{ex.Message}");

                return response;

            }

        }

        public async Task<BaseResponse> DeleteRole(DeleteRoleDTO payload, string RemotePort, string RemoteIpAddress)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (payload.CompanyID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Company is NULL";
                }
                if (payload.RoleID < 0)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Role ID is NULL";
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

                    var auditry = new AuditTrailReq
                    {
                        AccessDate = DateTime.Now,
                        AccessedFromIpAddress = RemoteIpAddress,
                        AccessedFromPort = RemotePort,
                        UserId = 3,
                        Operation = "Delete Company",
                        Payload = JsonConvert.SerializeObject(payload),
                        Response = ((int)ResponseCode.Ok).ToString().ToString()
                    };

                    var audit = _auditTrailRepository.CreateAuditTrail(auditry);

                    int result = await _roleRepository.DeleteRole(payload);
                    if (result > 0)
                    {
                        response.ResponseMessage = "Role Deleted Successfully";
                        response.ResponseCode = "00";
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
    }
}
