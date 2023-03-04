using Api.Base;
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
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/conta")]
    //[Authorize]
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
        [ProducesResponseType(typeof(ResponseResultDTO<ContaGetDTO>),StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Listar contas", Tags = new[] { "Contas" })]
        public async Task<IActionResult> GetAsync([FromQuery]ContaFilterDTO filter)
        {
            var listContas = await _contaAppService.GetAsync<ContaFilterDTO, ContaGetDTO>(filter);
            return CustomResponse(listContas);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseResultDTO<ContaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Cadastrar nova conta", Tags = new[] { "Contas" })]
        public async Task<IActionResult> AddAsync([FromBody] ContaDTO conta)
        {
            var result = await _contaAppService.AddAsync(conta);
            return CustomResponse(result);
        }

        [HttpPut("{idConta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Atualizar conta", Tags = new[] { "Contas" })]
        public async Task<IActionResult> UpdateAsync([FromRoute]int idConta, [FromBody] ContaDTO conta)
        {
            conta.Id = idConta;
            await _contaAppService.UpdateAsync(conta, new object[] { idConta });
            return CustomResponse<ContaDTO>(null);
        }

        [HttpDelete("{idConta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Deletar conta", Tags = new[] { "Contas" })]
        public async Task<IActionResult> DeleteAsync([FromRoute] int idConta)
        {
            await _contaAppService.DeleteAsync(idConta);
            return CustomResponse<ContaDTO>(null);
        }
    }
}
