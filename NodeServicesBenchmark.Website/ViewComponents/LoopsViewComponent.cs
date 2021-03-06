﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Caching.Memory;
using NodeServicesBenchmark.Website.Extensions;
using NodeServicesBenchmark.Website.Models;
using NodeServicesBenchmark.Website.Models.Loops;
using NodeServicesBenchmark.Website.Services;

namespace NodeServicesBenchmark.Website.ViewComponents
{
    public class LoopsViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ITemplateService _templateService;
        private readonly ICachedTemplateService _cachedTemplateService;

        public LoopsViewComponent(IMemoryCache memoryCache, ITemplateService templateService, ICachedTemplateService cachedTemplateService)
        {
            _memoryCache = memoryCache;
            _templateService = templateService;
            _cachedTemplateService = cachedTemplateService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ViewType viewType)
        {
            var loopModel = GenerateLoopItemList();

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


        private LoopModel GenerateLoopItemList()
        {
            return _memoryCache.GetOrCreate("loop-list", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                return new LoopModel()
                    .GenerateLoopModel();
            });
        }
    }
}
