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

        public bool ValidarInclusaoConta(Conta entity)
        {
            if(entity is null)
            {
                NotificarErro("Conta não localizada");
                return false;
            }
            if (!ValidateFieldsEntity(new ContasAddValidator(), entity)) return false;

            return true;
        }

        public bool ValidarUpdateConta(Conta entity)
        {
            if (entity is null)
            {
                NotificarErro("Conta não localizada.");
                return false;
            }
            if (!ValidateFieldsEntity(new ContasUpdateValidator(), entity)) return false;

            return true;
        }

        public void AplicarMultaContaAtrasada(Conta entity, IEnumerable<RegraDiaAtraso> listRegrasAtraso)
        {
            if (entity.ContaAtrasada)
            {
                entity.QuantidadeDiasAtraso = (entity.DataPagamento - entity.DataVencimento).Days;
                foreach (var regra in listRegrasAtraso)
                {
                    var diasAtrasoMaximo = regra.DiasAtrasoMaximo == 0 ? int.MaxValue : regra.DiasAtrasoMaximo;
                    if (regra.DiasAtrasoMinimo <= entity.QuantidadeDiasAtraso && diasAtrasoMaximo >= entity.QuantidadeDiasAtraso)
                    {
                        entity.Multa = regra.Multa;
                        entity.JurosDia = regra.JurosDia;
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
