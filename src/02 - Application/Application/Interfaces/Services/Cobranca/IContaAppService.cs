using Application.DTOs.Cobranca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Cobranca
{
    public interface IContaAppService
    {
        Task<IEnumerable<TResult>> GetAsync<TFilter, TResult>(TFilter filter);
        Task<ContaDTO> AddAsync(ContaDTO contaDto);
    }
}
