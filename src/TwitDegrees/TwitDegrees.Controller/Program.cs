using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using StructureMap;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Controller
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ManualResetEvent closingEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(ctrlType =>
                                      {
                                          closingEvent.Set();
                                          return true;
                                      }
                                  , true);

            log.Info("Begin Controller.");
            IoCConfigurationService.Initialize();
            ObjectFactory.AssertConfigurationIsValid();

            var a = Args.Parse(args);
            
            var controller = ObjectFactory.GetInstance<CrawlerController>();
            
            controller.EnableFriendCrawling = a.EnableFriendCrawling;
            
            // TODO this doesn't do anything yet... need to revise how the controller
            // decides what leads to follow up on (or not)
            controller.EnableFollowerCrawling = a.EnableFollowerCrawling;

            controller.ShutdownResetEvent = closingEvent;
            if (!string.IsNullOrEmpty(a.StartCrawlingAtName))
            {
                log.Info("Crawling starting at " + a.StartCrawlingAtName);
                controller.Crawl(a.StartCrawlingAtName);
            } 
            else
            {
                log.Info("Processing Queue Messages (not starting a new crawl root)");
            }
            
            log.Info("-- Hit ctrl-c to terminate the controller --");
            closingEvent.WaitOne();

            // once they hit ctrl-c:
            controller.Shutdown = true;
            log.Warn("Shutting down...(waiting 15 seconds for everything to settle down)");
            Thread.Sleep(15 * 1000);
        }

        
        class Args
        {
            public bool EnableFriendCrawling { get; private set; }
            public bool EnableFollowerCrawling { get; private set; }
            public string StartCrawlingAtName { get; private set;}

            public static Args Parse(string[] args)
            {
                var result = new Args();
                if (args.Length > 0)
                {
                    result.EnableFriendCrawling = bool.Parse(args[0]);
                }
                if (args.Length > 1)
                {
                    result.EnableFollowerCrawling = bool.Parse(args[1]);
                }
                if (args.Length > 2)
                {
                    result.StartCrawlingAtName = args[1];
                }
                return result;
            }
        }




        // from http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/707e9ae1-a53f-4918-8ac4-62a1eddb3c4a

        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);



        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes ctrlType);



        // An enumerated type for the control messages
        // sent to the handler routine.
        // ReSharper disable InconsistentNaming
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        // ReSharper restore InconsistentNaming
    }
}
