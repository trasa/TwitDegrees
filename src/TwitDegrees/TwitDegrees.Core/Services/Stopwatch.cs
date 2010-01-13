using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;

namespace TwitDegrees.Core.Services
{
    /// <summary>
    /// high precision timer using QueryPerformanceCounter API.
    /// Only enabled if STOPWATCH conditional is set.
    /// </summary>
    public class Stopwatch
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);


        private long startTime;
        private long freq;

        [Conditional("STOPWATCH")]
        public void Start()
        {
            QueryPerformanceCounter(out startTime);
        }

        public long Stop()
        {
#if STOPWATCH
            long stopTime;
            if (startTime > 0)
                stopTime = CurrentTime - startTime;
            else
                stopTime = CurrentTime;
            startTime = 0;
            return stopTime;
#else
            return 0;
#endif
        }

        public long CurrentTime 
        {
            get
            {
                long l;
                QueryPerformanceCounter(out l);
                return l;
            }
        }

        public long Resolution
        {
            get
            {
                if (freq == 0)
                    QueryPerformanceFrequency(out freq);
                return freq;
            }
        }

        public double ConvertToMs(long stopwatchResult)
        {
            return (Convert.ToDouble(stopwatchResult) / Resolution) * 1000.0;
        }

        public double ConvertToMs(double stopwatchResult)
        {
            return (Convert.ToDouble(stopwatchResult) / Resolution) * 1000.0;
        }

        public double ConvertToSec(long stopwatchResult)
        {
            return Convert.ToDouble(stopwatchResult)/Resolution;
        }

        public double ConvertToSec(double stopwatchResult)
        {
            return Convert.ToDouble(stopwatchResult)/ Resolution;
        }

        public void LogResult(Action<double> logAction)
        {
#if STOPWATCH
            long result = Stop();
            logAction(ConvertToMs(result));
            
#endif
        }
    }
}