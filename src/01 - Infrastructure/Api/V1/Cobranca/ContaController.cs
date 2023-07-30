using Api.Configurations;
using Api.Extensions.Atributos;
using Api.V1.Base;
using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.V1.Cobranca
{    
    [RouterController("conta")]
    [Authorize]
    [ApiVersion(ApiConfig.V1)]
    public class ContaController : MainController
    {
        private readonly IContaAppService _contaAppService;

        public ContaController(IServiceProvider serviceProvider,
            IContaAppService contaAppService) : base(serviceProvider)
        {
            _contaAppService = contaAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultDTO<IEnumerable<ContaGetDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResultDTO<ContaGetDTO>), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Listar contas", Tags = new[] { "Contas" })]
        public async Task<List<ContaGetDTO>> GetAsync([FromQuery] ContaFilterDTO filter)
        {
            return await _contaAppService.GetAsync<ContaFilterDTO, ContaGetDTO>(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseResultDTO<ContaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Cadastrar nova conta", Tags = new[] { "Contas" })]
        public async Task<ContaDTO> AddAsync([FromBody] ContaDTO conta)
        {
            return await _contaAppService.AddAsync(conta);
        }

        [HttpPut("{idConta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Atualizar conta", Tags = new[] { "Contas" })]
        public async Task<ContaDTO> UpdateAsync([FromRoute] int idConta, [FromBody] ContaDTO conta)
        {
            conta.Id = idConta;
            await _contaAppService.UpdateAsync(conta, new object[] { idConta });
            return conta;
        }

        [HttpDelete("{idConta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Deletar conta", Tags = new[] { "Contas" })]
        public async Task DeleteAsync([FromRoute] int idConta)
        {
            await _contaAppService.DeleteAsync(idConta);
        }
    }
}
