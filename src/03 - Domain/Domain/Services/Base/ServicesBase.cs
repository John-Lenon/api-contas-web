using Domain.Configurations;
using Domain.Interfaces.Application;
using Domain.Interfaces.Services.Base;
using Domain.Utilities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Domain.Services.Base
{
    public class ServicesBase<TEntity> : IServicesBase<TEntity> where TEntity : class, new()
    {
        protected INotificador Notificador { get; }

        public ServicesBase(InjectorService injector)
        {
            Notificador = injector.GetService<INotificador>();
        }

        public bool ValidateFieldsEntity<TValidator>(TValidator validator, TEntity entity) where TValidator : AbstractValidator<TEntity> 
        {
            var abstractValidator = validator.Validate(entity);

            if (abstractValidator.IsValid) return true;

            foreach (var error in abstractValidator.Errors) 
                NotificarErro(error.ErrorMessage);
           
            return false;
        }

        public virtual void NotificarErro(string mensagem) =>
             Notificador.Add(new Notificacao(EnumTipoNotificacao.Erro, mensagem));

    }

    public class ServicesContainerDi 
    { 
        public TService GetService<TService>(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<TService>();
        }
    }
}
