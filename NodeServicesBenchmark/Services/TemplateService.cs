using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.NodeServices;

namespace NodeServicesBenchmark.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly INodeServices _nodeServices;

        public TemplateService(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public async Task<IHtmlContent> GetTemplateAsync(string moduleName, object options)
        {
            return new HtmlString(await _nodeServices.InvokeAsync<string>($"node_modules/@justeat/{moduleName}", options));
        }
    }
}
