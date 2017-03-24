using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BitConertExTest
{
    [TestFixture]
    public class NunitDemo
    {
        [SetUp]
        protected void SetUp()
        {
        }

        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void DivideTest(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCase(12, 3, ExpectedResult = 4)]
        [TestCase(12, 2, ExpectedResult = 6)]
        [TestCase(12, 4, ExpectedResult = 3)]
        [Description("DivideTest")]
        public int DivideTest(int n, int d)
        {
            return (n / d);
        }

        [Test]
        public void ErroTest()
        {
            Assert.Fail("hello");
        }
    }
}
