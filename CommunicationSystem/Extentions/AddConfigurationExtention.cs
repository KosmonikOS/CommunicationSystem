using CommunicationSystem.Domain.Options;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationSystem.Extentions
{
    public static class AddConfigurationExtention
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<AuthOptions>(configuration.GetSection("Auth"));
            services.Configure<PathOptions>(configuration.GetSection("Path"));
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpClient"));
            services.Configure<SpaOptions>(configuration.GetSection("Spa"));
            services.Configure<MailOptions>(configuration.GetSection("Mail"));
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 209715200;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 209715200;
                x.MultipartBodyLengthLimit = 209715200;
            });
            return services;
        }
    }
}
