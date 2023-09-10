using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace hrms_be_backend_business.Logic
{
    public class ResignationClearanceService : IResignationClearanceService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResignationClearanceService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IResignationClearanceRepository _resignationClearanceRepository;

        public ResignationClearanceService(IConfiguration config, IResignationClearanceRepository resignationClearanceRepository, ILogger<ResignationClearanceService> logger, IAccountRepository accountRepository)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _resignationClearanceRepository = resignationClearanceRepository;
        }


        public async Task<BaseResponse> SubmitResignationClearance(RequesterInfo requesterInfo, ResignationClearanceVM payload)
        {
            string traceID = Guid.NewGuid().ToString();
            try
            {
                _logger.LogInformation($"IncomingRequest TraceID --- {traceID} Body ---- {JsonConvert.SerializeObject(payload)}");
                var errorMessages = String.Empty;
                StringBuilder errorBuilder = new StringBuilder();

                if (payload.LastDayOfWork < DateTime.Now)
                    errorMessages = errorMessages + "|Invalid Last Day of work";
                if (string.IsNullOrWhiteSpace(payload.ReasonForResignation))
                    errorMessages = errorMessages + "|Resignation reason is required";
                

                if (errorMessages.Length > 0)
                    return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = errorMessages.Remove(0, 1) };

               


                payload.UserID = requesterInfo.UserId;

                var resignationClearance = new ResignationClearanceDTO
                {
                    //Date = payload.Date,
                    //DateCreated = DateTime.Now,
                    //Created_By_User_Email = requesterInfo?.Username,
                    LastDayOfWork = payload.LastDayOfWork,
                    UserID = payload.UserID,
                    SRFID = payload.SRFID,
                    ReasonForResignation = payload.ReasonForResignation,
                };


                var resp = await _resignationClearanceRepository.CreateResignationClearance(resignationClearance);

                //if (resp < 1)
                //{
                //    switch (resp)
                //    {
                //        case -1:
                //            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Interview Record already exist" };


                //        default:
                //            return new BaseResponse { ResponseCode = ((int)ResponseCode.ValidationError).ToString(), ResponseMessage = "Processing error" };
                //    }
                //}

                return new BaseResponse { ResponseCode = ((int)ResponseCode.Ok).ToString(), ResponseMessage = "Resignation Clearance submitted successfully", Data = resignationClearance };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation Clearance", ex);
                return new BaseResponse { ResponseCode = ((int)ResponseCode.Exception).ToString(), ResponseMessage = "An error occured. We are currently looking into it." };
            }
        }
    }
}
