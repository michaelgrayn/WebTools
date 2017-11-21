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

    internal class InternalController : CrudControllerBase<Document>
    {
        private readonly IMongoCollection<Document> _collection;

        public InternalController(IMongoClient client, string database, string collection) : base(client, database, collection)
        {
            _collection = client.GetDatabase(database).GetCollection<Document>(collection);
        }

        public override async Task<IActionResult> GetDocumentsAsync(CrudControllerFilter<Document> document = null)
        {
            await Reset();
            return await base.GetDocumentsAsync(document);
        }

        public override async Task<IActionResult> PostDocumentAsync([FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset();
            return await base.PostDocumentAsync(document);
        }

        public override async Task<IActionResult> PutDocumentAsync([FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset();
            _collection.InsertOne(document);
            return await base.PutDocumentAsync(document);
        }

        public override async Task<IActionResult> DeleteDocumentAsync(ObjectId document)
        {
            if (document == ObjectId.Empty) return BadRequest();
            await Reset();
            return await base.DeleteDocumentAsync(document);
        }

        private async Task Reset()
        {
            await _collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            await _collection.InsertManyAsync(new[] { new Document(), new Document(), new Document() });
        }
    }
}
