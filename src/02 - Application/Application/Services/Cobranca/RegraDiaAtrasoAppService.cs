using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Application.Services.Base;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Repositorys.Cobranca;
using System;
using System.Linq.Expressions;

namespace Application.Services.Cobranca
{
    public class RegraDiaAtrasoAppService : ServiceAppBase<RegraDiaAtraso, IRegraDiaAtrasoRepository>, IRegraDiaAtrasoAppService
    {
        public RegraDiaAtrasoAppService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Expression<Func<RegraDiaAtraso, bool>> QueryGet(object filterBase)
        {
            var filter = (RegraDiaAtrasoFilterDTO)filterBase;
            return x =>
            (filter.Id == null || filter.Id == x.Id) &&
            (filter.DiasAtrasoMinimo == null || filter.DiasAtrasoMinimo == x.DiasAtrasoMinimo) &&
            (filter.DiasAtrasoMaximo == null || filter.DiasAtrasoMaximo == x.DiasAtrasoMaximo);
        }
    }
}
