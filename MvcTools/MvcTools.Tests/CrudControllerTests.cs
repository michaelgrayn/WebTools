// MvcTools.Tests.CrudControllerTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDb;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ResultTypes;

    internal class CrudController : BasicCrudController
    {
        /// <inheritdoc />
        public CrudController(IMongoClient client) : base(client) { }

        /// <inheritdoc />
        public override async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            await Reset(database, collection);
            return await base.GetDocumentsAsync(database, collection);
        }

        public override async Task<IActionResult> GetDocumentsAsync(string database, string collection, IEnumerable<ObjectId> ids)
        {
            await Reset(database, collection);
            return await base.GetDocumentsAsync(database, collection);
        }

        public override async Task<IActionResult> PostDocumentAsync(string database, string collection, [FromBody] object document)
        {
            await Reset(database, collection);
            return await base.PostDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] object document)
        {
            await Reset(database, collection);
            return await base.PutDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> DeleteDocumentAsync(string database, string collection, [FromBody] object document)
        {
            await Reset(database, collection);
            return await base.DeleteDocumentAsync(database, collection, document);
        }

        private async Task Reset(string database, string collection)
        {
            await GetCollection(database, collection).DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
            await GetCollection(database, collection).InsertManyAsync(new[] { new BsonDocument("_id", new ObjectId()), new BsonDocument("_id", new ObjectId()), new BsonDocument("_id", new ObjectId()) });
        }
    }

    [TestClass]
    public class CrudControllerTests
    {
        [TestMethod]
        public async Task TestGetDocumentsAsync()
        {
            var controller = new CrudController(new MongoClient());
            var documents = await controller.GetDocumentsAsync("Test", "CrudController");
            Assert.IsInstanceOfType(documents, typeof(JsonStringResult));
        }

        public async Task TestGetDocumentsByIdAsync()
        {
            var controller = new CrudController(new MongoClient());
            var documents = await controller.GetDocumentsAsync("Test", "CrudController", new List<ObjectId>());
            Assert.IsInstanceOfType(documents, typeof(JsonStringResult));
            var document = new BsonDocument("_id", ObjectId.GenerateNewId());
            await controller.PostDocumentAsync("Test", "CrudController", document);
            documents = await controller.GetDocumentsAsync("Test", "CrudController", new List<ObjectId> { document["_id"].AsObjectId });
            Assert.IsInstanceOfType(documents, typeof(JsonStringResult));
        }
    }
}
