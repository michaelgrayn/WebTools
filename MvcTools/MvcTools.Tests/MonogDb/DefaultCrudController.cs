// MvcTools.Tests.DefaultCrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Tests.MonogDb
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDb;
    using MongoDB.Driver;

    internal class DefaultCrudControllerBase : CrudControllerBase<object>
    {
        private readonly IMongoCollection<object> _collection;

        public DefaultCrudControllerBase(IMongoClient client, string database, string collection) : base(client, database, collection)
        {
            _collection = client.GetDatabase(database).GetCollection<object>(collection);
        }

        public override async Task<IActionResult> PutDocumentAsync([FromBody] object document)
        {
            _collection.DeleteMany(Builders<object>.Filter.Eq(MongoDbExtensions.Id, ((Document) document).Id));
            await PostDocumentAsync(document);
            return await base.PutDocumentAsync(document);
        }
    }
}
