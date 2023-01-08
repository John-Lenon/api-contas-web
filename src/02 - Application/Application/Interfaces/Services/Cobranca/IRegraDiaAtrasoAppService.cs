using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Cobranca
{
    public interface IRegraDiaAtrasoAppService
    {
        Task<IEnumerable<TResult>> GetAsync<TFilter, TResult>(TFilter filter);
    }
}
