using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Caching.Memory;

namespace NodeServicesBenchmark.Services
{
    public class CachedTemplateService : ICachedTemplateService
    {
        private readonly INodeServices _nodeServices;
        private readonly IMemoryCache _memoryCache;

        public CachedTemplateService(INodeServices nodeServices, IMemoryCache memoryCache)
        {
            _nodeServices = nodeServices;
            _memoryCache = memoryCache;
        }

        public async Task<IHtmlContent> GetTemplateAsync(string moduleName, object options)
        {
            return await _memoryCache.GetOrCreateAsync(moduleName, async _ =>
            {
                var template = new HtmlString(await _nodeServices.InvokeAsync<string>($"node_modules/@justeat/{moduleName}", options));
                return template;
            });
        }
    }
}
