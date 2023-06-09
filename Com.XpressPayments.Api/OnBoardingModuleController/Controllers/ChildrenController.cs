using Microsoft.AspNetCore.Mvc;

namespace Com.XpressPayments.Api.Controllers
{
    public class ChildrenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
