using Microsoft.AspNetCore.Mvc;

namespace XpressHRMS.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
