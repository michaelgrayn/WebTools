// MvcTools.Tests.MongoDbRepositoryTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MongoDb
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;
    using MvcTools.Domain.MongoDb;
    using MvcTools.Infrastructure;

    [TestClass]
    public class MongoDbRepositoryTests
    {
        [TestMethod]
        public void TestAddMongoDbIoc()
        {
            var services = new ServiceCollection();
            services.AddMongoClientIoC("mongodb://localhost").Clear();
        }

        [TestMethod]
        public async Task TestInsertAsync()
        {
            var collection = await GetCollection(0);
            var document = new Document();
            await collection.InsertDocumentAsync(document);
            Assert.AreNotEqual(document.Id, default);
            var count = await collection.GetDocumentsAsync();
            Assert.AreEqual(count.Count, 1);
        }

        [TestMethod]
        public async Task TestDeleteAsync()
        {
            var collection = await GetCollection(1);
            var document = new Document();
            await collection.InsertDocumentAsync(document);
            await collection.DeleteDocumentAsync(document.Id);
            var count = await collection.GetDocumentsAsync();
            Assert.AreEqual(count.Count, 0);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            const int newValue = 42;
            var collection = await GetCollectionOther(2);
            var document = new Document();
            await collection.InsertDocumentAsync(document);
            document.Value = newValue;
            await collection.UpdateDocumentAsync(document);
            var documents = await collection.GetDocumentsAsync(new PagingFilter<Document>(doc => doc.Id == document.Id));
            var updated = documents.Single();
            Assert.AreEqual(updated.Value, newValue);
        }

        private static async Task<IMongoDbRepository<Document>> GetCollection(uint testNumber)
        {
            var collection = await ResetCollection(testNumber);
            return new MongoDbRepository<Document>(collection);
        }

        private static async Task<IMongoCollection<Document>> ResetCollection(uint testNumber)
        {
            var collection = new MongoClient().GetDatabase("Test").GetCollection<Document>($"Test{testNumber}");
            await collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            return collection;
        }

        private static async Task<IMongoDbRepository<Document>> GetCollectionOther(uint testNumber)
        {
            await ResetCollection(testNumber);
            return new MongoDbRepository<Document>(new MongoClient(), "Test", $"Test{testNumber}");
        }
    }
}
