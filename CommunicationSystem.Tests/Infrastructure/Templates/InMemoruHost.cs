using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Tests.Infrastructure.DataInitializers;
using CommunicationSystem.Tests.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.Templates
{
    public class InMemoryHost
    {
        public HttpClient Client { get; set; }
        public CommunicationContext Context { get; set; }
        public InMemoryHost(string dbName)
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    //var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<CommunicationContext>));
                    //services.Remove(descriptor);
                    services.RemoveAll(typeof(DbContextOptions<CommunicationContext>));
                    services.AddDbContext<CommunicationContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });
                    var scope = services.BuildServiceProvider().CreateScope();
                    var context = scope.ServiceProvider.GetService<CommunicationContext>();
                    Context = context;
                    Context.Database.EnsureDeleted();
                    IntegrationTestAuthDataInitializer.Initialize(Context);
            });
            });
            factory.ClientOptions.BaseAddress = new Uri("https://192.168.64.21:5001");
            Client = factory.CreateClient();
        }
        public async Task AuthenticateAsync()
        {
            var user = new Login()
            {
                Email = "integration@test.now",
                Password = "MyPassword"
            };
            var data = HttpHelper.ConvertToHttpContent(user);
            var response = await Client.PostAsync("/api/auth", data);
            var result = await HttpHelper.GetDataAsync<AuthenticationResponse>(response);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.access_token);
        }
    }
}
