using Data.Contexts;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Data.Configurations
{
    public static class BancosDadosExtentions
    {
        public static IServiceProvider ConfigurarBancoDados(this IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();            
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ContasWebContext>();
            dbContext.Database.Migrate();
            PrepararUsuarioInicial(serviceScope);                
            return services;                           
        }

        private static void PrepararUsuarioInicial(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var usuarioInicial = userManager.FindByEmailAsync("teste@gmail.com").GetAwaiter().GetResult();

            if (usuarioInicial is null)
            {
                var user = new Usuario
                {
                    UserName = "John",
                    Email = "teste@gmail.com",
                    EmailConfirmed = true,
                };
                userManager.CreateAsync(user, "123456").GetAwaiter().GetResult();
            }
        }
    }
}
