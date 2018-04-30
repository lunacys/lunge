using NUnit.Framework;

namespace LunarisGE.Library.Tests
{
    [TestFixture]
    public class TestTest
    {
        [Test]
        public void Sqr()
        {
            Test test = new Test();
            Assert.AreEqual(25, test.Sqr(5));
        }
    }
}
