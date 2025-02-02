﻿using AutoMapper;
using ExcelDataReader;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.Helpers;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_common.DTO;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Repository;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace hrms_be_backend_business.Logic
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<EmployeeService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IUserAppModulePrivilegeRepository _privilegeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAuthService _authService;
        private readonly IUriService _uriService;
        private readonly IMailService _mailService;
        private readonly IGradeRepository _GradeRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IEmployeeTypeRepository _EmployeeTypeRepository;
        private readonly IPositionRepository _PositionRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IEmploymentStatusRepository _EmploymentStatusRepository;
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly IRolesRepo _rolesRepo;
        private readonly IDepartmentRepository _departmentrepository;

        public EmployeeService(IAccountRepository accountRepository, ILogger<EmployeeService> logger,
            IEmployeeRepository EmployeeRepository, IAuditLog audit, ICompanyRepository companyrepository, IWebHostEnvironment hostEnvironment, IAuthService authService, IUriService uriService, IMailService mailService, IUnitRepository unitRepository, IUserAppModulePrivilegeRepository privilegeRepository, IGradeRepository GradeRepository, IEmployeeTypeRepository EmployeeTypeRepository, IPositionRepository PositionRepository,
                IBranchRepository branchRepository, IEmploymentStatusRepository EmploymentStatusRepository,
                IJobDescriptionRepository jobDescriptionRepository, IRolesRepo rolesRepo, IDepartmentRepository departmentrepository)
        {
            _audit = audit;
            _logger = logger;
            _accountRepository = accountRepository;
            _EmployeeRepository = EmployeeRepository;
            _companyrepository = companyrepository;
            _hostEnvironment = hostEnvironment;
            _authService = authService;
            _uriService = uriService;
            _mailService = mailService;
            _unitRepository = unitRepository;
            _privilegeRepository = privilegeRepository;
            _GradeRepository = GradeRepository;
            _EmployeeTypeRepository = EmployeeTypeRepository;
            _PositionRepository = PositionRepository;
            _branchRepository = branchRepository;
            _EmploymentStatusRepository = EmploymentStatusRepository;
            _rolesRepo = rolesRepo;
            _jobDescriptionRepository = jobDescriptionRepository;
            _departmentrepository = departmentrepository;
        }

        public async Task<ExecutedResult<string>> CreateEmployeeBasis(CreateEmployeeBasisDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Create_Onboarding_Basis, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.FirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "First name is required";
                }
                if (payload.LastName == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Last name is required";
                }
                if (payload.OfficialEmail == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Official email is required";
                }
                if (payload.PhoneNumber == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Phone number is required";
                }
                if (payload.BranchId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Branch is required";
                }
                if (payload.EmploymentStatusId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Employment Status is required";
                }
                if (payload.JobRoleId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Job Role is required";
                }
                if (payload.DepartmentId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department is required";
                }
                if (payload.EmployeeTypeId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Employee Type is required";
                }
                if (payload.UnitId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeeBasisReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    MiddleName = payload.MiddleName,
                    OfficialEmail = payload.OfficialEmail,
                    BranchId = payload.BranchId,
                    PhoneNumber = payload.PhoneNumber,
                    DepartmentId = payload.DepartmentId,
                    DOB = payload.DOB,
                    EmployeeTypeId = payload.EmployeeTypeId,
                    EmploymentStatusId = payload.EmploymentStatusId,
                    JobRoleId = payload.JobRoleId,
                    PersonalEmail = payload.PersonalEmail,
                    ResumptionDate = payload.ResumptionDate,
                    UnitId = payload.UnitId,
                    GradeId = payload.GradeId,
                    IsModifield = false,
                    StaffId = payload.StaffId,
                    IsMD = payload.IsMD,
                    SupervisorId = payload.SupervisorId,
                    GroupHeadId = payload.GroupHeadId,
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeeBasis(repoPayload);

                _logger.LogInformation($" response from create employee: {repoResponse}");
             //   if (!repoResponse.Contains("Success"))
                {
                    //_logger.LogInformation($" response from create employee: {repoResponse}");
                    //return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId =  accessUser.data.UserId,
                    actionPerformed = "CreateEmployeeBasis",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (CreateEmployeeBasis)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeBasis(UpdateEmployeeBasisDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Update_Onboarding_Basis, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.FirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "First name is required";
                }
                if (payload.LastName == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Last name is required";
                }
                if (payload.OfficialEmail == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Official email is required";
                }
                if (payload.PhoneNumber == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Phone number is required";
                }
                if (payload.BranchId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Branch is required";
                }
                if (payload.EmploymentStatusId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Employment Status is required";
                }
                if (payload.JobRoleId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Job Role is required";
                }
                if (payload.DepartmentId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Department is required";
                }
                if (payload.EmployeeTypeId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Employee Type is required";
                }
                if (payload.UnitId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Unit is required";
                }
                //if (payload.SupervisorId < 1)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Unit is required";
                //}
                //if (payload.GroupHeadId < 1)
                //{
                //    isModelStateValidate = false;
                //    validationMessage += "  || Unit is required";
                //}

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeeBasisReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    MiddleName = payload.MiddleName,
                    OfficialEmail = payload.OfficialEmail,
                    BranchId = payload.BranchId,
                    PhoneNumber = payload.PhoneNumber,
                    DepartmentId = payload.DepartmentId,
                    DOB = payload.DOB,
                    EmployeeTypeId = payload.EmployeeTypeId,
                    EmploymentStatusId = payload.EmploymentStatusId,
                    JobRoleId = payload.JobRoleId,
                    PersonalEmail = payload.PersonalEmail,
                    ResumptionDate = payload.ResumptionDate,
                    UnitId = payload.UnitId,
                    GradeId = payload.GradeId,
                    EmployeeId = payload.EmployeeId,
                    IsModifield = true,
                    StaffId = payload.StaffId,
                    SupervisorId = payload.SupervisorId,
                    GroupHeadId = payload.GroupHeadId,
                    IsMD = payload.IsMD
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeeBasis(repoPayload);
                //if (!repoResponse.Contains("Success"))
                //{
                //    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                //}

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeBasis",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeBasis)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeCompensation(UpdateEmployeeCompensationDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Update_Employee_Compensation, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (payload.BaseSalary < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Base salary is required";
                }

                if (payload.PayrollId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Payroll is required";
                }


                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeeCompensationReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    BaseSalary = payload.BaseSalary,
                    EmployeeId = payload.EmployeeId,
                    PayrollId = payload.PayrollId,
                    SalaryEffectiveFrom = payload.SalaryEffectiveFrom,
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeeCompensation(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeCompensation",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Updated Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeCompensation)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeePersonalInfo(UpdateEmployeePersonalInfoDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BirthPlace))
                {
                    isModelStateValidate = false;
                    validationMessage += "Birth Place is required";
                }
                if (payload.NationalityId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Nationality is required";
                }
                if (payload.StateOfOriginId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || State of origin is required";
                }
                if (payload.LGAOfOriginId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || LGA of origin is required";
                }
                if (string.IsNullOrEmpty(payload.TownOfOrigin))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Town of origin is required";
                }
                if (string.IsNullOrEmpty(payload.MaidenName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Maiden name is required";
                }
                if (payload.MaritalStatusId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Marital Status is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeePersonalInfoReq //Should implement automapper
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    LGAOfOriginId = payload.LGAOfOriginId,
                    StateOfOriginId = payload.StateOfOriginId,
                    MaritalStatusId = payload.MaritalStatusId,
                    NationalityId = payload.NationalityId,
                    GenderId = payload.GenderId,
                    BirthPlace = payload.BirthPlace,
                    EmployeeId = payload.EmployeeId,
                    MaidenName = payload.MaidenName,
                    HasChildren = payload.HasChildren,
                   // PreviousEmployerAddress = payload.PreviousEmployerAddress,
                    NoOfChildren = payload.NoOfChildren,
                    SpouseName = payload.SpouseName,
                    TownOfOrigin = payload.TownOfOrigin,
                    NIN = payload.NIN,
                    ProfileImage = payload.ProfileImage
                    
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeePersonalInfo(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                foreach (var dat in payload.Identifications)
                {
                    if (dat.IdentificationTypeId > 0)
                    {
                        var employeeIdentificationDto = new EmployeeIdentificationReq
                        {
                            CountryIdentificationIssuedId = dat.CountryIdentificationIssuedId,
                            IdentificationDocument = dat.IdentificationDocument,
                            IdentificationNumber = dat.IdentificationNumber,
                            IdentificationTypeId = dat.IdentificationTypeId,
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            EmployeeId = payload.EmployeeId
                        };
                        var employeeIdentification = await _EmployeeRepository.ProcessEmployeeIdentification(employeeIdentificationDto);
                    }
                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateEmployeePersonalInfo",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (CreateEmployeeBasis)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeContactDetails(UpdateEmployeeContactDetailsDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.PersonalEmail))
                {
                    isModelStateValidate = false;
                    validationMessage += "Personal Email is required";
                }
                if (string.IsNullOrEmpty(payload.PhoneNumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "Phone Number is required";
                }
                if (payload.CurrentAddressStateId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Current Address State is required";
                }
                if (payload.CurrentAddressLGAId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Current Address LGA is required";
                }
                if (string.IsNullOrEmpty(payload.CurrentAddressCity))
                {
                    isModelStateValidate = false;
                    validationMessage += "Current Address City is required";
                }
                if (string.IsNullOrEmpty(payload.CurrentAddressOne))
                {
                    isModelStateValidate = false;
                    validationMessage += "Current Address One is required";
                }
                if (payload.MailingAddressStateId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Mailing Address State is required";
                }
                if (payload.CurrentAddressLGAId < 1)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Mailing Address LGA is required";
                }
                if (string.IsNullOrEmpty(payload.CurrentAddressCity))
                {
                    isModelStateValidate = false;
                    validationMessage += "Mailing Address City is required";
                }
                if (string.IsNullOrEmpty(payload.CurrentAddressOne))
                {
                    isModelStateValidate = false;
                    validationMessage += "Current Address One is required";
                }

                if (string.IsNullOrEmpty(payload.NextOfKinName))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Next Of Kin Name of origin is required";
                }
                if (string.IsNullOrEmpty(payload.NextOfKinPhoneNumber))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Next Of Kin Phone Number is required";
                }
                if (string.IsNullOrEmpty(payload.NextOfKinEmailAddress))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Next Of Kin Email Address is required";
                }
                if (string.IsNullOrEmpty(payload.NextOfKinRelationship))
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Next Of Kin Relationship is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeeContactDetailsReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    CurrentAddressCity = payload.CurrentAddressCity,
                    NextOfKinEmailAddress = payload.NextOfKinEmailAddress,
                    CurrentAddressLGAId = payload.CurrentAddressLGAId,
                    CurrentAddressStateId = payload.CurrentAddressStateId,
                    CurrentAddressOne = payload.CurrentAddressOne,
                    CurrentAddressTwo = payload.CurrentAddressTwo,
                    MailingAddressCity = payload.MailingAddressCity,
                    MailingAddressLGAId = payload.MailingAddressLGAId,
                    MailingAddressOne = payload.MailingAddressOne,
                    MailingAddressStateId = payload.MailingAddressStateId,
                    MailingAddressTwo = payload.MailingAddressTwo,
                    SpouseAddressCity = payload.SpouseAddressCity,
                    SpouseAddressLGAId = payload.SpouseAddressLGAId,
                    SpouseAddressOne = payload.SpouseAddressOne,
                    SpouseAddressStateId = payload.SpouseAddressStateId,
                    SpouseAddressTwo = payload.SpouseAddressTwo,
                    EmployeeId = payload.EmployeeId,
                    NextOfKinName = payload.NextOfKinName,
                    NextOfKinPhoneNumber = payload.NextOfKinPhoneNumber,
                    NextOfKinRelationship = payload.NextOfKinRelationship,
                    PersonalEmail = payload.PersonalEmail,
                    PhoneNumber = payload.PhoneNumber,
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeeContactDetails(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "CreateEmployeeContactDetails",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (CreateEmployeeContactDetails)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeProfesionalBackground(UpdateEmployeeProfBackgroundDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool allSubmitted = true;
                foreach (var dat in payload.ProfBackgroundDetails)
                {
                    if (!string.IsNullOrEmpty(dat.CompanyName) && !string.IsNullOrEmpty(dat.PositionHead) && !string.IsNullOrEmpty(dat.ContactEmail))
                    {
                        var repoPayload = new EmployeeProfesionalBackgroundReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            CompanyName = dat.CompanyName,
                            CompanyAddress = dat.CompanyAddress,
                            PositionHead = dat.PositionHead,
                            ContactEmail = dat.ContactEmail,
                            EmployeeId = payload.EmployeeId,
                            EndDate = dat.StartDate,
                            StartDate = dat.EndDate,
                        };
                        var response = await _EmployeeRepository.ProcessEmployeeProfesionalBackground(repoPayload);
                        if (!response.Contains("Success"))
                        {
                            allSubmitted = false;
                        }
                    }
                    else
                    {
                        allSubmitted = false;
                    }

                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeProfesionalBackground",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                string responseMsg = (allSubmitted == true) ? "Updated Successfully" : "One or more information did not submit because probably the there might be some detail missing";
                return new ExecutedResult<string>() { responseMessage = responseMsg, responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeProfesionalBackground)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeEduBackground(UpdateEmployeeEduBackgroundDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool allSubmitted = true;
                foreach (var dat in payload.EmployeeEduBackground)
                {
                    if (!string.IsNullOrEmpty(dat.CertificateDoc) && !string.IsNullOrEmpty(dat.CertificateName) && !string.IsNullOrEmpty(dat.InstitutionName))
                    {
                        var repoPayload = new EmployeeEduBackgroundReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            CertificateDoc = dat.CertificateDoc,
                            CertificateName = dat.CertificateName,
                            InstitutionName = dat.InstitutionName,
                            EmployeeId = payload.EmployeeId,
                            EndDate = dat.EndDate,
                            StartDate = dat.StartDate,
                        };
                        string repoResponse = await _EmployeeRepository.ProcessEmployeeEduBackground(repoPayload);
                        if (!repoResponse.Contains("Success"))
                        {
                            allSubmitted = false;
                        }
                    }
                    else
                    {
                        allSubmitted = false;
                    }

                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeEduBackground",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                string responseMsg = (allSubmitted == true) ? "Updated Successfully" : "One or more information did not submit because probably the there might be some detail missing";
                return new ExecutedResult<string>() { responseMessage = responseMsg, responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeEduBackground)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeReference(UpdateEmployeeReferenceDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool allSubmitted = true;
                foreach (var dat in payload.EmployeeReferences)
                {
                    if (!string.IsNullOrEmpty(dat.FullName) && !string.IsNullOrEmpty(dat.ContactAddress) && !string.IsNullOrEmpty(dat.EmailAddress) && !string.IsNullOrEmpty(dat.Occupation) && !string.IsNullOrEmpty(dat.PeriodKnown) && !string.IsNullOrEmpty(dat.PhoneNumber))
                    {
                        var repoPayload = new EmployeeReferenceReq
                        {
                            CreatedByUserId = accessUser.data.UserId,
                            DateCreated = DateTime.Now,
                            EmailAddress = dat.EmailAddress,
                            EmployeeId = payload.EmployeeId,
                            FullName = dat.FullName,
                            Occupation = dat.Occupation,
                            PeriodKnown = dat.PeriodKnown,
                            PhoneNumber = dat.PhoneNumber,
                            ContactAddress = dat.ContactAddress,
                        };
                        var repoResponse = await _EmployeeRepository.ProcessEmployeeReference(repoPayload);
                        if (!repoResponse.Contains("Success"))
                        {
                            allSubmitted = false;
                        }
                    }
                    else
                    {
                        allSubmitted = false;
                    }
                }
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeReference",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);
                string responseMsg = (allSubmitted == true) ? "Updated Successfully" : "One or more information did not submit because probably the there might be some detail missing";
                return new ExecutedResult<string>() { responseMessage = responseMsg, responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeReference)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UpdateEmployeeBankDetails(UpdateEmployeeBankDetailsDto payload, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                if (accessUser.data.EmployeeId != payload.EmployeeId)
                {
                    return new ExecutedResult<string>() { responseMessage = $"This information is to be completed by the employee", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.BankName))
                {
                    isModelStateValidate = false;
                    validationMessage += "Bank Name is required";
                }
                if (string.IsNullOrEmpty(payload.BVN))
                {
                    isModelStateValidate = false;
                    validationMessage += "|| BVN is required";
                }

                if (string.IsNullOrEmpty(payload.AccountName))
                {
                    isModelStateValidate = false;
                    validationMessage += " || Account Name is required";
                }
                if (string.IsNullOrEmpty(payload.AccountNumber))
                {
                    isModelStateValidate = false;
                    validationMessage += " || Account Number is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var repoPayload = new EmployeeBankDetailsReq
                {
                    CreatedByUserId = accessUser.data.UserId,
                    DateCreated = DateTime.Now,
                    EmployeeId = payload.EmployeeId,
                    AccountName = payload.AccountName,
                    AccountNumber = payload.AccountNumber,
                    PensionAdministrator = payload.PensionAdministrator,
                    BankName = payload.BankName,
                    BVN = payload.BVN,
                    NIN = payload.NIN,
                    PensionPinNumber = payload.PensionPinNumber,
                    TaxNumber = payload.TaxNumber,
                };
                string repoResponse = await _EmployeeRepository.ProcessEmployeeBankDetails(repoPayload);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "UpdateEmployeeBankDetails",
                    payload = JsonConvert.SerializeObject(payload),
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Updated Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (UpdateEmployeeBankDetails)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> CheckEmployeeStaffId(string StaffId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                string repoResponse = await _EmployeeRepository.CheckEmployeeStaffId(StaffId, accessUser.data.CompanyId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = EnumHelper.GetEnumDescription(ResponseCode.Ok), responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (CheckEmployeeStaffId)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<BaseResponse> CreateEmployeeBulkUpload(IFormFile payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                var accessUser = await _authService.CheckUserAccess(requester.AccessToken, requester.IpAddress);
                if (accessUser.data == null)
                {
                    return new BaseResponse() { ResponseMessage = $"Unathorized User", ResponseCode = ((int)ResponseCode.AuthorizationError).ToString(), Data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Update_Onboarding_Basis, accessUser.data.UserId);
                if (!checkPrivilege.ToLower().Contains("success"))
                {
                    return new BaseResponse() { ResponseMessage = $"{checkPrivilege}", ResponseCode = ((int)ResponseCode.NoPrivilege).ToString(), Data = null };

                }
                if (payload == null || payload.Length <= 0)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "No file for Upload";
                    return response;
                }
                else if (!Path.GetExtension(payload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "File not an Excel Format";
                    return response;
                }
                else
                {
                    requester.UserId = accessUser.data.UserId;
                    requester.Username = accessUser.data.OfficialMail;
                    var stream = new MemoryStream();
                    await payload.CopyToAsync(stream);

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    var reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    int rowCount = ds.Tables[0].Rows.Count;
                    DataTable serviceDetails = ds.Tables[0];

                    int k = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var dataTable = GenerateUserDataTableHeader();
                        var createEmployeeList = new List<CreateEmployeeBasisDto>();
                        string FirstName = serviceDetails.Rows[0][0].ToString();
                        string MiddleName = serviceDetails.Rows[0][1].ToString();
                        string LastName = serviceDetails.Rows[0][2].ToString();
                        string Email = serviceDetails.Rows[0][3].ToString();
                        string DOB = serviceDetails.Rows[0][4].ToString();
                        string ResumptionDate = serviceDetails.Rows[0][5].ToString();
                        string OfficialMail = serviceDetails.Rows[0][6].ToString();
                        string PhoneNumber = serviceDetails.Rows[0][7].ToString();
                        string StaffID = serviceDetails.Rows[0][8].ToString();
                        string UnitName = serviceDetails.Rows[0][9].ToString();
                        string GradeName = serviceDetails.Rows[0][10].ToString();
                        string EmployeeTypeName = serviceDetails.Rows[0][11].ToString();
                        string BranchName = serviceDetails.Rows[0][12].ToString();
                        string EmploymentStatusName = serviceDetails.Rows[0][13].ToString();
                        string RoleName = serviceDetails.Rows[0][14].ToString();
                        string DepartmentName = serviceDetails.Rows[0][15].ToString();
                        //string CompanyName = serviceDetails.Rows[0][16].ToString();


                        if (FirstName != "FirstName" || MiddleName != "MiddleName"
                        || LastName != "LastName" || Email != "Email" || DOB != "DOB(dd/mm/yyyy)" || ResumptionDate != "ResumptionDate(dd/mm/yyyy)"
                        || OfficialMail != "OfficialMail" || PhoneNumber != "PhoneNumber" || StaffID != "StaffID" || UnitName != "UnitName" || GradeName != "GradeName" || EmployeeTypeName != "EmployeeTypeName"
                        || BranchName != "BranchName" || EmploymentStatusName != "EmploymentStatusName"
                        || RoleName != "RoleName" || DepartmentName != "DepartmentName" /*|| CompanyName != "CompanyName"*/)
                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            var company = await _companyrepository.GetCompanyById(accessUser.data.CompanyId);
                            if (company == null || company.IsDeleted)
                            {
                                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                                response.ResponseMessage = "Company not found";
                                return response;
                            }

                            var departmentsTask = _departmentrepository.GetDepartmentes(accessUser.data.CompanyId);
                            var unitsTask = _unitRepository.GetUnites(accessUser.data.CompanyId);
                            var gradsTask = _GradeRepository.GetGrades(accessUser.data.CompanyId);
                            var employeTypesTask = _EmployeeTypeRepository.GetEmployeeTypes(accessUser.data.CompanyId);
                            var employeStatusTask = _EmploymentStatusRepository.GetEmploymentStatus(accessUser.data.CompanyId);
                            var rolesTask = _rolesRepo.GetAllRoles();
                            var branchesTask = _branchRepository.GetBranches(accessUser.data.CompanyId);

                            await Task.WhenAll(departmentsTask, unitsTask, gradsTask,
                            employeTypesTask, employeStatusTask, rolesTask, branchesTask);

                            // You can access the results directly
                            var departments = departmentsTask.Result;
                            var units = unitsTask.Result;
                            var grades = gradsTask.Result;
                            var employeeTypes = employeTypesTask.Result;
                            var employmentStatuses = employeStatusTask.Result;
                            var roles = rolesTask.Result;
                            var branches = branchesTask.Result;
                            string patternEmail = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {
                                DateOnly resumptionDatenew = new DateOnly();
                                DateOnly date = new DateOnly();
                                bool isDobConverted = false;

                                long? unitID = null;
                                long? gradeID = null;
                                long? employeeTypeID = null;
                                long? branchID = null;
                                long? employmentStatusID = null;
                                long? roleID = null;
                                long? departmentID = null;

                                string rowError = string.Empty;
                                string firstName = serviceDetails.Rows[row][0].ToString();
                                string middleName = serviceDetails.Rows[row][1].ToString();
                                string lastName = serviceDetails.Rows[row][2].ToString();
                                string email = serviceDetails.Rows[row][3].ToString();
                                string dob = serviceDetails.Rows[row][4].ToString();
                                string resumptionDate = serviceDetails.Rows[row][5].ToString();
                                string officialMaildata = serviceDetails.Rows[row][6].ToString();
                                string phoneNumber = serviceDetails.Rows[row][7].ToString();
                                string staffID = serviceDetails.Rows[row][8].ToString();
                                var unitName = serviceDetails.Rows[row][9].ToString();
                                var gradeName = serviceDetails.Rows[row][10].ToString();
                                var employeeTypeName = serviceDetails.Rows[row][11].ToString();
                                var branchName = serviceDetails.Rows[row][12].ToString();
                                var employmentStatusName = serviceDetails.Rows[row][13].ToString();
                                var roleName = serviceDetails.Rows[row][14].ToString();
                                var departmentName = serviceDetails.Rows[row][15].ToString();
                                //var companyName = serviceDetails.Rows[row][16].ToString();

                                if (string.IsNullOrEmpty(firstName))
                                    rowError = $"{rowError} First name is required.";
                                else if (string.IsNullOrEmpty(lastName))
                                    rowError = $"{rowError} Last name is required.";
                                else if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, patternEmail))
                                    rowError = $"{rowError} Invalid email supplied.";
                                else if (string.IsNullOrEmpty(officialMaildata) || !Regex.IsMatch(officialMaildata, patternEmail))
                                    rowError = $"{rowError} Invalid official supplied.";
                                else if (string.IsNullOrEmpty(phoneNumber))
                                    rowError = $"{rowError} Phone number is required.";
                                else if (string.IsNullOrEmpty(staffID))
                                    rowError = $"{rowError} StaffID is required.";
                                //else if (string.IsNullOrEmpty(employeeTypeName))
                                //    rowError = $"{rowError} Employee type is required.";
                                //else if (string.IsNullOrEmpty(departmentName))
                                //    rowError = $"{rowError} Department name is required.";


                                if (!string.IsNullOrEmpty(unitName))
                                {
                                    var unit = units.FirstOrDefault(m => m.UnitName.ToLower() == (unitName.ToLower()).Trim());

                                    if (unit == null)
                                        rowError = $"{rowError} Unit {unitName} doesn't exist.";
                                    else
                                        unitID = unit.UnitID;
                                }


                                if (!string.IsNullOrEmpty(gradeName))
                                {
                                    var grade = grades.FirstOrDefault(m => m.GradeName.ToLower() == (gradeName.ToLower()).Trim());
                                    if (grade == null)
                                        rowError = $"{rowError} Grade {gradeName} doesn't exist.";
                                    else
                                        gradeID = grade.GradeID;
                                }

                                if (!string.IsNullOrEmpty(employeeTypeName))
                                {
                                    var employeeType = employeeTypes.FirstOrDefault(m => m.EmployeeTypeName.ToLower() == (employeeTypeName.ToLower()).Trim());
                                    if (employeeType == null)
                                        rowError = $"{rowError} employee Type {employeeTypeName} doesn't exist.";
                                    else
                                        employeeTypeID = employeeType.EmployeeTypeID;
                                }


                                if (!string.IsNullOrEmpty(branchName))
                                {
                                    var branch = branches.FirstOrDefault(m => m.BranchName.ToLower() == (branchName.ToLower()).Trim());
                                    if (branch == null)
                                        rowError = $"{rowError} branch {branchName} doesn't exist.";
                                    else
                                        branchID = branch.BranchID;
                                }

                                if (!string.IsNullOrEmpty(employmentStatusName))
                                {
                                    var employeeStatus = employmentStatuses.FirstOrDefault(m => m.EmploymentStatusName.ToLower() == (employmentStatusName.ToLower()).Trim());
                                    if (employeeStatus == null)
                                        rowError = $"{rowError} employee status name {employmentStatusName} doesn't exist.";
                                    else
                                        employmentStatusID = employeeStatus.EmploymentStatusID;
                                }


                                if (!string.IsNullOrEmpty(departmentName))
                                {
                                    var department = departments.FirstOrDefault(m => m.DepartmentName.ToLower() == (departmentName.ToLower()).Trim());
                                    if (department == null)
                                        rowError = $"{rowError} department {departmentName} doesn't exist.";
                                    else
                                        departmentID = department.DepartmentID;
                                }


                                if (!string.IsNullOrEmpty(roleName))
                                {
                                    var role = roles.FirstOrDefault(m => m.RoleName.ToLower() == (roleName.ToLower()).Trim());

                                    if (role != null)
                                        roleID = role.RoleId;

                                    //if (role == null)
                                    //    rowError = $"{rowError} role {roleName} doesn't exist.";
                                    //else
                                    //    roleID = role.RoleId;
                                }

                                //if (companyName.ToUpper().Trim() != company.CompanyName.ToUpper().Trim())
                                //    rowError = $"{rowError} Company name {companyName} is different from the selected company.";

                                try
                                {
                                    dob = dob.Split(" ").FirstOrDefault();
                                    resumptionDate = resumptionDate.Split(" ").FirstOrDefault();

                                    string format = "dd/MM/yyyy";
                                    date = ParseDate(dob);
                                    //date = DateOnly.ParseExact(dob, format, CultureInfo.InvariantCulture);
                                    isDobConverted = true;
                                    resumptionDatenew = ParseDate(resumptionDate);
                                    //resumptionDatenew = DateOnly.ParseExact(resumptionDate, format, CultureInfo.InvariantCulture);
                                }
                                catch (Exception ex)
                                {
                                    if (isDobConverted == false)
                                        rowError = $"{rowError} Invalid DOB({dob}) .";
                                    else
                                        rowError = $"{rowError} Invalid resumption Date({resumptionDate}).";
                                }


                                if (rowError.Length > 0)
                                {
                                    response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                                    response.ResponseMessage = $"Error on excel row {row} - {rowError}";
                                    return response;
                                }


                                var userrequest = new CreateEmployeeBasisDto
                                {
                                    FirstName = firstName,
                                    MiddleName = middleName,
                                    LastName = lastName,
                                    PersonalEmail = email,
                                    //DOB = Convert.ToDateTime(date),
                                    //ResumptionDate = Convert.ToDateTime(resumptionDate),
                                    OfficialEmail = officialMaildata,
                                    PhoneNumber = phoneNumber,
                                    UnitId = unitID == null ? 0 : Convert.ToInt64(unitID),
                                    GradeId = gradeID == null ? 0 : Convert.ToInt64(gradeID),
                                    EmployeeTypeId = employeeTypeID == null ? 0 : Convert.ToInt64(employeeTypeID),
                                    BranchId = branchID == null ? 0 : Convert.ToInt64(branchID),
                                    EmploymentStatusId = employmentStatusID == null ? 0 : Convert.ToInt64(employmentStatusID),
                                    JobRoleId = roleID == null ? 0 : Convert.ToInt64(roleID),
                                    DepartmentId = departmentID == null ? 0 : Convert.ToInt64(departmentID),


                                };

                                createEmployeeList.Add(userrequest);
                                dataTable.Rows.Add(new object[] { userrequest.FirstName.Trim(), staffID.Trim(), userrequest.MiddleName.Trim(), userrequest.LastName.Trim(), userrequest.PersonalEmail.Trim(), date, resumptionDatenew, userrequest.OfficialEmail.Trim(), userrequest.PhoneNumber.Trim(), unitID, gradeID, employeeTypeID, branchID, employmentStatusID, roleID, departmentID, accessUser.data.CompanyId });

                            }

                            var resp = await _EmployeeRepository.AddEmployeeBulk(dataTable, requester, company.LastStaffNumber, createEmployeeList.Count, accessUser.data.CompanyId);


                            var auditLog = new AuditLogDto
                            {
                                userId = requester.UserId,
                                actionPerformed = "Add Employee bulk",
                                payload = JsonConvert.SerializeObject(createEmployeeList),
                                response = null,
                                actionStatus = $"Successful",
                                ipAddress = requester.IpAddress
                            };
                            await _audit.LogActivity(auditLog);


                            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"{createEmployeeList.Count} Employees created successfully";
                            response.Data = null;
                            return response;
                        }

                    }

                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Error reading file Or file is empty";
                    response.Data = null;
                    return response;

                }


            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627)
                {
                    if (sqlEx.Message.Contains("UC_StaffID_CompanyID"))
                    {
                        var output = String.Join(";", Regex.Matches(sqlEx.Message, @"\((.+?)\)")
                        .Cast<Match>()
                        .Select(m => m.Groups[1].Value));
                        var outputArray = output.Split(",");

                        _logger.LogError($"Exception Occured ==> {sqlEx.ToString()}");
                        response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Upladed employee files contains duplicate StaffID - {outputArray[1]}";

                        return response;

                    }

                    _logger.LogError($"Exception Occured ==> {sqlEx.ToString()}");
                    response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Processing error. Kindly reach out to support.";
                    return response;
                }


                _logger.LogError($"Exception Occured ==> {sqlEx.ToString()}");
                response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Processing error. Kindly reach out to support.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.ToString()}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;

                return response;
            }
        }

        public async Task<ExecutedResult<string>> ApproveEmployee(long EmployeeId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    _logger.LogError("Unathorized User");
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Approve_Onboarding, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    _logger.LogError(checkPrivilege);
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                var password = Utils.GenerateDefaultPassword(6);

                string repoResponse = await _EmployeeRepository.ApproveEmployee(EmployeeId, password, accessUser.data.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    _logger.LogError($"We did not get a successful response from Approve employee - {repoResponse}");
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var UserId = repoResponse.Replace("Success", "");
                var userDetails = await _accountRepository.GetUserById(Convert.ToInt64(UserId));
                string token = $"{RandomGenerator.GetNumber(20)}{userDetails.UserId}";
                _logger.LogInformation($"About to send email to approved user with details: {JsonConvert.SerializeObject(userDetails)} ");
                _mailService.SendEmailApproveUser(userDetails.OfficialMail, $"{userDetails.FirstName} {userDetails.LastName} {userDetails.MiddleName}", password, "Xpress HRMS User Creation", token);
               
                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "ApproveEmployee",
                    payload = null,
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Approved Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (ApproveEmployee)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DisapproveEmployee(long EmployeeId, string Comment, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Disapprove_Employee, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comment))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comment is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }

                string repoResponse = await _EmployeeRepository.DisapproveEmployee(EmployeeId, Comment, accessUser.data.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DisapproveEmployee",
                    payload = null,
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Disapproved Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (DisapproveEmployee)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> DeleteEmployee(long EmployeeId, string Comment, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var checkPrivilege = await _privilegeRepository.CheckUserAppPrivilege(UserModulePrivilegeConstant.Delete_Employee, accessUser.data.UserId);
                if (!checkPrivilege.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{checkPrivilege}", responseCode = ((int)ResponseCode.NoPrivilege).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(Comment))
                {
                    isModelStateValidate = false;
                    validationMessage += "Comment is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }

                string repoResponse = await _EmployeeRepository.DeleteEmployee(EmployeeId, Comment, accessUser.data.UserId);
                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }

                var auditLog = new AuditLogDto
                {
                    userId = accessUser.data.UserId,
                    actionPerformed = "DeleteEmployee",
                    payload = null,
                    response = null,
                    actionStatus = $"Successful",
                    ipAddress = RemoteIpAddress
                };
                await _audit.LogActivity(auditLog);

                return new ExecutedResult<string>() { responseMessage = "Deleted Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (DeleteEmployee)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployees(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }

                var returnData = await _EmployeeRepository.GetEmployees(filter.PageNumber, filter.PageSize, accessUser.data.UserId);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployees)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesPending(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }

                var returnData = await _EmployeeRepository.GetEmployeesPending(filter.PageNumber, filter.PageSize, accessUser.data.UserId);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployeesPending)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesApproved(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }

                var returnData = await _EmployeeRepository.GetEmployeesApproved(filter.PageNumber, filter.PageSize, accessUser.data.UserId);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployeesApproved)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesDisapproved(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }

                var returnData = await _EmployeeRepository.GetEmployeesDisapproved(filter.PageNumber, filter.PageSize, accessUser.data.UserId);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployeesDisapproved)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<PagedExcutedResult<IEnumerable<EmployeeVm>>> GetEmployeesDeleted(PaginationFilter filter, string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            long totalRecords = 0;
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.AuthorizationError).ToString(), ResponseCode.AuthorizationError.ToString());

                }
                var returnData = await _EmployeeRepository.GetEmployeesDeleted(filter.PageNumber, filter.PageSize, accessUser.data.UserId);
                if (returnData == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }
                if (returnData.data == null)
                {
                    return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.NotFound).ToString(), ResponseCode.NotFound.ToString());
                }

                totalRecords = returnData.totalRecords;

                var pagedReponse = PaginationHelper.CreatePagedReponse<EmployeeVm>(returnData.data, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Ok).ToString(), ResponseCode.Ok.ToString());

                return pagedReponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployeesDeleted)=====>{ex}");
                return PaginationHelper.CreatePagedReponse<EmployeeVm>(null, validFilter, totalRecords, _uriService, route, ((int)ResponseCode.Exception).ToString(), $"Unable to process the transaction, kindly contact us support");
            }
        }
        public async Task<ExecutedResult<EmployeeSindgleVm>> GetEmployeeById(long EmployeeId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {

            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<EmployeeSindgleVm>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }

                var repoResponse = await _EmployeeRepository.GetEmployeeById(EmployeeId, accessUser.data.CompanyId);
                if (repoResponse == null)
                {
                    return new ExecutedResult<EmployeeSindgleVm>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                return new ExecutedResult<EmployeeSindgleVm>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = repoResponse };
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeService (GetEmployeeById)=====>{ex}");
                return new ExecutedResult<EmployeeSindgleVm>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        private DataTable GenerateUserDataTableHeader()
        {
            System.Data.DataTable table = new System.Data.DataTable();

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "FirstName",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "StaffID",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "MiddleName",
                DataType = typeof(string)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "LastName",
                DataType = typeof(string)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "Email",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "DOB",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "ResumptionDate",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "OfficialMail",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "PhoneNumber",
                DataType = typeof(string)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "UnitID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "GradeID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "EmployeeTypeID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "BranchID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "EmploymentStatusID",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "RoleId",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "DepartmentId",
                DataType = typeof(long)
            });

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "CompanyId",
                DataType = typeof(long)
            });


            return table;
        }


        public static DateOnly ParseDate(string dob)
        {
            string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy" };
            DateOnly date;

            if (DateTime.TryParseExact(dob, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                date = new DateOnly(parsedDate.Year, parsedDate.Month, parsedDate.Day);
            }
            else
            {
                throw new Exception("Invalid date");
            }

            return date;
        }
    }
}
