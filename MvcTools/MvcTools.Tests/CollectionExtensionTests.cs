// MvcTools.MvcTools.Tests.CollectionExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionExtensionTests
    {
        [TestMethod]
        public void TestIsEmptyAndIsNotEmpty()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsEmpty());
            Assert.IsFalse(list.IsNotEmpty());
            list = new List<int>();
            Assert.IsTrue(list.IsEmpty());
            Assert.IsFalse(list.IsNotEmpty());
            list.Add(0);
            Assert.IsFalse(list.IsEmpty());
            Assert.IsTrue(list.IsNotEmpty());
        }

        [TestMethod]
        public void TestNone()
        {
            var list = new List<int>();
            Assert.IsTrue(list.None());
            list.Add(0);
            list.Add(1);
            list.Add(2);
            Assert.IsFalse(list.None());
            Assert.IsTrue(list.None(x => x < 0));
            Assert.IsFalse(list.None(x => x < 1));
        }

        [TestMethod]
        public void TestRemove()
        {
            var list = new List<int> { 0, 1, 2 };
            list.Remove();
            Assert.AreEqual(1, list.Last());
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TestSwap()
        {
            var list = new List<int> { 3, 4, 5 };
            const int a = 0;
            const int b = 2;
            list.Swap(a, b);
            Assert.AreEqual(5, list[a]);
            Assert.AreEqual(3, list[b]);
        }

        [TestMethod]
        public void TestSwapRemove()
        {
            var list = new List<int> { 6, 7, 8, 9 };
            list.SwapRemove(1);
            Assert.AreEqual(8, list.Last());
            Assert.AreEqual(3, list.Count);
        }
    }
}
