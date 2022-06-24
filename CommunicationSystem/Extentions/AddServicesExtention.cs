using CommunicationSystem.Services.Services;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationSystem.Extentions
{
    public static class AddServicesExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IConfirmationTokenService, ConfirmationTokenService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IStudentMarkService, StudentMarkService>();
            services.AddScoped<IMessageService, MessageService>();
            return services;
        }
    }
}
