using Api.V1.Base;
using Application.DTOs.Usuario;
using Integration.Tests.Config;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integration.Tests.Configurations.ContextosCompartilhados
{
    // O contexto desta classe será compartilhado entre os testes
    public class IntegrationTestsFixure<TStartup> : IDisposable where TStartup : class
    {
        public readonly AppFactoryTests<TStartup> Factory;
        //public readonly WebApplicationFactory<ProgramTests> Factory; 
        public readonly HttpClient Client;

        public IntegrationTestsFixure()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
            };

            Factory = new AppFactoryTests<TStartup>();

            //Factory = new WebApplicationFactory<ProgramTests>();
            Client = Factory.CreateClient(clientOptions);
            RealizarLoginAsync().GetAwaiter().GetResult();
        }

        private async Task RealizarLoginAsync()
        {
            var jsonContent = new StringContent(
               JsonSerializer.Serialize(new UsuarioLoginDTO { Email = "teste@gmail.com", Password = "123456" }), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("api/v1/auth/login", jsonContent);

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsByteArrayAsync();

            var contentResponse = JsonSerializer.Deserialize<ResponseResultDTO<string>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", contentResponse.Dados);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
