using Microsoft.AspNetCore.Mvc;

namespace WatchMe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
