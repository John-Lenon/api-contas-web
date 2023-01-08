using Api.Configurations;
using Application.Configurations;
using Application.Extensions;
using AutoMapper;
using Domain.Configurations;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

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
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            var userManager = services.GetRequiredService<UserManager<Usuario>>();
            var usuarioInicial = userManager.FindByEmailAsync("teste@gmail.com").GetAwaiter().GetResult();

            if(usuarioInicial is null)
            {
                var user = new Usuario
                {
                    UserName = "John",
                    Email = "teste@gmail.com",
                    EmailConfirmed = true,                    
                };
                userManager.CreateAsync(user, "123456").GetAwaiter().GetResult();
            }

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
