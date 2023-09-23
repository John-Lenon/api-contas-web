using Domain.Enumerators.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Extensions.Atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutorizationContasWeb : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PermissoesContasWeb : Attribute, IAuthorizationFilter
    {
        private IEnumerable<string> _enumPermissoes { get; }

        public PermissoesContasWeb(params EnumPermissoes[] enumPermissoes)
        {
            _enumPermissoes = enumPermissoes.Select(x => x.ToString());
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var possuiTodasPermissoes = _enumPermissoes.All(permissao => context.HttpContext.User.Claims.Any(claim => claim.Value == permissao));
            if (!possuiTodasPermissoes)
            {
                context.Result = new ObjectResult(new { Message = "Acesso não autorizado" })
                {
                    StatusCode = 401
                };
                return;
            }
        }
    }
}
