using Data.Contexts;
using Data.Repository.Base;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Repositorys.Cobranca;

namespace Data.Repository.Cobranca
{
    public class RegraDiaAtrasoRepository : RepositoryBase<RegraDiaAtraso>, IRegraDiaAtrasoRepository
    {
        public RegraDiaAtrasoRepository(ContasWebContext contasWebContext) : base(contasWebContext)
        {
        }
    }
}
