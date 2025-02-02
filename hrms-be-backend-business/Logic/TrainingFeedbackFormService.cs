﻿using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Text;

namespace hrms_be_backend_business.Logic
{
    public class TrainingFeedbackFormService : ITrainingFeedbackFormService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<TrainingFeedbackFormService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ITrainingFeedbackFormRepository _trainingFeedbackFormRepository;


        public TrainingFeedbackFormService(IAccountRepository accountRepository, ILogger<TrainingFeedbackFormService> logger,
            ITrainingFeedbackFormRepository trainingFeedbackFormRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;
            _logger = logger;
            _accountRepository = accountRepository;
            _trainingFeedbackFormRepository = trainingFeedbackFormRepository;
            _companyrepository = companyrepository;
        }
        public async Task<BaseResponse> CreateSupervisorTrainingFeedbackForm(SupervisorTrainingFeedbackFormCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                if (string.IsNullOrEmpty(payload.CourseTitle))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Course title is required";
                    return response;
                }
                if (requester.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }

                var repoResponse = await _trainingFeedbackFormRepository.CreateSupervisorTrainingFeedbackForm(payload, requester.Username);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Supervisor Training Feedback form created successfully.";
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

        public async Task<BaseResponse> CreateTraineeTrainingFeedbackForm(TraineeTrainingFeedbackFormCreate payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                if (string.IsNullOrEmpty(payload.CourseTitle))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Course title is required";
                    return response;
                }
                if (requester.UserId < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name is required";
                    return response;
                }

                var repoResponse = await _trainingFeedbackFormRepository.CreateTraineeTrainingFeedbackForm(payload, requester.Username);
                if (!repoResponse.Contains("Success"))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = repoResponse;
                    return response;
                }

                response.Data = payload;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Trainee Training Feedback form created successfully.";
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
    }
}
