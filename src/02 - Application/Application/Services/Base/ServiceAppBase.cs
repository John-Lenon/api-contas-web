using AutoMapper;
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

        public ServiceAppBase(IServiceProvider serviceProvider)
        {
            _notificador = serviceProvider.GetRequiredService<INotificador>();
            _autoMapper = serviceProvider.GetRequiredService<IMapper>();
            _repository = serviceProvider.GetRequiredService<TRepository>();
        }

        public async Task<IEnumerable<TResult>> GetAsync<TFilter, TResult>(TFilter filter)
        {
            var listResult = await _repository.Get(QueryGet(filter)).ToListAsync();
            return _autoMapper.Map<IEnumerable<TResult>>(listResult);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return await _repository.AddAsync(entity);
        }

        protected void NotificarErro(string mensagem) =>
             _notificador.Add(new Notificacao(EnumTipoNotificacao.Error, mensagem));

        protected bool OperacaoValida() =>
            !(_notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Error).Count() > 0);

        protected virtual Expression<Func<TEntity, bool>> QueryGet(object filter) => x => true;
    }
}
