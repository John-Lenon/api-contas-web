using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositorys.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> AddAsync(TEntity entity);

        Task SaveChavesAsync();
    }
}
