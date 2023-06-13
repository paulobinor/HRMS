using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Gender;
using Com.XpressPayments.Data.Repositories.Institutions;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public class InstitutionsService : IInstitutionsService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<InstitutionsService> _logger;

        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IInstitutionsRepository _InstitutionsRepository;

        public InstitutionsService(IAccountRepository accountRepository, ILogger<InstitutionsService> logger,
            IInstitutionsRepository InstitutionsRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;

            _accountRepository = accountRepository;
            _InstitutionsRepository = InstitutionsRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> GetAllInstitutions(RequesterInfo requester)
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

                var Institutions = await _InstitutionsRepository.GetAllInstitutions();

                if (Institutions.Any())
                {
                    response.Data = Institutions;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Institutions fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Institutions found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllInstitutions() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllInstitutions() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
