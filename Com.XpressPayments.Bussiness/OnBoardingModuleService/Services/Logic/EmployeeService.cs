using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;

using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Employee;
using Com.XpressPayments.Data.Repositories.EmployeeType;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public  class EmployeeService : IEmployeeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<EmployeeService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public EmployeeService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<EmployeeService> logger,
            IEmployeeRepository EmployeeRepository, IAuditLog audit, ICompanyRepository companyrepository, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _EmployeeRepository = EmployeeRepository;
            _companyrepository = companyrepository;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public async Task<BaseResponse> UpdateEmployee(UpdateEmployeeDTO updateDto, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                var Emp = await _EmployeeRepository.GetEmployeeById(updateDto.EmpID);
                if (null == Emp)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Employee";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _EmployeeRepository.UpdateEmployee(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedEmp = await _EmployeeRepository.GetEmployeeById(updateDto.EmpID);

                    _logger.LogInformation("Employee updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee updated successfully.";
                    response.Data = updatedEmp;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Employee.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateEmployeeDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateEmployeeDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveEmployee(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }
                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                //update action performed into audit log here

                var Emp = await _EmployeeRepository.GetAllActiveEmployee();

                if (Emp.Any())
                {
                    response.Data = Emp;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Employee found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveEmployee() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveEmployee() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllEmployee(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                //update action performed into audit log here

                var Employee = await _EmployeeRepository.GetAllEmployee();

                if (Employee.Any())
                {
                    response.Data = Employee;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Employee found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployee() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployee() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeeById(long EmpID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }
                         

                    }

                }

                var EmployeeType = await _EmployeeRepository.GetEmployeeById(EmpID);

                if (EmployeeType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = EmployeeType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Employee fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmployeeById(long EmployeeTypeID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetEmployeeById(long EmployeeTypeID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeebyCompanyId(long companyId, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 3)
                //    {
                //        if (Convert.ToInt32(RoleId) != 4)
                //        {
                //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //            return response;
                //        }


                //    }

                //}

                if (Convert.ToInt32(RoleId) != 2)
                {
                  
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }

                }

                var Emp = await _EmployeeRepository.GetAllEmployeeCompanyId(companyId);

                if (Emp == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Emp;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Employee fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public Tuple<bool, bool> checkPermission(int roleId, int roleId2)
        {
            bool checkCanCreateAndRead = false;
            bool canApprove = false;


            //logically check the role of those that are creating and the created
            if (roleId2 == ApplicationConstant.SuperAdmin)
            {
                if (roleId == ApplicationConstant.HrHead
                || roleId == ApplicationConstant.HrAdmin)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrHead)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrAdmin)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = false;
                }
            }

            return new Tuple<bool, bool>(checkCanCreateAndRead, canApprove);

        }

        public async Task<BaseResponse> GetEmpPendingApproval( long CompanyID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                var mappeduser = new List<EmployeeDTO>();
                var users = await _EmployeeRepository.GetEmpPendingApproval(CompanyID);
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {
                        var usermap = _mapper.Map<EmployeeDTO>(user);
                        usermap.UserStatus = "Pending";
                        usermap.UserStatusId = Convert.ToInt32(UserStatus.PENDING);
                        mappeduser.Add(usermap);
                    }


                    
                        response.Data = mappeduser;
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "Employee Details fetched successfully.";
                        return response;
                    
                }
                else
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No Users found.";
                    response.Data = null;
                    return response;
                }


             

                //update action performed into audit log here

                //var Employee = await _EmployeeRepository.GetEmpPendingApproval();

                //if (Employee.Any())
                //{
                //    response.Data = Employee;
                //    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Employee Details fetched successfully.";
                //    return response;
                //}

                //response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //response.ResponseMessage = "No Employee Details found.";
                //response.Data = null;
                //return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmpPendingApproval(CompanyID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetEmpPendingApproval(CompanyID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApproveEmp(ApproveEmp approveEmp, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);

                var UserInfo = await _accountRepository.FindUser(approveEmp.Email);

                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                if (null == UserInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User information cannot be found.";
                    return response;
                }



                //Tuple<bool, bool> checkRole = checkPermission(UserInfo.RoleId, requesterInfo.RoleId);


                //if (!checkRole.Item2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(approveEmp.Email);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot approve this User because User was created by you.";
                        return response;
                    }
                    //string defaultPass = Utils.RandomPassword();
                    //string defaultPass = ApplicationConstant.DefaultPassword;
                    dynamic resp = await _EmployeeRepository.ApproveEmp(Convert.ToInt32(requesterUserId),  user.Email);
                    //dynamic resp = await _EmployeeRepository.ApproveEmp(requesterUserId, defaultPass, user.Email);

                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} approved successfully.");
                        //_accountRepository.SendEmail(user.Email, user.FirstName, defaultPass, "Activation Email", _hostEnvironment.ContentRootPath, "", port);
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} approved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while approving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> DisapproveEmp(DisapproveEmpDto disapproveEmp, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                if (Convert.ToInt32(RoleId) != 1 || Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(disapproveEmp.Email);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot disapprove this User because User was created by you.";
                        return response;
                    }




                    dynamic resp = await _accountRepository.DeclineUser(Convert.ToInt32(requesterUserId), user.Email, disapproveEmp.DisapprovedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} disapproved successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} disapproved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while disapproving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

    }
}
