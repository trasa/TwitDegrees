using Blackfin.Core.NHibernate;
using Blackfin.Core.NHibernate.Fluent;
using FluentNHibernate.Cfg;
//using TwitDegrees.Presentation.Core.Mappings;
using TwitDegrees.Presentation.Core.Models;

namespace TwitDegrees.Presentation.Core.Services
{
    public class TwitDegreesNHibernateSettingsProvider : INHibernateSettingsProvider
    {
        

        public SupportedDatabases DatabaseType
        {
            get { return SupportedDatabases.MsSql; }
        }

        public string ConnectionStringKeyName
        {
            get { return "twitdegrees"; }
        }

        public bool ShowSql
        {
            get { return false; }
        }

        public int BatchSize
        {
            get { return 32; }
        }

        public bool GenerateStatistics
        {
            get { return true; }
        }

        public void FluentlyConfigureMap(MappingConfiguration m)
        {
            //m.FluentMappings.AddFromAssemblyOf<TwitterUserMap>();
            m.HbmMappings.AddFromAssemblyOf<TwitterUser>();
        }

        public FluentConfiguration FluentlyConfigure(FluentConfiguration config)
        {
            return config;
//            return config.ExposeConfiguration(
//                c => c.SetProperty(
//                         "proxyfactory.factory_class",
//                         "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle"));
        }
    }
}
