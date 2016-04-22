﻿using Interfaces;
using Prism.Modularity;
using Prism.Regions;

namespace OutputScreen.ModuleDefinition
{
    public class OutputScreenModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public OutputScreenModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.OutputScreenRegion, typeof(RegionView));
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.DiagnosticsRegion, typeof(DiagnosticsView.DiagnosticsView));
        }
    }
}
