using lunge.Library.Bindables;
using NUnit.Framework;

namespace lunge.Library.Tests.Bindables
{
    [TestFixture]
    public class BindableFloat_Test
    {
        [Test]
        public void TestValueChanging()
        {
            var bf1 = new BindableFloat(15, 0, 100, BindableActionHandler);
            
            Assert.Equals(bf1.Value, 15);
            Assert.Equals(bf1.MinValue, 0);
            Assert.Equals(bf1.MaxValue, 0);
        }

        private void BindableActionHandler(object? sender, BindableValueChangeEvent<float> e)
        {
            
        }
    }
}