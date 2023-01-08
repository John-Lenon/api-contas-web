using Data.Contexts;
using Data.Repository.Base;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Repositorys.Cobranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository.Cobranca
{
    public class ContaRepository : RepositoryBase<Conta>, IContaRepository
    {
        public ContaRepository(ContasWebContext context) : base(context)
        {
        }
    }
}
