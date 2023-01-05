using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.AppContants;
using XpressHRMS.Data.DTO;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Data.Response;

namespace XpressHRMS.Business.Services.Logic
{
    public class SSOservice : ISSOservice
    {
        private readonly ILogger<SSOservice> _logger;
        private readonly IGenericRepository _genericRepository;
        public SSOservice(ILogger<SSOservice> logger, IGenericRepository genericRepository)
        {
            _logger = logger;
            _genericRepository = genericRepository;

        }

        public async Task<BaseResponse> Login(UserLoginDTO user)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                string URL = URLConstant.SSOBaseURL + URLConstant.Login;
                var client = new RestClient(URL);
                var validation = await _genericRepository.PostAsync<UserLoginDTO, BaseResponse>(URL, user);
                if (validation.Data != null)
                {
                   // var S_Response = JsonConvert.DeserializeObject<SSOResponse>(validation.Data.ToString());

                    response.ResponseCode = "";
                    response.ResponseMessage = "";
                    response.Data = validation.Data;
                    return response;

                }
                response.ResponseMessage = validation.ResponseMessage;
                return response;

            }
            catch (Exception ex)
            {
                return response;

            }

        }

    }
}
