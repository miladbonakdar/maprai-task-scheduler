using Microsoft.AspNetCore.Mvc;

namespace MapraiScheduler.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}