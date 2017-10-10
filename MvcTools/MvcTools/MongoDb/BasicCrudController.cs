namespace MvcTools.MongoDb
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentController;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    [Route("api/{database}/{collection}")]
    public class BasicCrudController : FluentControllerBase
    {
        private readonly IMongoClient _client;

        public BasicCrudController(IMongoClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string database, string collection)
        {
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            var data = await GetCollection(database, collection).FindAllAsync();
            var json = data.ToJson(jsonWriterSettings);
            var actionResult = Content(json);
            return actionResult;
        }

        private IMongoCollection<BsonDocument> GetCollection(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<BsonDocument>(collection);
        }
    }
}
