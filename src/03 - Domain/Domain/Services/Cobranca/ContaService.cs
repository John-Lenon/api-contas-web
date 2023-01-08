using Domain.Entities.Cobranca;
using Domain.Interfaces.Services.Cobranca;
using Domain.Services.Base;
using Domain.Validations.Cobranca;
using System;
using System.Collections.Generic;

namespace Domain.Services.Cobranca
{
    public class ContaService : ServicesBase<Conta>, IContaService
    {
        public ContaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void CalcularValorCorrigido(Conta entity)
        {
            var valorMulta = entity.ValorOriginal * entity.Multa;
            var valorTotalJurosDias = entity.ValorOriginal * entity.JurosDia * entity.QuantidadeDiasAtraso;
            entity.ValorCorrigido = entity.ValorOriginal + valorMulta + valorTotalJurosDias;
        } 

        public bool ValidarConta(Conta entity)
        {
            if(entity is null)
            {
                NotificarErro("Conta inválida.");
                return false;
            }
            if (!ValidateFieldsEntity(new ContasAddValidator(), entity)) return false;

            return true;
        }

        public void AplicarMultaContaAtrasada(Conta entity, IEnumerable<RegraDiaAtraso> listRegrasAtraso)
        {
            if (entity.ContaAtrasada)
            {
                entity.QuantidadeDiasAtraso = (entity.DataPagamento - entity.DataVencimento).Days;
                foreach (var regra in listRegrasAtraso)
                {
                    if (regra.DiasAtrasoMinimo == 1 && regra.DiasAtrasoMinimo <= entity.QuantidadeDiasAtraso &&
                        regra.DiasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = 2;
                        entity.JurosDia = 0.1M;
                    }
                    else if (regra.DiasAtrasoMinimo == 4 && regra.DiasAtrasoMinimo <= entity.QuantidadeDiasAtraso &&
                        regra.DiasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = 3;
                        entity.JurosDia = 0.2M;

                    }
                    else if (regra.DiasAtrasoMinimo == 11 && regra.DiasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = 5;
                        entity.JurosDia = 0.3M;
                    }
                }
            }
        }
    }
}
