using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.NodeServices;

namespace NodeServicesBenchmark.Website.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly INodeServices _nodeServices;

        public TemplateService(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public async Task<IHtmlContent> GetTemplateAsync(string templateName, object options = null)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentException("No value was provided for the template name", nameof(templateName));
            }

            var moduleName = "templates/templates.js";
            var functionName = "getTemplate";
            var language = CultureInfo.CurrentUICulture.Name;
            var rawTemplate = await _nodeServices.InvokeExportAsync<string>(moduleName, functionName, templateName, language, options);
            var template = new HtmlString(rawTemplate);

            return template;
        }
    }
}
