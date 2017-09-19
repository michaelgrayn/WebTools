// MvcTools.MvcTools.Tests.MongoExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Threading.Tasks;
    using Extensions.MongoDb;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Bson;
    using MongoDB.Driver;

    // ReSharper disable All
    public class Document : MongoDbDocument
    {
        public override ObjectId Id
        {
            get => base.Id;
            set => base.Id = value;
        }
    }

    [TestClass]
    public class MongoExtensionTests
    {
        [TestMethod]
        public async Task TestMongoExtensionsAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test");
            var document = new Document { Id = ObjectId.Empty };
            await collection.Save(document);
            Assert.AreEqual(document, await collection.Find(x => x.Id == document.Id).SingleAsync());
        }
    }
}
