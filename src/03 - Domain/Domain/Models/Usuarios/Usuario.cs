
using Domain.Entities.Cobranca;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities.Usuarios
{
    public class Usuario : IdentityUser
    {
        //public int Id { get; set; }
        public string Cpf { get; set; }
        //public string Nome { get; set; }
        //public string SobreNome { get; set; }
        //public string Email { get; set; }
        //public string Telefone { get; set; }
        //public string Senha { get; set; }

        public ICollection<Conta> Contas { get; set; }
    }
}
