using Application.DTOs.Cobranca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Cobranca
{
    public interface IContaAppService
    {
        Task<List<TResult>> GetAsync<TFilter, TResult>(TFilter filter);
        Task<ContaDTO> AddAsync(ContaDTO contaDto);
        Task UpdateAsync(ContaDTO dto, object[] ids);
        Task DeleteAsync(int idConta, bool saveChanges = true);
    }
}
