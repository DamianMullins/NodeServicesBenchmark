using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodeServicesBenchmark.Website.Services;

namespace NodeServicesBenchmark.Website
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();


            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                    opts => opts.ResourcesPath = "Resources");

            services.AddMemoryCache();
            services.AddNodeServices();

            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<ICachedTemplateService, CachedTemplateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: RouteNames.LoopBenchmarks,
                    template: "loop-benchmarks/{viewType}",
                    defaults: new { controller = "Home", action = "LoopBenchmarks" }
                );

                routes.MapRoute(
                    name: RouteNames.RandomLoopBenchmarks,
                    template: "random-loop-benchmarks/{viewType}",
                    defaults: new { controller = "Home", action = "RandomLoopBenchmarks" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
