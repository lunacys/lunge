using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace lunge.Library.Tests
{
    [TestFixture]
    public class Vector2Extensions_Test
    {
        [Test]
        public void Normalized_Test()
        {
            Vector2 testVec2 = new Vector2(50, 0);
            Vector2 testVec3 = new Vector2(0, 50);
            
            Assert.AreEqual(testVec2.Normalized(), new Vector2(1, 0));
            Assert.AreEqual(testVec3.Normalized(), new Vector2(0, 1));
        }

        [Test]
        public void ConvertToAngle_Test()
        {
            Vector2 testVec1 = new Vector2(1, 0);
            Vector2 testVec2 = new Vector2(0, -1);
            Vector2 testVec3 = new Vector2(-1, 0);

            Assert.AreEqual(testVec1.ConvertToAngle(), 0);
            Assert.AreEqual(testVec2.ConvertToAngle(), -(float)Math.PI / 2);
            Assert.AreEqual(testVec3.ConvertToAngle(), (float)Math.PI);
        }

        [Test]
        public void Abs_Test()
        {
            Vector2 vec1 = new Vector2(-1, -2);
            Vector2 vec2 = new Vector2(15, -15);
            Vector2 vec3 = new Vector2(-10, 99999);
            
            vec2.Abs();
            vec3.Abs();

            Assert.That(vec1.Abs() == new Vector2(1, 2));
            Assert.That(vec2 == new Vector2(15, -15));
            Assert.That(vec3.Abs() == new Vector2(10, 99999));
        }
    }
}