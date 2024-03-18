using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hrms_be_backend_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : BaseController
    {       
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {           
            _countryService = countryService;
        }

        [HttpGet("GetCountries")]
        [ProducesResponseType(typeof(ExecutedResult<List<CountryVm>>), 200)]
        public async Task<IActionResult> GetCountries()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var RemotePort = Request.HttpContext.Connection.RemotePort.ToString();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            var route = Request.Path.Value;
            return this.CustomResponse(await _countryService.GetCountries(accessToken, claim, RemoteIpAddress, RemotePort));
        }
    }
}
