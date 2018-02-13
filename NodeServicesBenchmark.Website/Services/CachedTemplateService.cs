using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Caching.Memory;
using NodeServicesBenchmark.Website.Extensions;

namespace NodeServicesBenchmark.Website.Services
{
    public class CachedTemplateService : ICachedTemplateService
    {
        private readonly ITemplateService _templateService;
        private readonly IMemoryCache _memoryCache;

        public CachedTemplateService(ITemplateService templateService, IMemoryCache memoryCache)
        {
            _templateService = templateService;
            _memoryCache = memoryCache;
        }
        
        public async Task<IHtmlContent> GetTemplateAsync(string templateName, object options = null)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentException("No value was provided for the template name", nameof(templateName));
            }

            var templateKey = templateName.BuildTemplateKey(options);

            return await _memoryCache.GetOrCreateAsync(templateKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                var template = await _templateService.GetTemplateAsync(templateName, options);
                return template;
            });
        }
    }
}
