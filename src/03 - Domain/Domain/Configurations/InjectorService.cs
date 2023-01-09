using System;

namespace Domain.Configurations
{
    public class InjectorService
    {
        private IServiceProvider _serviceProvider;
        public InjectorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public InjectorService()
        {
        }

        public virtual TService GetService<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }
    }
}
