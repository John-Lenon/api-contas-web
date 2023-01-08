using Api.Base;
using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Api.V1.Cobranca
{
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/conta")]
    [Authorize]
    public class ContaController : MainController
    {
        private readonly IContaAppService _contaAppService;

        public ContaController(IServiceProvider serviceProvider, 
            IContaAppService contaAppService) : base(serviceProvider)
        {
            _contaAppService = contaAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ContaGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Listar contas", Tags = new[] { "Contas" })]
        public async Task<IActionResult> GetAsync([FromQuery]ContaFilterDTO filter)
        {
            var listContas = await _contaAppService.GetAsync<ContaFilterDTO, ContaGetDTO>(filter);
            return CustomResponse(listContas);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Description = "Cadastrar nova conta", Tags = new[] { "Contas" })]
        public async Task<IActionResult> AddAsync([FromBody] ContaDTO conta)
        {
            var result = await _contaAppService.AddAsync(conta);
            return CustomResponse(result);
        }
    }
}
