
using Domain.Entities.Cobranca;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities.Usuarios
{
    public class Usuario : IdentityUser
    {
        public string Cpf { get; set; }
        public ICollection<Conta> Contas { get; set; }
    }
}
