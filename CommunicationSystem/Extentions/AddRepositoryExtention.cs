using CommunicationSystem.Services.Repositories;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationSystem.Extentions
{
    public static class AddRepositoryExtention
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICreateTestRepository, CreateTestRepository>();
            services.AddScoped<ICreateQuestionRepository, CreateQuestionRepository>();
            services.AddScoped<ICreateOptionRepository, CreateOptionRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            return services;
        }
    }
}
