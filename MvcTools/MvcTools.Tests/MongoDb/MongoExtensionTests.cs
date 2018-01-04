// MvcTools.Tests.MongoExtensionTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;
    using MvcTools.Infrastructure;

    [TestClass]
    public class MongoExtensionTests
    {
        [TestMethod]
        public void TestAddMongoDbIoc()
        {
            var services = new ServiceCollection();
            services.AddMongoClientIoC("mongodb://localhost").Clear();
        }

        [TestMethod]
        public async Task TestSaveAndDeleteAsync()
        {
            var collection = await GetCollection(0);
            var document = new Document();
            var save = await collection.SaveAsync(document);
            Assert.AreNotEqual(save.UpsertedId, default);
            document.Value = 1;
            save = await collection.SaveAsync(document);
            Assert.AreEqual(save.ModifiedCount, 1);
            var delete = await collection.DeleteOneAsync(document.Filter<Document>());
            Assert.AreEqual(delete.DeletedCount, 1);
        }

        [TestMethod]
        public async Task TestSaveManyAndFindAllAsync()
        {
            var collection = await GetCollection(1);
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
            var collection = await GetCollection(2);
            var documents = new List<Document> { new Document(), new Document() };
            await collection.SaveManyAsync(documents);
            Assert.AreEqual(documents.Count, (await collection.FindAllAsync()).Count);
        }

        [TestMethod]
        public async Task TestFindByIdAsync()
        {
            var collection = await GetCollection(4);
            var documents = new List<Document> { new Document(), new Document() };
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            await collection.InsertManyAsync(documents);
            var findById = await collection.FindByIdAsync(documents.Select(x => x.Id));
            Assert.AreEqual(documents[0].Id, findById[0].Id);
            Assert.AreEqual(documents[1].Id, findById[1].Id);
        }

        private static async Task<IMongoCollection<Document>> GetCollection(uint testNumber)
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>($"Test{testNumber}");
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            return collection;
        }
    }
}
