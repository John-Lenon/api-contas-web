using Domain.Entities.Cobranca;
using Domain.Interfaces.Services.Base;
using System.Collections.Generic;

namespace Domain.Interfaces.Services.Cobranca
{
    public interface IContaService : IServicesBase<Conta>
    {
        bool ValidarConta(Conta entity);
        void CalcularValorCorrigido(Conta entity);
        void AplicarMultaContaAtrasada(Conta entity, IEnumerable<RegraDiaAtraso> listRegrasAtraso);
    }
}
