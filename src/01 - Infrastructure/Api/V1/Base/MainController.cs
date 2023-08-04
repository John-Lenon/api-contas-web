using AutoMapper;
using Domain.Interfaces.Application;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Api.V1.Base
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected INotificador Notificador { get; private set; }
        protected IMapper AutoMapper { get; private set; }

        public MainController(IServiceProvider serviceProvider)
        {
            Notificador = serviceProvider.GetRequiredService<INotificador>();
            AutoMapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ValidarModelState(context)) return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            if (result is null)
            {
                context.Result = CustomResponse<object>(null);
                return;
            }
            context.Result = CustomResponse(result.Value);
        }

        public IActionResult CustomResponse<TResponse>(TResponse contentResponse)
        {
            if (Notificador.ListNotificacoes.Count() >= 1)
            {
                var errosInternos = Notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.ErroInterno);
                if (errosInternos.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = errosInternos.ToArray() };
                    return new ObjectResult(result) { StatusCode = 500 };
                }

                var erros = Notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Erro);
                if (erros.Any())
                {
                    var result = new ResponseResultDTO<TResponse>(default) { Mensagens = erros.ToArray() };
                    return BadRequest(result);
                }

                var informacoes = Notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Informacao);
                if (informacoes.Any())
                    return Ok(new ResponseResultDTO<TResponse>(contentResponse) { Mensagens = informacoes.ToArray() });
            }

            return Ok(new ResponseResultDTO<TResponse>(contentResponse));
        }

        protected void NotificarErro(string mensagem) =>
             Notificador.Add(new Notificacao(EnumTipoNotificacao.Erro, mensagem));

        private bool ValidarModelState(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {                                
                if(!ValidarContentTypeRequest(modelState, context)) return false;

                var valoresInvalidosModelState = modelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid);
                if (valoresInvalidosModelState.Count() == 0) return true;

                ExtrairMensagensDeErroDaModelState(valoresInvalidosModelState, context);
                return false;
            }
            return true;
        }

        private void ExtrairMensagensDeErroDaModelState(IEnumerable<KeyValuePair<string, ModelStateEntry>> valoresInvalidosModelState,
            ActionExecutingContext context)
        {            
            var listaErros = new List<Notificacao>();
            foreach (var model in valoresInvalidosModelState)
            {
                var nomeCampo = model.Key.StartsWith("$.") ? model.Key.Substring(2) : model.Key;
                listaErros.Add(new Notificacao(EnumTipoNotificacao.Erro, $"Campo {nomeCampo} não está num formato válido."));
            }

            context.Result = new BadRequestObjectResult(new ResponseResultDTO<string>(null, listaErros.ToArray()));
        }

        private bool ValidarContentTypeRequest(ModelStateDictionary modelState, ActionExecutingContext context)
        {
            var valoresInvalidosModelState = modelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid);
            if (valoresInvalidosModelState.Count() == 1)
            {
                var model = valoresInvalidosModelState.First();
                var modelErro = model.Value.Errors.FirstOrDefault();

                if (model.Key == "" && modelErro.ErrorMessage == "")
                {
                    var result = new ResponseResultDTO<object>();
                    result.ContentTypeInvalido();
                    context.Result = new BadRequestObjectResult(result);
                    return false;
                }else if (model.Key == "")
                {
                    model.Value.ValidationState = ModelValidationState.Valid;
                    return true;
                }
            }
            return true;
        }
    }

    public class ResponseResultDTO<TResponse>
    {
        public TResponse Dados { get; set; }
        public Notificacao[] Mensagens { get; set; }

        public ResponseResultDTO(TResponse data, Notificacao[] notificacoes = null)
        {
            Dados = data;
            Mensagens = notificacoes;
        }
       
        public void ContentTypeInvalido()
        {
            Mensagens = new Notificacao[]
            {
                new Notificacao (EnumTipoNotificacao.Erro, "Content-Type inválido.")
            };
        }

        public ResponseResultDTO()
        {
        }
    }
}
