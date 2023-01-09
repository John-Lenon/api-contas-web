using Api;
using Api.Base;
using Application.DTOs.Usuario;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixure<StartupApiTests>>
    {
    }

    public class IntegrationTestsFixure<TStartup> : IDisposable where TStartup : class
    {
        public readonly AppFactoryTests<TStartup> Factory;
        public readonly HttpClient Client;

        public IntegrationTestsFixure()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
            };

            Factory = new AppFactoryTests<TStartup>();
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

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", contentResponse.Data);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
