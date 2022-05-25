using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Options;
using CommunicationSystem.Extentions;
using Microsoft.Extensions.Options;
using MediatR;
using System.Reflection;

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
            //Variables
            var authOptions = Configuration.GetSection("Auth").Get<AuthOptions>();
            var spaOptions = Configuration.GetSection("Spa").Get<SpaOptions>();
            var connection = Configuration.GetConnectionString("PostgreSQL");

            //Configuring options
            services.Configure<AuthOptions>(Configuration.GetSection("Auth"));
            services.Configure<PathOptions>(Configuration.GetSection("Path"));
            services.Configure<SmtpOptions>(Configuration.GetSection("SmtpClient"));
            services.Configure<SpaOptions>(Configuration.GetSection("Spa"));
            services.Configure<MailOptions>(Configuration.GetSection("Mail"));

            //Authentication 
            services.AddCustomJwt(authOptions);

            //CustomServices
            services.AddServices();
            services.AddRepositories();

            services.AddSignalR();

            services.AddAutoMapper(Assembly.Load("CommunicationSystem.Domain"));

            services.AddMediatR(Assembly.Load("CommunicationSystem.Services"));

            services.AddDbContext<CommunicationContext>(options => options.UseNpgsql(connection));

            services.AddControllersWithViews()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = spaOptions.RootPath;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SpaOptions> spaOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();

                app.UseSpaStaticFiles();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<VideoChatHub>("/videochathub");
                //endpoints.MapHub<MessengerHub>("/messengerhub");
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = spaOptions.Value.SourcePath;

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
