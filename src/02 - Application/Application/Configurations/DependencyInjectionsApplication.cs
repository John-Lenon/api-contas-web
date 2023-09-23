using Application.Interfaces.Services.Cobranca;
using Application.Interfaces.Services.Usuario;
using Application.Services.Cobranca;
using Application.Services.Usuario;
using Application.Utility;
using Data.Repository.Cobranca;
using Domain.Interfaces.Application;
using Domain.Interfaces.Repositorys.Cobranca;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Configurations
{
    public static class DependencyInjectionsApplication
    {
        public static IServiceCollection AddDependencyInjectionsApp(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());           
            services.AddScoped<IContaAppService, ContaAppService>();
            services.AddScoped<IRegraDiaAtrasoAppService, RegraDiaAtrasoAppService>();
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IRegraDiaAtrasoRepository, RegraDiaAtrasoRepository>();
            services.AddScoped<IContaRepository, ContaRepository>();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
