using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
