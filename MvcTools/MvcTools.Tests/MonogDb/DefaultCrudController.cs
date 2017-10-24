// MvcTools.Tests.DefaultCrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MonogDb
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDb;
    using MongoDB.Driver;

    internal class DefaultCrudController : BasicCrudController<object>
    {
        public DefaultCrudController(IMongoClient client) : base(client, new DefaultCrudControllerFilter()) { }

        public override async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] object document)
        {
            GetCollection(database, collection).DeleteMany(Builders<object>.Filter.Eq(MongoDbExtensions.Id, ((Document) document).Id));
            await PostDocumentAsync(database, collection, document);
            return await base.PutDocumentAsync(database, collection, document);
        }
    }
}
