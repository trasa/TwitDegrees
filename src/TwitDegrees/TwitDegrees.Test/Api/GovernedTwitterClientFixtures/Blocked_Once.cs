using System.Reflection;
using log4net;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using TwitDegrees.Core.Api;

namespace TwitDegrees.Test.Api.GovernedTwitterClientFixtures
{
    [TestFixture]
    public class Blocked_Once : When_Requesting_User
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        protected override ITwitterGovernor Governor
        {
            get { return new SwitchedTwitterGovernor(1); }
        }
        protected string SecondResult { get; set; }


        protected override void When()
        {
            base.When();
            SecondResult = ClientUnderTest.GetUser("trasa");
        }


        [Test]
        public void Got_First_Result()
        {
            Assert.That(Result, Is.EqualTo(Original));
        }

        [Test]
        public void Got_Second_Result()
        {
            Assert.That(SecondResult, Is.EqualTo(Original));
        }
    }


    [TestFixture]
    public class Blocked_Many : When_Requesting_User
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        protected override ITwitterGovernor Governor
        {
            get { return new SwitchedTwitterGovernor(3); }
        }

        protected string[] Results { get; set; }


        protected override void When()
        {
            
            Results = new string[3];
            Results[0] = ClientUnderTest.GetUser("trasa");
            Results[1] = ClientUnderTest.GetUser("trasa");
            Results[2] = ClientUnderTest.GetUser("trasa");
        }


        [Test]
        public void GotResults()
        {
            foreach(string r in Results)
            {
                Assert.That(r , Is.EqualTo(Original));
            }
        }
    }
}
