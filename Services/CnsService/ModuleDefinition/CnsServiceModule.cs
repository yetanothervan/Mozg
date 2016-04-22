using Interfaces;
using Microsoft.Practices.Unity;
using Prism.Modularity;

namespace CnsService.ModuleDefinition
{
    public class CnsServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public CnsServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ICnsService, CnsService>();
        }
    }
}
