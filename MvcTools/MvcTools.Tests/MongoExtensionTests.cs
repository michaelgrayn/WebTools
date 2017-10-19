// MvcTools.Tests.MongoExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDb;
    using MongoDB.Bson;
    using MongoDB.Driver;

    // ReSharper disable All
    public class Document : MongoDbDocument<Document>
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
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);

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

            var random = new Random();
            var documents = new List<Document> { new Document { Value = random.Next() }, new Document { Value = random.Next() } };
            await collection.SaveManyAsync(documents);
            var result = await collection.FindAllAsync();
            Assert.AreEqual(result.Count, documents.Count);
            var first = await collection.FindByIdAsync(result.First().Id);
            Assert.AreEqual(result.First().Value, first.Value);
        }

        [TestMethod]
        public async Task TestSaveManyAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test2");
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);

            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            Assert.AreEqual(documents.Count, (await collection.FindAllAsync()).Count);
        }

        [TestMethod]
        public async Task TestMultiFilterAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test3");
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);

            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            await collection.DeleteManyAsync(MongoDbExtensions.CreateMultiIdFilter<Document>(documents.Select(x => x.Id)));
            Assert.AreEqual(await collection.CountAsync(FilterDefinition<Document>.Empty), 0);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>("Test4");
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);

            var documents = new List<Document> { new Document(), new Document() };
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            await collection.InsertManyAsync(documents);
            var findById = await collection.FindByIdAsync(documents.Select(x => x.Id));
            Assert.AreEqual(documents[0].Id, findById[0].Id);
            Assert.AreEqual(documents[1].Id, findById[1].Id);
        }
    }
}
