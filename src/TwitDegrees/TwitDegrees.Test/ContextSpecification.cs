using NUnit.Framework;

namespace TwitDegrees.Test
{
    public class ContextSpecification
    {
        [SetUp]
        public void SetUp()
        {
            Given();
            When();
            // Then(Asserts.Happen());
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        protected virtual void Given() { }
        protected virtual void When() { }
        protected virtual void CleanUp() { }

    }
}
