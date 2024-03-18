using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    public class ChildrenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
