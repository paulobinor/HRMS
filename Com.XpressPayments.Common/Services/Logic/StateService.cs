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
    public class StateService : IStateService
    {
        private readonly ILogger<StateService> _logger;
        private readonly IStateCodeRepo _StateCodeRepo;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public StateService(ILogger<StateService> logger, IStateCodeRepo stateRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _StateCodeRepo = stateRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<List<StateCodeDTO>>> GetAllStateCountryID(int CountryID, int StateID)
        {

            try
            {

                var result = await _StateCodeRepo.GetAllStateCountryID(CountryID, StateID);
                if (result != null)
                {
                    return new BaseResponse<List<StateCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<StateCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllStateByCountryID() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<List<StateCodeDTO>>> GetAllState(int CountryID)
        {

            try
            {

                var result = await _StateCodeRepo.GetAllState(CountryID);
                if (result != null)
                {
                    return new BaseResponse<List<StateCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<StateCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllState() ===>{ex.Message}");
                return null;
            }

        }
    }
}
