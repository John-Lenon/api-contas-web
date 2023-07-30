using Domain.Entities.Cobranca;
using FluentValidation;
using System;

namespace Domain.Validations.Cobranca
{
    public class ContasAddValidator : AbstractValidator<Conta>
    {
        public ContasAddValidator()
        {
            var dataMinima = new DateTime(2000, 1, 1);

            RuleFor(e => e.Id)
                .Must(id => id == 0).WithMessage("O campo Id não pode ser definido manualmente");

            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("Campo Nome é obrigatório.")
                .MaximumLength(60).WithMessage("Campo Nome deve ter no máximo {MaxLength} caracteres.");

            RuleFor(e => e.ValorOriginal)
                .NotEmpty().WithMessage("Campo Valor Original é obrigatório.")
                .GreaterThan(0).WithMessage("Campo Valor Original deve ser maior que {ComparisonValue}.");

            RuleFor(e => e.DataVencimento)
                .NotEmpty().WithMessage("Campo Data Vencimento é obrigatório.")
                .Must(data => data >= dataMinima).WithMessage("Data Vencimento Inválida.");

            RuleFor(e => e.DataPagamento)
                .NotEmpty().WithMessage("Campo Data Pagamento é obrigatório.")
                .Must(data => data >= dataMinima).WithMessage("Data Pagamento Inválida.");                                      

            RuleFor(e => e.QuantidadeDiasAtraso)
                .GreaterThanOrEqualTo(0).WithName("O campo Quantidade Dias Atraso deve ser maior ou igual a {ComparisonValue}.");
            
            RuleFor(e => e.ValorCorrigido)
                .GreaterThanOrEqualTo(0).WithName("O campo Valor Corrigido deve ser maior ou igual a {ComparisonValue}.");

            RuleFor(e => e.Multa)
                .GreaterThanOrEqualTo(0).WithName("O campo Multa deve ser maior ou igual a {ComparisonValue}.");

            RuleFor(e => e.JurosDia)
                .GreaterThanOrEqualTo(0).WithName("O campo Juros Dia deve ser maior ou igual a {ComparisonValue}.");
        }
    }

    public class ContasUpdateValidator : AbstractValidator<Conta>
    {
        public ContasUpdateValidator()
        {
            var dataMinima = new DateTime(2000, 1, 1);

            RuleFor(e => e.Id)
                .Must(id => id != 0).WithMessage("O campo Id não pode ser zero");

            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("Campo Nome é obrigatório.")
                .MaximumLength(60).WithMessage("Campo Nome deve ter no máximo {MaxLength} caracteres.");

            RuleFor(e => e.ValorOriginal)
                .NotEmpty().WithMessage("Campo Valor Original é obrigatório.")
                .GreaterThan(0).WithMessage("Campo Valor Original deve ser maior que {ComparisonValue}.");

            RuleFor(e => e.DataVencimento)
                .NotEmpty().WithMessage("Campo Data Vencimento é obrigatório.")
                .Must(data => data >= dataMinima).WithMessage("Data Vencimento Inválida.");

            RuleFor(e => e.DataPagamento)
                .NotEmpty().WithMessage("Campo Data Pagamento é obrigatório.")
                .Must(data => data >= dataMinima).WithMessage("Data Pagamento Inválida.");

            RuleFor(e => e.QuantidadeDiasAtraso)
                .GreaterThanOrEqualTo(0).WithName("O campo Quantidade Dias Atraso deve ser maior ou igual a {ComparisonValue}.");

            RuleFor(e => e.ValorCorrigido)
                .GreaterThanOrEqualTo(0).WithName("O campo Valor Corrigido deve ser maior ou igual a {ComparisonValue}.");

            RuleFor(e => e.Multa)
                .GreaterThanOrEqualTo(0).WithName("O campo Multa deve ser maior ou igual a {ComparisonValue}.");

            RuleFor(e => e.JurosDia)
                .GreaterThanOrEqualTo(0).WithName("O campo Juros Dia deve ser maior ou igual a {ComparisonValue}.");
        }
    }
}
