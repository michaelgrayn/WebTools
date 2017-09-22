// MvcTools.Tests.MongoExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDb;
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

        public int Value { get; set; }
    }

    [TestClass]
    public class MongoExtensionTests
    {
        [TestMethod]
        public async Task TestSaveAndDeleteAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test0");
            var document = new Document();
            var save = await collection.SaveAsync(document);
            Assert.AreNotEqual(save.UpsertedId, default);
            document.Value = 1;
            save = await collection.SaveAsync(document);
            Assert.AreEqual(save.ModifiedCount, 1);
            var delete = await collection.DeleteOneAsync(document);
            Assert.AreEqual(delete.DeletedCount, 1);
        }

        [TestMethod]
        public async Task TestSaveManyAndFindAllAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test1");
            await collection.DeleteManyAsync(Builders<Document>.Filter.Empty);
            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            var result = await collection.FindAllAsync();
            Assert.AreEqual(result.Count, documents.Count);
        }

        [TestMethod]
        public async Task TestSaveManyAndDeleteManyAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test2");
            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            var result = await collection.DeleteManyAsync(documents);
            Assert.AreEqual(result.DeletedCount, documents.Count);
        }
    }
}
