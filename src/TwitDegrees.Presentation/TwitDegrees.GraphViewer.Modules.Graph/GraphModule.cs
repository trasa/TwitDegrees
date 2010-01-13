using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using TwitDegrees.GraphViewer.Infrastructure;


namespace TwitDegrees.GraphViewer.Modules.Graph
{
    public class GraphModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public GraphModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            // register views / services
//            container.RegisterType<IGraphView, GraphView>();
            container.RegisterType<GraphViewModel, GraphViewModel>();
            regionManager.RegisterViewWithRegion(RegionNames.GraphRegion, typeof(GraphView)); // view first, not view-model first.
        }
    }
}
