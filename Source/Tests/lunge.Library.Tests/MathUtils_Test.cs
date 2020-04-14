using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace lunge.Library.Tests
{
    public class MathUtils_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InBetween_Test()
        {
            Assert.AreEqual(MathUtils.NormalizeInRange(100f, 50f, 0f, 100f), 50.0f);
            Assert.AreEqual(MathUtils.NormalizeInRange(200f, 50f, 0f, 100f), 100.0f);
        }

        [Test]
        public void FromPolar_Test()
        {
            // Length tests
            Assert.AreEqual(MathUtils.FromPolar(0, 10).Length(), 10);
            Assert.AreEqual(MathUtils.FromPolar((float)Math.PI/2.0f, 10).Length(), 10);

            // Direction tests
            Assert.AreEqual(MathUtils.FromPolar(0, 10), new Vector2(10, 0));
            Assert.AreEqual(MathUtils.FromPolar((float)Math.PI, 10).X, new Vector2(-10, 0).X);
        }
    }
}