using FluentValidation;

namespace Domain.Interfaces.Services.Base
{
    public interface IServicesBase<TEntity> where TEntity : class, new()
    {
        bool ValidateFieldsEntity<TValidator>(TValidator validator, TEntity entity) where TValidator : AbstractValidator<TEntity>;
    }
}
