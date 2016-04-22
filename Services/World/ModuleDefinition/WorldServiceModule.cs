using Interfaces;
using Microsoft.Practices.Unity;
using Prism.Modularity;

namespace WorldService.ModuleDefinition
{
    public class WorldServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public WorldServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IWorldService, WorldService>(new ContainerControlledLifetimeManager());
        }
    }
}
