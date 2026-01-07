using Fundo.Applications.WebApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Fundo.Services.Tests.Integration
{
    public class LoanApiFactory : WebApplicationFactory<Fundo.Applications.WebApi.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LoanDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<LoanDbContext>(options =>
                    options.UseInMemoryDatabase($"LoanDbTest-{Guid.NewGuid()}"));

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
                LoanDbInitializer.Initialize(context);
            });
        }
    }
}
