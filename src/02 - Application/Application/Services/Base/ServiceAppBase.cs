using Application.Interfaces.Services.Usuario;
using AutoMapper;
using Domain.Configurations;
using Domain.Enumerators.Usuario;
using Domain.Interfaces.Application;
using Domain.Interfaces.Repositorys.Base;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
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
        protected IUserService _userService { get; }

        public ServiceAppBase(InjectorService injector)
        {
            _notificador = injector.GetService<INotificador>();
            _autoMapper = injector.GetService<IMapper>();
            _repository = injector.GetService<TRepository>();
            _userService = injector.GetService<IUserService>();
        }

        public virtual async Task<List<TResult>> GetAsync<TFilter, TResult>(TFilter filter)
        {
            var listResult = await _repository.Get(GetExpressionFilter(filter)).ToListAsync();
            if (listResult is null || !listResult.Any()) return null;
            return _autoMapper.Map<List<TResult>>(listResult);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _repository.AddAsync(entity);
            return result;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            var result = _repository.Update(entity);

            if (saveChanges) 
                await _repository.SaveChavesAsync();

            return result;
        }

        public virtual async Task DeleteAsync(int id, bool saveChanges = true)
        {
            var result = await _repository.DeleteAsync(GetExpressionDelete(id));
            if (result is null)
            {
                Notificar(EnumTipoNotificacao.Erro, "Conta não encontrada.");
                return;
            }
            if (saveChanges) 
                await _repository.SaveChavesAsync();
        }

        protected virtual bool PossuiPermissao(params EnumPermissoes[] permissoesParaValidar)
        {
            return _userService.PossuiPermissao(permissoesParaValidar);
        }

        protected virtual void Notificar(EnumTipoNotificacao tipo, string mensagem) =>
             _notificador.Add(new Notificacao(tipo, mensagem));

        public virtual bool OperacaoValida() =>
            !(_notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Erro 
            || item.Tipo == EnumTipoNotificacao.ErroInterno).Count() > 0);

        protected abstract Expression<Func<TEntity, bool>> GetExpressionFilter(object filter);

        protected abstract Expression<Func<TEntity, bool>> GetExpressionDelete(int idConta);
    }
}
