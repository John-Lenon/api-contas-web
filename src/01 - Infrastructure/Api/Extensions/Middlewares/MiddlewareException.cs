using Api.V1.Base;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Extensions.Middlewares
{
    public class MiddlewareException
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environmentHost;

        public MiddlewareException(RequestDelegate next, IWebHostEnvironment environmentHost)
        {
            _next = next;
            _environmentHost = environmentHost;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = new ResponseResultDTO<string>();
                response.Mensagens = new Notificacao[]
                {
                    new Notificacao(
                        EnumTipoNotificacao.ErroInterno,
                        $"Erro interno no servidor.{(_environmentHost.IsDevelopment()? " " + ex.Message : "")}")
                };

                httpContext.Response.Headers.Add("content-type", "application/json; charset=utf-8");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
