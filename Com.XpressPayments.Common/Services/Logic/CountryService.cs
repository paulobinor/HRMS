using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
    public class CountryService : ICountryService
    {
        private readonly ILogger<CountryService> _logger;
        private readonly ICountryCodeRepo _CountryCodeRepo;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public CountryService(ILogger<CountryService> logger, ICountryCodeRepo CountryRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _CountryCodeRepo = CountryRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<List<CountryCode>>> GetAllCounytries()
        {

            try
            {

                var result = await _CountryCodeRepo.GetAllCountries();
                if (result != null)
                {
                    return new BaseResponse<List<CountryCode>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<CountryCode>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllCountries() ===>{ex.Message}");
                return null;
            }

        }
 }  }
