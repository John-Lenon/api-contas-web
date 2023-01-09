using Application.Interfaces.Services.Cobranca;
using Application.Utility;
using AutoMapper;
using Domain.Configurations;
using Domain.Interfaces.Application;
using Domain.Interfaces.Repositorys.Cobranca;
using Domain.Interfaces.Services.Cobranca;
using Moq;
using Xunit;

namespace Unit.Tests.Application.Cobranca
{
    public class ContaAppServiceFixture
    {
        public Mock<InjectorService> InjectorServiceMock { get; } = new Mock<InjectorService>();
        public Mock<IContaService> ContaServiceMock { get; } = new Mock<IContaService>();
        public Mock<IRegraDiaAtrasoAppService> RegraDiaAtrasoAppServiceMock { get; } = new Mock<IRegraDiaAtrasoAppService>();
        public Mock<IMapper> AutoMapperMock { get; } = new Mock<IMapper>();
        public Mock<IContaRepository> ContaRepositoryMock { get; } = new Mock<IContaRepository>();

        public ContaAppServiceFixture()
        {           
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition(nameof(ContaAppServiceCollection))]
    public class ContaAppServiceCollection : ICollectionFixture<ContaAppServiceFixture>
    {
    }
}
