using Microsoft.AspNetCore.Mvc;

namespace hrms_be_backend_api.Controllers
{
    public class ChildrenController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
