using Api.Configurations;
using Api.Extensions.Atributos;
using Api.V1.Base;
using Application.Configurations;
using Application.DTOs.Usuario;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.V1.Autenticacao
{
    [RouterController("auth")]
    [ApiVersion(ApiConfig.V1)]
    public class AuthController : MainController
    {
        private readonly SignInManager<Usuario> _signManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly ConfiguracoesJwt _appSettings;

        public AuthController(IServiceProvider serviceProvider,
            SignInManager<Usuario> signManager,
            UserManager<Usuario> userManager,
            ConfiguracoesJwt appSettings) : base(serviceProvider)
        {
            _signManager = signManager;
            _userManager = userManager;
            _appSettings = appSettings;
        }

        [HttpPost("new-account")]
        [SwaggerOperation(Description = "Registrar usuário", Tags = new[] { "Autenticação" })]
        public async Task<object> Registrar([FromBody] UsuarioAddDTO usuario)
        {
            var user = new Usuario
            {
                UserName = usuario.Nome,
                Email = usuario.Email,
                EmailConfirmed = true,
                Cpf = usuario.Cpf,
            };

            var result = await _userManager.CreateAsync(user, usuario.Password);

            if (result.Succeeded)
                return await GerarJWT(usuario.Email);

            foreach (var error in result.Errors)
                NotificarErro(error.Description);

            return usuario;
        }

        [HttpPost("login")]
        [SwaggerOperation(Description = "Login usuário", Tags = new[] { "Autenticação" })]
        public async Task<object> Login(UsuarioLoginDTO loginUser)
        {
            var usuario = await _userManager.FindByEmailAsync(loginUser.Email);
            if (usuario is null)
            {
                NotificarErro("Usuário não encontrado.");
                return loginUser;
            }

            var resultLogin = await _signManager.PasswordSignInAsync(usuario.UserName, loginUser.Password, false, true);

            if (resultLogin.Succeeded)
            {
                return await GerarJWT(loginUser.Email);
            }
            if (resultLogin.IsLockedOut)
            {
                NotificarErro("Usuario temporariamente bloqueado por tentativas invalidas!");
                return loginUser;
            }

            NotificarErro("Usuario ou senha incorreto");
            return loginUser;
        }


        private async Task<string> GerarJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Emissor,
                //Audience = _appSettings.ValidoEm,                
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }
    }
}
