using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NodeServicesBenchmark.Website.Models;
using NodeServicesBenchmark.Website.Models.Loops;
using NodeServicesBenchmark.Website.Services;

namespace NodeServicesBenchmark.Website.ViewComponents
{
    public class LoopsViewComponent : ViewComponent
    {
        private readonly ITemplateService _templateService;
        private readonly ICachedTemplateService _cachedTemplateService;

        public LoopsViewComponent(ITemplateService templateService, ICachedTemplateService cachedTemplateService)
        {
            _templateService = templateService;
            _cachedTemplateService = cachedTemplateService;
        }

        public async Task <IViewComponentResult> InvokeAsync(ViewType viewType)
        {
            var loopItem = new Faker<LoopItem>()
                .RuleFor(l => l.Name, f => f.Company.CompanyName())
                .RuleFor(l => l.Address, f => f.Address.FullAddress())
                .RuleFor(l => l.Description, f => f.Company.CatchPhrase())
                .RuleFor(l => l.ImageUrl, _ => "https://loremflickr.com/g/320/240/brutalist")
                .RuleFor(l => l.Price, f => f.Finance.Amount());

            var loopModel = new Faker<LoopModel>()
                .RuleFor(l => l.LoopItems, f => loopItem.Generate(250))
                .Generate();

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
