using Data.Contexts;
using Domain.Interfaces.Repositorys.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Base
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity: class, new()
    {       
        protected ContasWebContext ContasWebContext { get; }
        protected DbSet<TEntity> DbSet { get; }

        public RepositoryBase(ContasWebContext contasWebContext)
        {
            ContasWebContext = contasWebContext;
            DbSet = ContasWebContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
        {
            var result = DbSet.Where(expression);
            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task SaveChavesAsync() => await ContasWebContext.SaveChangesAsync();
    }
}
