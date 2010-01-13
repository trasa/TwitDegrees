using System.Threading;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Test.Services
{
    [TestFixture]
    public class StopwatchFixture
    {
        [Test]
        public void Run()
        {
#if STOPWATCH
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread.Sleep(1000);
            long result = stopwatch.Stop();
            double ms = stopwatch.ConvertToMs(result);
            Assert.That(ms, Is.GreaterThanOrEqualTo(900));
#endif
        }
    }
}
