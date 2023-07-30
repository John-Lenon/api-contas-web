using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions.Atributos
{
    public class RouterControllerAttribute : RouteAttribute
    {
        public RouterControllerAttribute(string template) : base("api/v{version:ApiVersion}/" + template)
        {
        }
    }
}
