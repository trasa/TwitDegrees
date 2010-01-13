using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Blackfin.Core.NHibernate;
using log4net;
using StructureMap;
using TwitDegrees.Presentation.Core.Services;

namespace TwitDegrees.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("PathSearch", "{controller}/{action}/{start}/{dest}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            log.Debug("Application start.");
            RegisterRoutes(RouteTable.Routes);
            IoCContainer.Configure();

            ObjectFactory.Configure(x => x.ForRequestedType<ISessionStorage>().TheDefault.IsThis(new WebSessionStorage(this)));
            ObjectFactory.AssertConfigurationIsValid();

            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }
    }
}