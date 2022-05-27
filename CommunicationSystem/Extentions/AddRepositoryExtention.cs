using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationSystem.Extentions
{
    public static class AddRepositoryExtention
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            return services;
        }
    }
}
