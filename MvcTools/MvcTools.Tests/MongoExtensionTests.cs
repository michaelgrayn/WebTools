// MvcTools.Tests.MongoExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Linq;
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
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
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

        [TestMethod]
        public async Task TestMultiFilterAndCountAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test3");
            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            await collection.DeleteManyAsync(MongoDbExtensions.CreateMultiIdFilter(documents));
            Assert.AreEqual(await collection.CountAsync(), 0);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test4");
            var documents = new List<Document> { new Document(), new Document() };
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            await collection.InsertManyAsync(documents);
            var findById = await collection.FindByIdAsync(documents.Select(x => x.Id));
            Assert.AreEqual(documents[0].Id, findById[0].Id);
            Assert.AreEqual(documents[1].Id, findById[1].Id);
        }
    }
}
