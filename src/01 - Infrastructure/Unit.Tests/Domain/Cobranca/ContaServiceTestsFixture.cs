using Application.Utility;
using Domain.Configurations;
using Domain.Interfaces.Application;
using Domain.Services.Cobranca;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unit.Tests.Domain.Cobranca
{
    public class ContaServiceTestsFixture : IDisposable
    {        
        public Mock<InjectorService> InjectorService { get; set; }


        public ContaServiceTestsFixture()
        {
            InjectorService = new Mock<InjectorService>();            
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition(nameof(ContaServiceCollection))]
    public class ContaServiceCollection : ICollectionFixture<ContaServiceTestsFixture>
    {
    }
}
