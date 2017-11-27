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

        public async Task<IActionResult> GetDocuments(CrudControllerFilter<Document> document = null)
        {
            await Reset();
            return await GetDocumentsAsync(document);
        }

        public async Task<IActionResult> PostDocument([FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset();
            return await PostDocumentAsync(document);
        }

        public async Task<IActionResult> PutDocument([FromBody] Document document)
        {
            if (document.Id == ObjectId.Empty) return BadRequest();
            await Reset();
            _collection.InsertOne(document);
            return await PutDocumentAsync(document);
        }

        public async Task<IActionResult> DeleteDocument(ObjectId document)
        {
            if (document == ObjectId.Empty) return BadRequest();
            await Reset();
            return await DeleteDocumentAsync(document);
        }

        private async Task Reset()
        {
            await _collection.DeleteManyAsync(FilterDefinition<Document>.Empty);
            await _collection.InsertManyAsync(new[] { new Document(), new Document(), new Document() });
        }
    }
}
