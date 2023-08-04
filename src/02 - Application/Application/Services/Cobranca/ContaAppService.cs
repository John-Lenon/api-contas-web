using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Application.Services.Base;
using Domain.Configurations;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Repositorys.Cobranca;
using Domain.Interfaces.Services.Cobranca;
using Domain.Utilities;
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
            if (!_contaService.ValidarInclusaoConta(entity)) return null;

            var listRegras = await ObterRegrasDiasAtrasoAsync();
            if (!OperacaoValida()) return null;

            _contaService.AplicarMultaContaAtrasada(entity, listRegras);
            _contaService.CalcularValorCorrigido(entity);

            var result = await AddAsync(entity);
            await _repository.SaveChavesAsync();

            return _autoMapper.Map<ContaDTO>(result);
        }


        public async Task UpdateAsync(ContaDTO dto, object[] ids)
        {
            dto = dto is null? new ContaDTO() : null;  
            var contaEntity = await _repository.GetByIdAsync(ids);
            _autoMapper.Map(dto, contaEntity);
            if (!_contaService.ValidarUpdateConta(contaEntity)) return;

            await UpdateAsync(contaEntity, true);
            return;
        }

        public override async Task DeleteAsync(int id, bool saveChanges = true)
        {
            await base.DeleteAsync(id, saveChanges);
            if (!OperacaoValida()) return;
            Notificar(EnumTipoNotificacao.Informacao, $"Conta {id} deletada com sucesso.");
        }

        protected override Expression<Func<Conta, bool>> GetExpressionFilter(object filterBase)
        {
            var filter = (ContaFilterDTO)filterBase;
            return x =>
            (filter.Id == null || filter.Id == x.Id) &&
            (filter.Nome == null || filter.Nome == x.Nome) &&
            (filter.Vencimento == null || filter.Vencimento == x.DataVencimento) &&
            (filter.VencimentoInicial == null || filter.VencimentoInicial <= x.DataVencimento) &&
            (filter.VencimentoFinal == null || filter.VencimentoFinal >= x.DataVencimento);
        }

        protected override Expression<Func<Conta, bool>> GetExpressionDelete(int idConta) => x => x.Id == idConta;

        public virtual async Task<IEnumerable<RegraDiaAtraso>> ObterRegrasDiasAtrasoAsync()
        {
            var listRegras = await _regrasDiasAtraso.GetAsync<RegraDiaAtrasoFilterDTO, RegraDiaAtrasoDTO>(new RegraDiaAtrasoFilterDTO());

            if (listRegras is null)
            {
                Notificar(EnumTipoNotificacao.Erro, "Nenhuma Regra para dias atrasados cadastrada.");
                return null;
            }
            return _autoMapper.Map<IEnumerable<RegraDiaAtraso>>(listRegras);
        }
    }
}
