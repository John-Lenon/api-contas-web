using Api.Configurations;
using Api.Extensions.Middlewares;
using Application.Configurations;
using Application.Extensions;
using Data.Configurations;
using Domain.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.WebApiConfig();
            services.AddSwaggerConfig();
            services.AddIdentityConfiguration(Configuration);
            services.AddDependencyInjectionsApp();
            services.AddDependencyInjectionsDomain();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
            IServiceProvider services)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            services.ConfigurarBancoDados();            
            app.UseCors("Production");

            app.UseMiddleware<MiddlewareException>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerConfig(provider);
        }
    }
}
