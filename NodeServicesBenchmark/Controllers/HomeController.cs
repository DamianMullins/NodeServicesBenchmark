using Microsoft.AspNetCore.Mvc;

namespace NodeServicesBenchmark.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet(Name = RouteNames.Home)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("no-template", Name = RouteNames.NoTemplateServiceRoute)]
        public IActionResult NoTemplateService()
        {
            return View();
        }

        [Route("with-template", Name = RouteNames.WithTemplateServiceRoute)]
        public IActionResult WithTemplateService()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
