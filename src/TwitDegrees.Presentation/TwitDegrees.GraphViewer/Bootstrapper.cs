using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using TwitDegrees.GraphViewer.Infrastructure.ServiceClient;
using TwitDegrees.GraphViewer.Infrastructure.Services;
using TwitDegrees.GraphViewer.Modules.Graph;

namespace TwitDegrees.GraphViewer
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();
            Application.Current.RootVisual = shell;
            
            // TODO: this should go into unity / ioc
            // as a registered instance
            var dispatchHost = new DispatchHost(shell.Dispatcher);
            DispatchHostLocator.DispatchHost = dispatchHost;

            return shell;
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterInstance<Binding>(new BasicHttpBinding());
            // TODO endpoint is not correct for general use...
            Container.RegisterInstance(new EndpointAddress("http://localhost:1066/Api/Twitter.svc"));
            Container.RegisterType<ITwitterClient, TwitterClient>();
            
            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            var cat = new ModuleCatalog();
            cat.AddModule(typeof(GraphModule));
            return cat;
        }
    }
}
