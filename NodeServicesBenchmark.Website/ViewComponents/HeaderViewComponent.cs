using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NodeServicesBenchmark.Website.Models;
using NodeServicesBenchmark.Website.Services;

namespace NodeServicesBenchmark.Website.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ITemplateService _templateService;
        private readonly ICachedTemplateService _cachedTemplateService;

        public HeaderViewComponent(ITemplateService templateService, ICachedTemplateService cachedTemplateService)
        {
            _templateService = templateService;
            _cachedTemplateService = cachedTemplateService;
        }
        
        public async Task <IViewComponentResult> InvokeAsync(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.RazorTemplate:
                    return View();
                case ViewType.NodeServices:
                    return new HtmlContentViewComponentResult(
                        await _templateService.GetTemplateAsync("header"));
                case ViewType.CachedNodeServices:
                    return new HtmlContentViewComponentResult(
                        await _cachedTemplateService.GetTemplateAsync("header"));
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
