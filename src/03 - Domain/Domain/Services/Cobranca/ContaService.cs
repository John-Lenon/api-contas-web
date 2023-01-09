using Domain.Configurations;
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
        public ContaService(InjectorService injector) : base(injector)
        {
        }

        public void CalcularValorCorrigido(Conta entity)
        {
            var valorMulta = Math.Round((entity.ValorOriginal * (entity.Multa / 100)), 2);
            var valorTotalJurosDias = Math.Round((entity.ValorOriginal * (entity.JurosDia / 100) * entity.QuantidadeDiasAtraso), 2);
            entity.ValorCorrigido = Math.Round((entity.ValorOriginal + valorMulta + valorTotalJurosDias), 2);
        } 

        public bool ValidarConta(Conta entity)
        {
            if(entity is null)
            {
                NotificarErro("Algum dado da conta está num formato inválido.");
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
                        return;
                    }
                    else if (regra.DiasAtrasoMinimo == 4 && regra.DiasAtrasoMinimo <= entity.QuantidadeDiasAtraso &&
                        regra.DiasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = 3;
                        entity.JurosDia = 0.2M;
                        return;

                    }
                    else if (regra.DiasAtrasoMinimo == 11 && regra.DiasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = 5;
                        entity.JurosDia = 0.3M;
                        return;
                    }
                }
            }
            else
            {
                entity.QuantidadeDiasAtraso = 0;
                entity.Multa = 0;
                entity.JurosDia = 0;
            }
        }
    }
}
