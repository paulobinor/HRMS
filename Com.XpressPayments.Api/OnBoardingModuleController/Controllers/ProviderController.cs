using Microsoft.AspNetCore.Mvc;

namespace Com.XpressPayments.Api.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
