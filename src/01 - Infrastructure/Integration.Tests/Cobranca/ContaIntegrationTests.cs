using Api.V1.Base;
using Application.DTOs.Cobranca;
using Bogus;
using Domain.Entities.Cobranca;
using Integration.Tests.Configurations;
using Integration.Tests.Configurations.ContextosCompartilhados;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Tests.Cobranca
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class ContaIntegrationTests
    {
        private readonly IntegrationTestsFixure<IntegrationTestsStartup> _testsFixture;

        public ContaIntegrationTests(IntegrationTestsFixure<IntegrationTestsStartup> testsFixure)
        {
            _testsFixture = testsFixure;
        }
      
        [Trait("Teste Integracao", "Contas")]
        [Fact(DisplayName = "Cadastrar Conta")]
        public async Task Contas_RealizarCadastro_DeveCadastrarComSucesso()
        {
            // Arrange
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(GerarConta()), Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync("api/v1/conta", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsByteArrayAsync();

            var contentResponse = JsonSerializer.Deserialize<ResponseResultDTO<ContaDTO>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            Assert.NotNull(contentResponse?.Dados);
        }

        [Trait("Teste Integracao", "Contas")]
        [Fact(DisplayName = "Falhar ao cadastrar conta")]
        public async Task Contas_RealizarCadastro_DeveFalhar()
        {          
            // Arrange
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(GerarContaInvalida()), Encoding.UTF8, "application/json");

            // Act
            var response = await _testsFixture.Client.PostAsync("api/v1/conta", jsonContent);

            // Assert
            var stream = await response.Content.ReadAsByteArrayAsync();

            var contentResponse = JsonSerializer.Deserialize<ResponseResultDTO<ContaDTO>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Null(contentResponse.Dados);
            Assert.True(contentResponse.Mensagens.Count() > 0);                       
        }

        public Conta GerarConta()
        {
            var contaGenerator = new Faker<Conta>("pt_BR");

            contaGenerator.CustomInstantiator(faker => new Conta
            {
                DataPagamento = faker.Date.Between(new DateTime(2021, 1, 1), new DateTime(2024, 1, 1)),
                DataVencimento = faker.Date.Between(new DateTime(2021, 1, 1), new DateTime(2024, 1, 1)),
                Nome = faker.Name.FirstName(),
                ValorOriginal = faker.Random.Decimal(3, 10),
            });
            return contaGenerator.Generate();
        }

        public Conta GerarContaInvalida()
        {
            return new Conta
            {
                DataPagamento = new DateTime(1800, 1, 1),
                DataVencimento = new DateTime(2023, 12, 12),
                Nome = " ",
                ValorOriginal = 0,
            };
        }
    }
}
