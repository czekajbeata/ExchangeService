using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using ExchangeService.Client.Services;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeService.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TokenService>();
            services.AddSingleton<ReloadService>();
            services.AddSingleton<AppState>();
            services.AddStorage();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
