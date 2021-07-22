using Microsoft.AspNetCore.Mvc;

namespace PerformPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("LoggedInView");
            }
            return View("LoggedOutView");
        }
    }
}
