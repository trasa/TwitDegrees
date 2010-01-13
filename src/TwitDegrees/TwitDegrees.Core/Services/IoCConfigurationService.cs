using System.Configuration;
using StructureMap;
using StructureMap.Attributes;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Config;
using TwitDegrees.Core.Messaging;
using TwitDegrees.Core.Repositories;
using SettingsProvider=TwitDegrees.Core.Config.SettingsProvider;

namespace TwitDegrees.Core.Services
{
    public static class IoCConfigurationService
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(
                x =>
                {
                    x.ForRequestedType<ISettingsProvider>()
                            .CacheBy(InstanceScope.Singleton)
                            .TheDefaultIsConcreteType<SettingsProvider>();

                    x.ForRequestedType<IStatusService>().TheDefaultIsConcreteType<StatusService>();

                    // what rate limit?
//                    x.ForRequestedType<ITwitterClient>().TheDefaultIsConcreteType<GovernedTwitterClient>();
                    x.ForRequestedType<ITwitterClient>().TheDefaultIsConcreteType<TwitterClient>();

                    x.ForRequestedType<ITwitterComm>().TheDefaultIsConcreteType<TwitterComm>();

                    x.ForRequestedType<IRateLimitStatusProvider>().TheDefaultIsConcreteType<RateLimitStatusProvider>();
//                    x.ForRequestedType<ITwitterGovernor>().TheDefaultIsConcreteType<TwitterGovernor>().AsSingletons();
                    x.ForRequestedType<ITwitterGovernor>().TheDefaultIsConcreteType<PermissiveTwitterGovernor>().AsSingletons();

                    x.ForRequestedType<ITwitterRequestQueue>()
                        .CacheBy(InstanceScope.Singleton)
                        .TheDefault.Is.ConstructedBy(()=> new TwitterRequestQueue(
                             ((TwitterSection)ConfigurationManager.GetSection("twitter") ?? new TwitterSection()).RequestQueueName
                            ));

                    x.ForRequestedType<ITwitterResponseQueue>()
                        .CacheBy(InstanceScope.Singleton)
                        .TheDefault.Is.ConstructedBy(()=> new TwitterResponseQueue(
                            ((TwitterSection)ConfigurationManager.GetSection("twitter") ?? new TwitterSection()).ResponseQueueName
                            ));


                    x.ForRequestedType<IUserWriter>().TheDefaultIsConcreteType<SqlUserWriter>();

                    x.ForRequestedType<IRecentFriendRegistry>()
                        .CacheBy(InstanceScope.Singleton)
                        .TheDefaultIsConcreteType<RecentFriendRegistry>();


                });
        }


        public static void ClearConfiguration()
        {
            ObjectFactory.Initialize(x => { });
        }

        public static void RemoveMocks()
        {
            ObjectFactory.ResetDefaults();
        }
    }
}
