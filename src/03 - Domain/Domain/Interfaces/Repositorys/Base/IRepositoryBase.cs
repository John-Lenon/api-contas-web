using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositorys.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> expression);

        void Update(TEntity entity);

        Task<TEntity> GetByIdAsync(params object[] ids);

        Task SaveChavesAsync();
    }
}
