using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.JobDescription;
using Com.XpressPayments.Data.Repositories.UnitHead;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public class JobDescriptionService : IJobDescriptionService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<JobDescriptionService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IJobDescriptionRepository _jobDescriptionRepository;

        public JobDescriptionService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<JobDescriptionService> logger,
            IJobDescriptionRepository jobDescriptionRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _jobDescriptionRepository = jobDescriptionRepository;
            _companyrepository = companyrepository;
        }

        //public async Task<BaseResponse> CreateJobDescription(CreateJobDescriptionDTO creatDto, RequesterInfo requester)
        //{
        //    var response = new BaseResponse();
        //    try
        //    {
        //        string createdbyUserEmail = requester.Username;
        //        string createdbyUserId = requester.UserId.ToString();
        //        string RoleId = requester.RoleId.ToString();

        //        var ipAddress = requester.IpAddress.ToString();
        //        var port = requester.Port.ToString();

        //        var requesterInfo = await _accountRepository.FindUser(createdbyUserEmail);
        //        if (null == requesterInfo)
        //        {
        //            response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "Requester information cannot be found.";
        //            return response;
        //        }

        //        if (Convert.ToInt32(RoleId) != 1)
        //        {
        //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
        //            return response;
        //        }

        //        //validate UnitDto payload here 
        //        if (String.IsNullOrEmpty(creatDto.JobDescriptionName) || creatDto.CompanyID <= 0 )
        //            //|| creatDto.DepartmentID <= 0 ||
        //            //creatDto.HodID <= 0 || creatDto.UnitID <= 0)
        //        {
        //            response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Please ensure all required fields are entered.";
        //            return response;
        //        }

        //        var isExistsComp = await _companyrepository.GetCompanyById(creatDto.CompanyID);
        //        if (null == isExistsComp)
        //        {
        //            response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"Invalid Company suplied.";
        //            return response;
        //        }
        //        else
        //        {
        //            if (isExistsComp.IsDeleted)
        //            {
        //                response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
        //                response.ResponseMessage = $"The Company suplied is already deleted, HOD cannot be created under it.";
        //                return response;
        //            }
        //        }

        //        creatDto.JobDescriptionName = $"{creatDto.JobDescriptionName} ({isExistsComp.CompanyName})";

        //        var isExists = await _jobDescriptionRepository.GetAllJobDescription(creatDto.JobDescriptionName);
        //        if (null != isExists)
        //        {
        //            response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = $"JobDescription with name : {creatDto.UnitHeadName} already exists.";
        //            return response;
        //        }

        //        dynamic resp = await _unitHeadRepository.CreateUnitHead(creatDto, createdbyUserEmail);
        //        if (resp > 0)
        //        {
        //            //update action performed into audit log here

        //            var UnitHead = await _unitHeadRepository.GetUnitHeadByName(creatDto.UnitHeadName);

        //            response.Data = UnitHead;
        //            response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
        //            response.ResponseMessage = "UnitHead created successfully.";
        //            return response;
        //        }
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = "An error occured while Creating Unit. Please contact admin.";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception Occured: CreateUnitHead ==> {ex.Message}");
        //        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
        //        response.ResponseMessage = $"Exception Occured: CreateUnitHead ==> {ex.Message}";
        //        response.Data = null;
        //        return response;
        //    }
        //}
    }
}
