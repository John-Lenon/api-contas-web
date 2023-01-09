using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Cobranca
{
    public interface IRegraDiaAtrasoAppService
    {
        Task<List<TResult>> GetAsync<TFilter, TResult>(TFilter filter);
    }
}
