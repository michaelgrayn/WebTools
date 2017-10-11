// MvcTools.Tests.CrudControllerTests.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests
{
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
            await GetCollection(database, collection).DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
            await GetCollection(database, collection).InsertManyAsync(new[] { new BsonDocument("a", 1), new BsonDocument("b", 2) });
            return await base.GetDocumentsAsync(database, collection);
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
    }
}
