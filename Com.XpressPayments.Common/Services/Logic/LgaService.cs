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
    public class LgaService : ILgaService
    {
        private readonly ILogger<LgaService> _logger;
        private readonly ILgaCodeRepo _LgaCodeRepo;
        private readonly IAuditTrailRepository _auditTrailRepository;

        public LgaService(ILogger<LgaService> logger, ILgaCodeRepo LgaRepository, IAuditTrailRepository auditTrailRepository)
        {
            _logger = logger;
            _LgaCodeRepo = LgaRepository;
            _auditTrailRepository = auditTrailRepository;

        }

        public async Task<BaseResponse<List<LgaCodeDTO>>> GetAllLGAByState( int StateID, int LGAID)
        {

            try
            {

                var result = await _LgaCodeRepo.GetAllLGAbyStateId(StateID, LGAID);
                if (result != null)
                {
                    return new BaseResponse<List<LgaCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<LgaCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLGA() ===>{ex.Message}");
                return null;
            }

        }

        public async Task<BaseResponse<List<LgaCodeDTO>>> GetAllLGA(int StateID)
        {

            try
            {

                var result = await _LgaCodeRepo.GetAllLGA(StateID);
                if (result != null)
                {
                    return new BaseResponse<List<LgaCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "Record Retreived Successfully",
                        Data = result

                    };

                }
                else
                {
                    return new BaseResponse<List<LgaCodeDTO>>()
                    {
                        ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0'),
                        ResponseMessage = "No record found",
                        Data = result

                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllLGA() ===>{ex.Message}");
                return null;
            }

        }
    }
}
