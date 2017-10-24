// MvcTools.Tests.CrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MonogDb
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDb;
    using MongoDB.Bson;
    using MongoDB.Driver;

    internal class CrudControllerFilter : ICrudControllerFilter<Document>
    {
        public (FilterDefinition<Document> filter, int pageNumber, int pageSize) Filter()
        {
            return FilterDefinition<Document>.Empty.All();
        }
    }

    internal class CrudController : BasicCrudController<Document>
    {
        public CrudController(IMongoClient client) : base(client, new CrudControllerFilter()) { }

        public override async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            await Reset(database, collection);
            return await base.GetDocumentsAsync(database, collection);
        }

        public override async Task<IActionResult> PostDocumentAsync(string database, string collection, [FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset(database, collection);
            return await base.PostDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset(database, collection);
            GetCollection(database, collection).InsertOne(document);
            return await base.PutDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> DeleteDocumentAsync(string database, string collection, ObjectId document)
        {
            if (document == ObjectId.Empty) return BadRequest();
            await Reset(database, collection);
            return await base.DeleteDocumentAsync(database, collection, document);
        }

        private async Task Reset(string database, string collection)
        {
            await GetCollection(database, collection).DeleteManyAsync(FilterDefinition<Document>.Empty);
            await GetCollection(database, collection).InsertManyAsync(new[] { new Document(), new Document(), new Document() });
        }
    }
}
