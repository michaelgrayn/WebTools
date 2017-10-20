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

    internal class CrudAuth : ICrudAuthenticator<Document>
    {
        public (FilterDefinition<Document> filter, int pageNumber, int pageSize) Get()
        {
            return FilterDefinition<Document>.Empty.All();
        }

        public bool CanPost(Document document)
        {
            return document.Id != ObjectId.Empty;
        }

        public bool CanPut(Document document)
        {
            return document.Id != ObjectId.Empty;
        }

        public bool CanDelete(ObjectId document)
        {
            return document != ObjectId.Empty;
        }
    }

    internal class CrudController : BasicCrudController<Document>
    {
        public CrudController(IMongoClient client) : base(client, new CrudAuth()) { }

        public override async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            await Reset(database, collection);
            return await base.GetDocumentsAsync(database, collection);
        }

        public override async Task<IActionResult> PostDocumentAsync(string database, string collection, [FromBody] Document document)
        {
            await Reset(database, collection);
            return await base.PostDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] Document document)
        {
            GetCollection(database, collection).DeleteMany(Builders<Document>.Filter.Eq(MongoDbExtensions.Id, document.Id));
            var id = document.Id;
            GetCollection(database, collection).InsertOne(document);
            document.Id = id;
            return await base.PutDocumentAsync(database, collection, document);
        }

        public override async Task<IActionResult> DeleteDocumentAsync(string database, string collection, ObjectId document)
        {
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
