using Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.ILogic;
using Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo;
using Com.XpressPayments.Bussiness.Util;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LearningAndDevelopmentModuleService.Service.Logic
{
    internal class TrainingPlanService : ITrainingPlanService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingPlanService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly IMailService _mailService;

        public TrainingPlanService(IAccountRepository accountRepository, ILogger<TrainingPlanService> logger,
            ITrainingPlanRepository trainingPlanRepository, IAuditLog audit, ICompanyRepository companyrepository, IMailService mailService)
        {
            _audit = audit;
            _mailService = mailService;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingPlanRepository = trainingPlanRepository;
            _companyrepository = companyrepository;
        }
        public async Task<BaseResponse> CreateTrainingPlan(TrainingPlanCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (string.IsNullOrEmpty(payload.Name))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }
                if (payload.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }

                var repoResponse = await _trainingPlanRepository.CreateTrainingPlan(payload);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }
                var userDetails = await _accountRepository.FindUser(payload.UserId);


                //response.ResponseCode = "00";
                //response.ResponseMessage = "Record inserted successfully";
                //return response;

                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Training Plan created successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }

        public Task<BaseResponse> ApproveTrainingPlan(long TrainingPlanID, RequesterInfo requester)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> DisaproveTrainingPlan(TrainingPlanDisapproved payload, RequesterInfo requester)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetAllTrainingPlan(RequesterInfo requester)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetTrainingPlanById(long TrainingPlanID, RequesterInfo requester)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> GetTrainingPlanByUserId(long UserID, RequesterInfo requester)
        {
            throw new NotImplementedException();
        }
    }
}
