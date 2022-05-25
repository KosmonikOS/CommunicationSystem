﻿using CommunicationSystem.Services.Services;
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
            return services;
        }
    }
}
