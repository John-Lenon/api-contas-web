using AutoMapper;
using Domain.Interfaces.Application;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Api.Base
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

        protected IActionResult CustomResponse<TResponse>(TResponse contentResponse)
        {
            if (Notificador.ListNotificacoes.Count() >= 1)
            {
                var erros = Notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Error);

                if (erros.Any()) return BadRequest(new ResponseResultDTO(null)
                {
                    Mensagens = erros.ToArray()
                });

                var informacoes = Notificador.ListNotificacoes.Where(item => item.Tipo == EnumTipoNotificacao.Informacao);
                if (informacoes.Any())
                    return Ok(new ResponseResultDTO(contentResponse) { Mensagens = informacoes.ToArray() });
            }

            return Ok(new ResponseResultDTO(contentResponse));
        }

        protected void NotificarErro(string mensagem) =>
             Notificador.Add(new Notificacao(EnumTipoNotificacao.Error, mensagem));
    }

    public class ResponseResultDTO 
    {        
        public object Data { get; set; } 
        public Notificacao[] Mensagens { get; set; }

        public ResponseResultDTO(object data)
        {
            Data = data;
        }
    }
}
