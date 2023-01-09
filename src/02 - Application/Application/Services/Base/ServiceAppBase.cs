using AutoMapper;
using Domain.Configurations;
using Domain.Interfaces.Application;
using Domain.Interfaces.Repositorys.Base;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services.Base
{
    public abstract class ServiceAppBase<TEntity, TRepository> where TRepository : class, IRepositoryBase<TEntity>
        where TEntity : class, new()
    {
        protected INotificador _notificador { get; }
        protected IMapper _autoMapper { get; }
        protected TRepository _repository { get; }

        public ServiceAppBase(InjectorService injector)
        {
            _notificador = injector.GetService<INotificador>();
            _autoMapper = injector.GetService<IMapper>();
            _repository = injector.GetService<TRepository>();
        }

        public virtual async Task<List<TResult>> GetAsync<TFilter, TResult>(TFilter filter)
        {
            var listResult = await _repository.Get(QueryGet(filter)).ToListAsync();
            if (listResult is null || !listResult.Any()) return null;
            return _autoMapper.Map<List<TResult>>(listResult);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _repository.AddAsync(entity);
            return result;
        }

        public virtual async Task UpdateAsync<TDto>(TDto dto, object[] ids, bool saveChanges = true)
        {
            var entity = await _repository.GetByIdAsync(ids);
            if(entity is null)
            {
                NotificarErro("Conta não encontrada.");
                return;
            }
            _autoMapper.Map(dto, entity);
            _repository.Update(entity);
            if (saveChanges) await _repository.SaveChavesAsync();
        }

        public virtual async Task DeleteAsync(int id, bool saveChanges = true)
        {
            var result = await _repository.DeleteAsync(QueryDelete(id));
            if (result is null)
            {
                NotificarErro("Conta não encontrada.");
                return;
            }
            if (saveChanges) await _repository.SaveChavesAsync();
        }

        protected virtual void NotificarErro(string mensagem) =>
             _notificador.Add(new Notificacao(EnumTipoNotificacao.Error, mensagem));

        public virtual bool OperacaoValida() =>
            !(_notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Error).Count() > 0);

        protected abstract Expression<Func<TEntity, bool>> QueryGet(object filter);

        protected abstract Expression<Func<TEntity, bool>> QueryDelete(int idConta);
    }
}
