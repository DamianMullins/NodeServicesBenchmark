using Microsoft.AspNetCore.Mvc;

namespace NodeServicesBenchmark.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult WithViewComponent()
        {
            return View();
        }
        public IActionResult NoViewComponent()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
