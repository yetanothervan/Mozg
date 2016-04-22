using Interfaces;
using Prism.Modularity;
using Prism.Regions;

namespace SomeToolbar.ModuleDefinition
{
    public class SomeToolbarModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public SomeToolbarModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.SomeToolbarRegion, typeof(RegionView));
        }
    }
}
