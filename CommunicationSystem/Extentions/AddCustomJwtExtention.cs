using CommunicationSystem.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace CommunicationSystem.Extentions
{
    public static class AddCustomJwtExtention
    {
        public static IServiceCollection AddCustomJwt(this IServiceCollection services,AuthOptions authOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                ((path.StartsWithSegments("/messengerhub")) || 
                                (path.StartsWithSegments("/videochathub"))))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };

                });
            return services;
        }
    }
}
