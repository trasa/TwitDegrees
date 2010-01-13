using System;
using System.Web.Mvc;
using System.Web.Routing;
using Blackfin.Core.NHibernate;
using Blackfin.Core.NHibernate.Fluent;
using NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace TwitDegrees.Presentation.Core.Services
{
    public class TwitDegreesRegistry : Registry
    {
        public TwitDegreesRegistry()
        {
            Scan(scanner =>
                     {
                         scanner.TheCallingAssembly();
                         scanner.AssemblyContainingType(typeof(NHibernateConfigurationService));
                         scanner.WithDefaultConventions();
                     });

//            ForRequestedType<INHibernateSettingsProvider>()
//                .TheDefault.IsThis(NHibernateSettingsProvider.Default);

            ForRequestedType<NHibernateSession>()
                .TheDefaultIsConcreteType<NHibernateSession>()
                .AsSingletons();

            ForRequestedType<INHibernateSettingsProvider>()
                .TheDefaultIsConcreteType<TwitDegreesNHibernateSettingsProvider>()
                .AsSingletons();

            ForRequestedType<ISessionFactory>()
                .TheDefault.Is.ConstructedBy(c => ObjectFactory.GetInstance<INHibernateConfigurationService>().CreateSessionFactory());


//            ForRequestedType<IMembershipService>()
//                .CacheBy(InstanceScope.HttpContext)
//                .TheDefaultIsConcreteType<AccountMembershipService>();
//
//            ForRequestedType<MembershipProvider>()
//                .TheDefault.IsThis(Membership.Provider);

            //            ForRequestedType<NerdDinnerDataContext>()
            //            .CacheBy(InstanceScope.HttpContext)
            //            .TheDefault.Is.ConstructedBy(() => new NerdDinnerDataContext());

        }
    }

    public static class IoCContainer
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(init => init.AddRegistry<TwitDegreesRegistry>());
            
        }
    }


    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type controllerType = base.GetControllerType(controllerName);
            if (controllerType == null)
                return null;
            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}
