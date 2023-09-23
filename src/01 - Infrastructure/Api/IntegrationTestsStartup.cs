using Api.Configurations;
using Application.Configurations;
using Application.Extensions.Autorizacao;
using Data.Configurations;
using Domain.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Integration.Tests.Configurations
{
    public class IntegrationTestsStartup
    {
        public IntegrationTestsStartup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.Testing.json")
                    .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.WebApiConfig();

            services.AddIdentityConfiguration(Configuration);
            services.AddDependencyInjectionsApp();
            services.AddDependencyInjectionsDomain();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
            IServiceProvider services)
        {
            services.ConfigurarBancoDados();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
