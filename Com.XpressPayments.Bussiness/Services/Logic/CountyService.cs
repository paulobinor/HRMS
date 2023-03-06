
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Branch;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.CountryStateLga;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public  class CountyService : ICountyService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<CountyService> _logger;
       
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly ICountryRepository _countryRepository;

        public CountyService( IAccountRepository accountRepository, ILogger<CountyService> logger,
            ICountryRepository countryRepository, IAuditLog audit, ICompanyRepository companyrepository)
        {
            _audit = audit;

            _logger = logger;
           
            _accountRepository = accountRepository;
            _countryRepository = countryRepository;
            _companyrepository = companyrepository;
        }

        public async Task<BaseResponse> GetAllCountry(RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) > 2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                //update action performed into audit log here

                var Departments = await _countryRepository.GetAllCountries();

                if (Departments.Any())
                {
                    response.Data = Departments;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Country fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Country found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllCountries() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllCountries() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }
    }
}
