using Microsoft.AspNetCore.Mvc;
using NodeServicesBenchmark.Website.Models;

namespace NodeServicesBenchmark.Website.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Name = RouteNames.Home)]
        public IActionResult Index(ViewType viewType)
        {
            ViewData["viewType"] = viewType;
            return View();
        }

        [HttpGet(Name = RouteNames.Benchmarks)]
        public IActionResult Benchmarks(ViewType viewType)
        {
            ViewData["viewType"] = viewType;
            return View();
        }
    }
}
