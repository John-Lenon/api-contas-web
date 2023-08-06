using Integration.Tests.Configurations.ContextosCompartilhados;
using Xunit;

namespace Integration.Tests.Configurations
{
    // Registrando o contexto IntegrationTestsFixure<T> como um contexto compartilhado entre os testes que 
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixure<StartupApiTests>>
    {
    }
}
