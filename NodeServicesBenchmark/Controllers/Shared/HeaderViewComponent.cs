using Microsoft.AspNetCore.Mvc;

namespace NodeServicesBenchmark.Controllers.Shared
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
