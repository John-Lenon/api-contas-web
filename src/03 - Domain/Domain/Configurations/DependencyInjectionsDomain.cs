using Domain.Interfaces.Services.Cobranca;
using Domain.Services.Cobranca;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configurations
{
    public static class DependencyInjectionsDomain
    {
        public static IServiceCollection AddDependencyInjectionsDomain(this IServiceCollection services)
        {
            services.AddScoped<IContaService, ContaService>();
            return services;
        }
    }
}
