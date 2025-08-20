using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Infrastructure.Configuration;

namespace Portfolio.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure EmailJsSettings from appsettings.json
            services.Configure<EmailJsSettings>(configuration.GetSection("EmailJsSettings"));
            
            // Configure RecaptchaSettings from appsettings.json
            services.Configure<RecaptchaSettings>(configuration.GetSection("RecaptchaSettings"));

            return services;
        }
    }
}