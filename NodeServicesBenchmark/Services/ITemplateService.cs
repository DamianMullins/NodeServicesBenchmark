using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace NodeServicesBenchmark.Services
{
    public interface ITemplateService
    {
        Task<IHtmlContent> GetTemplateAsync(string moduleName, object options);
    }
}
