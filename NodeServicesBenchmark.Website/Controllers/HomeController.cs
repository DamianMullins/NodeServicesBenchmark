using Microsoft.AspNetCore.Mvc;

namespace NodeServicesBenchmark.Website.Controllers
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

        [Route("with-cached-template", Name = RouteNames.WithCachedTemplateServiceRoute)]
        public IActionResult WithCachedTemplateService()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
