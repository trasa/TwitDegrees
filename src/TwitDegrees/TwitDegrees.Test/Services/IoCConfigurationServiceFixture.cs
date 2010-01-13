using Blackfin.Core.NUnit;
using NUnit.Framework;
using StructureMap;
using TwitDegrees.Core.Api;
using TwitDegrees.Core.Services;

namespace TwitDegrees.Test.Services
{
    [TestFixture]
    public class IoCConfigurationServiceFixture
    {
        [Test]
        public void ValidateContainer()
        {
            ObjectFactory.AssertConfigurationIsValid();
        }

        [Test]
        public void ClearConfiguration()
        {
            IoCConfigurationService.ClearConfiguration();
            Expect.Exception<StructureMapException>(() => ObjectFactory.GetInstance<ITwitterComm>());
        }


        [SetUp]
        public void SetUp()
        {
            IoCConfigurationService.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            IoCConfigurationService.ClearConfiguration();
        }

    }
}
