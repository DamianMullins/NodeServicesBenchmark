using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NodeServicesBenchmark.Website.Extensions;
using NodeServicesBenchmark.Website.Models;
using NodeServicesBenchmark.Website.Models.Loops;
using NodeServicesBenchmark.Website.Services;

namespace NodeServicesBenchmark.Website.ViewComponents
{
    public class RandomLoopsViewComponent : ViewComponent
    {
        private readonly ITemplateService _templateService;
        private readonly ICachedTemplateService _cachedTemplateService;

        public RandomLoopsViewComponent(ITemplateService templateService, ICachedTemplateService cachedTemplateService)
        {
            _templateService = templateService;
            _cachedTemplateService = cachedTemplateService;
        }

        public async Task <IViewComponentResult> InvokeAsync(ViewType viewType)
        {
            var loopModel = new LoopModel()
                .GenerateLoopModel();

            switch (viewType)
            {
                case ViewType.RazorTemplate:
                    return View(loopModel);
                case ViewType.NodeServices:
                    return new HtmlContentViewComponentResult(
                        await _templateService.GetTemplateAsync("loops", loopModel));
                case ViewType.CachedNodeServices:
                    return new HtmlContentViewComponentResult(
                        await _cachedTemplateService.GetTemplateAsync("loops", loopModel));
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
