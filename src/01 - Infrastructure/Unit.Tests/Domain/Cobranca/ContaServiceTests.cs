using Application.Utility;
using Domain.Configurations;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Application;
using Domain.Services.Cobranca;
using Domain.Validations.Cobranca;
using Moq;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Unit.Tests.Domain.Cobranca
{
    [Collection(nameof(ContaServiceCollection))]
    public class ContaServiceTests
    {
        private readonly ContaServiceTestsFixture _contaFixture;
        private Notificador _notificador { get; }
        private Mock<InjectorService> _injectorService;
        private readonly ITestOutputHelper _testOutputHelper;

        public ContaServiceTests(ContaServiceTestsFixture contaFixture, ITestOutputHelper testOutputHelper)
        {
            _contaFixture = contaFixture;
            _injectorService = contaFixture.InjectorService;
            _testOutputHelper = testOutputHelper;
            _notificador = new Notificador();
            contaFixture.InjectorService.Setup(provider => provider.GetService<INotificador>()).Returns(_notificador);
        }
        
        [Theory(DisplayName = "Calculo Valor Corrigito")]
        [Trait("Cliente Service", null)]
        [InlineData(100.99, 2, 0.1, 2, 103.21)]
        [InlineData(4353.876, 3, 0.2, 6, 4536.75)]
        [InlineData(1234.73, 5, 0.3, 23, 1381.67)]
        public void ContaService_ValorCorrigido_CalcularNovoValor(decimal valorOriginal, decimal multa, decimal jurosDia,
            int quantidadeDiasAtraso, decimal resultado)
        {
            // Arrange 
            var contaService = new ContaService(_injectorService.Object);
            var entity = new Conta
            {
                ValorOriginal = valorOriginal,
                Multa = multa,
                JurosDia = jurosDia,
                QuantidadeDiasAtraso = quantidadeDiasAtraso
            };

            // Act
            contaService.CalcularValorCorrigido(entity);

            // Assert
            Assert.Equal(entity.ValorCorrigido, resultado);
        }

        [Fact(DisplayName = "Validar Conta")]
        [Trait("Cliente Service", null)]
        public void ContaService_ValidarConta_ValidacaoDeveFalhar()
        {
            // Arrange 
            var contaService = new ContaService(_injectorService.Object);

            // Act
            var result = contaService.ValidarInclusaoConta(null);

            // Assert
            Assert.True(_notificador.ListNotificacoes.Count() > 0);
            Assert.False(result);
        }

        [Fact(DisplayName = "Validar Campos da Conta")]
        [Trait("Cliente Service", null)]
        public void ContaService_ValidarCamposConta_ValidacaoDeveFalhar()
        {
            // Arrange 
            var contaService = new ContaService(_injectorService.Object);
            var contaEntity = new Conta
            {
                DataPagamento = new DateTime(2021, 9, 11),
                DataVencimento = new DateTime(1800, 1, 1),
                ValorOriginal = 0,
                Nome = " "
            };

            // Act
            var result = contaService.ValidateFieldsEntity(new ContasAddValidator(), contaEntity);

            // Assert
            Assert.True(_notificador.ListNotificacoes.Count() == 4);
            var mensagensNotificador = string.Join("", _notificador.ListNotificacoes.ToArray().Select(item => $"\n{item.Descricao}"));

            _testOutputHelper.WriteLine($"Mensagens do notificador: {mensagensNotificador}");
            Assert.False(result);
        }
    }
}
