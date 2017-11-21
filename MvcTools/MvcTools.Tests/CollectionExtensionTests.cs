// MvcTools.Tests.CollectionExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using CollectionExtensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionExtensionTests
    {
        [TestMethod]
        public void TestIsEmptyAndIsNotEmpty()
        {
            List<int> enumerable = null;
            Assert.IsTrue(enumerable.IsEmpty());
            Assert.IsFalse(enumerable.IsNotEmpty());
            enumerable = new List<int>();
            Assert.IsTrue(enumerable.IsEmpty());
            Assert.IsFalse(enumerable.IsNotEmpty());
            enumerable.Add(0);
            Assert.IsFalse(enumerable.IsEmpty());
            Assert.IsTrue(enumerable.IsNotEmpty());
        }

        [TestMethod]
        public void TestNone()
        {
            var enumerable = new List<int>();
            Assert.IsTrue(enumerable.None());
            enumerable.Add(0);
            enumerable.Add(1);
            enumerable.Add(2);
            Assert.IsFalse(enumerable.None());
            Assert.IsTrue(enumerable.None(x => x < 0));
            Assert.IsFalse(enumerable.None(x => x < 1));
        }

        [TestMethod]
        public void TestToIList()
        {
            var enumerable = new[] { 0, 1, 2 };
            var list = enumerable.ToIList();
            Assert.IsInstanceOfType(list, typeof(IList<int>));
        }

        [TestMethod]
        public void TestAddMany()
        {
            var collection = new List<int> { 0, 1, 2 };
            collection.AddMany(new[] { 3, 4 });
            Assert.AreEqual(5, collection.Count);
        }

        [TestMethod]
        public void TestRemoveMany()
        {
            var collection = new List<int> { 0, 1, 2 };
            collection.RemoveMany(new[] { 1, 2 });
            Assert.AreEqual(1, collection.Count);
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
