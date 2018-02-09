using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace NodeServicesBenchmark.Website.Services
{
    public interface ITemplateService
    {
        Task<IHtmlContent> GetTemplateAsync(string templateName, object options = null);
    }
}
