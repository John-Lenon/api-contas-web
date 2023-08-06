using Application.DTOs.Cobranca;
using Application.Interfaces.Services.Cobranca;
using Application.Services.Cobranca;
using Application.Utility;
using AutoMapper;
using Domain.Configurations;
using Domain.Entities.Cobranca;
using Domain.Interfaces.Application;
using Domain.Interfaces.Repositorys.Cobranca;
using Domain.Interfaces.Services.Cobranca;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Unit.Tests.Application.Cobranca
{
    [Collection(nameof(ContaAppServiceCollection))]
    public class ContaAppServiceTests
    {
        private Mock<InjectorService> _injectorServiceMock { get; set; }
        private Mock<IContaService> _contaServiceMock { get; set; }
        private Mock<IRegraDiaAtrasoAppService> _regraDiaAtrasoAppServiceMock { get; set; }
        private Mock<IMapper> _autoMapperMock { get; } = new Mock<IMapper>();
        private Mock<IContaRepository> _contaRepositoryMock { get; } = new Mock<IContaRepository>();
        private Notificador Notificador { get; }

        public ContaAppServiceTests(ContaAppServiceFixture contaAppService)
        {
            _injectorServiceMock = contaAppService.InjectorServiceMock;
            _contaServiceMock = contaAppService.ContaServiceMock;
            _regraDiaAtrasoAppServiceMock = contaAppService.RegraDiaAtrasoAppServiceMock;

            Notificador = new Notificador();
            _injectorServiceMock.Setup(p => p.GetService<INotificador>()).Returns(Notificador);
            _injectorServiceMock.Setup(p => p.GetService<IMapper>()).Returns(_autoMapperMock.Object);
            _injectorServiceMock.Setup(p => p.GetService<IContaRepository>()).Returns(_contaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Adicionar Conta com sucesso")]
        [Trait("Cliente App Service", null)]
        public async Task ContaAppService_AddAsync_CadastrarContaComSucesso()
        {
            // Arrange
            var contaEntity = new Conta();
            var contaDto = new ContaDTO();
            IEnumerable<RegraDiaAtraso> listRegras = new List<RegraDiaAtraso>() { new RegraDiaAtraso { Id = 2 } };

            var contaAppServiceTest = new Mock<ContaAppService>(_injectorServiceMock.Object, _contaServiceMock.Object, _regraDiaAtrasoAppServiceMock.Object);
            contaAppServiceTest.Setup(e => e.ObterRegrasDiasAtrasoAsync()).Returns(Task.FromResult(listRegras));
            contaAppServiceTest.Setup(e => e.OperacaoValida()).Returns(true);

            SimularRetornoMetodosDependenciasCadastroSucesso(contaEntity, contaDto);

            // Act
            await contaAppServiceTest.Object.AddAsync(contaDto);

            // Asserts            
            _contaServiceMock.Verify(conta => conta.ValidarInclusaoConta(contaEntity), Times.Once());
            _contaRepositoryMock.Verify(repo => repo.SaveChavesAsync(), Times.Once());
            _contaServiceMock.Verify(repo => repo.AplicarMultaContaAtrasada(contaEntity, listRegras), Times.Once());
            _contaServiceMock.Verify(repo => repo.CalcularValorCorrigido(contaEntity), Times.Once());
            contaAppServiceTest.Verify(p => p.ObterRegrasDiasAtrasoAsync(), Times.Once());
            contaAppServiceTest.Verify(p => p.AddAsync(contaEntity), Times.Once());
        }

        private void SimularRetornoMetodosDependenciasCadastroSucesso(Conta contaEntity, ContaDTO contaDto)
        {
            _autoMapperMock.Setup(p => p.Map<Conta>(contaDto)).Returns(contaEntity);
            _contaServiceMock.Setup(p => p.ValidarInclusaoConta(contaEntity)).Returns(true);
            _contaRepositoryMock.Setup(p => p.AddAsync(contaEntity)).ReturnsAsync(default(Conta));
        }
    }
}
