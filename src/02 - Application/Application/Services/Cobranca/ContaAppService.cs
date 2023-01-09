using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Application.Services.Base;
using Domain.Configurations;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Repositorys.Cobranca;
using Domain.Interfaces.Services.Cobranca;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services.Cobranca
{
    public class ContaAppService : ServiceAppBase<Conta, IContaRepository>, IContaAppService
    {
        private readonly IContaService _contaService;
        private readonly IRegraDiaAtrasoAppService _regrasDiasAtraso;

        public ContaAppService(InjectorService injector,
            IContaService contaService,
            IRegraDiaAtrasoAppService regrasDiasAtraso) : base(injector)
        {
            _contaService = contaService;
            _regrasDiasAtraso = regrasDiasAtraso;
        }

        public async Task<ContaDTO> AddAsync(ContaDTO contaDto)
        {
            var entity = _autoMapper.Map<Conta>(contaDto);
            if (!_contaService.ValidarConta(entity)) return null;

            var listRegras = await ObterRegrasDiasAtrasoAsync();
            if (!OperacaoValida()) return null;

            _contaService.AplicarMultaContaAtrasada(entity, listRegras);
            _contaService.CalcularValorCorrigido(entity);

            var result = await AddAsync(entity);
            await _repository.SaveChavesAsync();

            return _autoMapper.Map<ContaDTO>(result);
        }

        protected override Expression<Func<Conta, bool>> QueryGet(object filterBase)
        {
            var filter = (ContaFilterDTO)filterBase;
            return x =>
            (filter.Id == null || filter.Id == x.Id) &&
            (filter.Nome == null || filter.Nome == x.Nome) &&
            (filter.Vencimento == null || filter.Vencimento == x.DataVencimento) &&
            (filter.VencimentoInicial == null || filter.VencimentoInicial <= x.DataVencimento) &&
            (filter.VencimentoFinal == null || filter.VencimentoFinal >= x.DataVencimento);
        }

        #region Metodos Privados 

        public virtual async Task<IEnumerable<RegraDiaAtraso>> ObterRegrasDiasAtrasoAsync()
        {
            var listRegras = await _regrasDiasAtraso.GetAsync<RegraDiaAtrasoFilterDTO, RegraDiaAtrasoDTO>(new RegraDiaAtrasoFilterDTO());

            if (listRegras is null)
            {
                NotificarErro("Nenhuma Regra para dias atrasados cadastrada.");
                return null;
            }
            return _autoMapper.Map<IEnumerable<RegraDiaAtraso>>(listRegras);
        }

        #endregion
    }
}
