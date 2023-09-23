using Domain.Enumerators.Usuario;

namespace Application.Interfaces.Services.Usuario
{
    public interface IUserService
    {
        public string Name { get; }

        bool PossuiPermissao(params EnumPermissoes[] permissoesParaValidar);
    }
}
