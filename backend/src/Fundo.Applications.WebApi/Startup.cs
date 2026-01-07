using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

namespace Fundo.Applications.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddDbContext<LoanDbContext>(options =>
            {
                if (_environment.IsEnvironment("Testing"))
                {
                    options.UseInMemoryDatabase("LoanDbTest");
                }
                else
                {
                    options.UseSqlServer(_configuration.GetConnectionString("LoanDb"));
                }
            });

            services.Configure<ApiKeyOptions>(_configuration.GetSection(ApiKeyOptions.SectionName));

            services.AddCors(options =>
            {
                options.AddPolicy("Frontend", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseCors("Frontend");
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
            LoanDbInitializer.Initialize(context);
        }
    }
}
