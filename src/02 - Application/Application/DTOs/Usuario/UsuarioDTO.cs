namespace Application.DTOs.Usuario
{
    public class UsuarioAddDTO
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UsuarioLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }    
    }
}
