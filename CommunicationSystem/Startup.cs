using CommunicationSystem.Hubs;
using CommunicationSystem.Services.Interfaces;
using CommunicationSystem.Models;
using CommunicationSystem.Options;
using CommunicationSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using CommunicationSystem.Repositories.Interfaces;
using CommunicationSystem.Repositories;
using Newtonsoft.Json;

namespace CommunicationSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authOptions = Configuration.GetSection("Auth").Get<AuthOptions>();
            var connection = Configuration.GetConnectionString("PostgreSQL");

            //Configuring options
            services.Configure<AuthOptions>(Configuration.GetSection("Auth"));
            services.Configure<PathOptions>(Configuration.GetSection("Path"));
            services.Configure<SmtpOptions>(Configuration.GetSection("SmtpClient"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
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
                            // if hub is requested
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                ((path.StartsWithSegments("/messengerhub")) || (path.StartsWithSegments("/videochathub"))))
                            {
                                // getting token from request
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };

                });

            services.AddSignalR();

            //services.AddCors(); // Узнать где используется

            //Adding custom services

            services.AddSingleton<IUserIdProvider, EmailUserIdProvider>();
            services.AddScoped<IMailSender, MailService>();
            services.AddScoped<IFileSaver, FileService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IConfirmationToken, ConfirmationTokenService>();
            services.AddScoped<IRegistration, RegistrationService>();
            services.AddScoped<IUserActivity, UserActivityService>();
            services.AddScoped<IMessage, MessageService>();

            //Adding repositories

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICreateTestRepository, CreateTestRepository>();
            services.AddScoped<IMessengerRepository, MessengerRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();

            services.AddDbContext<CommunicationContext>(options =>
                        options.UseNpgsql(connection),
                        ServiceLifetime.Transient);

            services.AddControllersWithViews()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();

                app.UseSpaStaticFiles();
            }
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<VideoChatHub>("/videochathub");
                endpoints.MapHub<MessengerHub>("/messengerhub");
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
