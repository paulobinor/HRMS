using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
using System.Web.Http;
using RestSharp;
using Polly;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using RestSharp.Authenticators;
using System.Threading;

namespace XpressHRMS.Business.Services.Logic
{
    [Route("api/[controller]")]

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
                if (validation != null)
                {
                    var S_Response = JsonConvert.DeserializeObject<SSOResponse>(validation);
                   
                    response.ResponseCode = "";
                    response.ResponseMessage = "";
                    response.Data = S_Response;
                    return response;

                }
                return response;

            }
            catch (Exception ex)
            {
                return response;

            }

        }

    }
}
