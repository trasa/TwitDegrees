using System;
using System.Reflection;
using System.Threading;
using log4net;
using StructureMap;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Crawler
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            IoCConfigurationService.Initialize();
            ObjectFactory.AssertConfigurationIsValid();

            Log.Info("Begin Crawling!");
            ObjectFactory.GetInstance<TwitterCrawler>();
            Log.Info("We are crawling...waiting for requests.  hit [enter] to stop waiting.");
            Console.ReadLine();
        }
    }
}
