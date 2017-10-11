// MvcTools.BasicCrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using System.Threading.Tasks;
    using FluentController;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Driver;

    /// <summary>
    /// A controller for basic crud operations.
    /// </summary>
    [Route("api/{database}/{collection}")]
    public class BasicCrudController : FluentControllerBase
    {
        /// <summary>
        /// The MongoDb client.
        /// </summary>
        private readonly IMongoClient _client;

        /// <summary>
        /// Settings to correctly convert a <see cref="BsonDocument" /> to JSON.
        /// </summary>
        private readonly JsonWriterSettings _jsonWriterSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicCrudController" /> class.
        /// </summary>
        /// <param name="client">MongoDb client.</param>
        public BasicCrudController(IMongoClient client)
        {
            _client = client;
            _jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
        }

        /// <summary>
        /// Gets all documents from <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <returns>An action result that writes the data to the result stream.</returns>
        [HttpGet]
        public virtual async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            var data = await GetCollection(database, collection).FindAllAsync();
            return JsonString(data.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Gets a MongoDb collection by name.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <returns>The MongoDb collection.</returns>
        protected IMongoCollection<BsonDocument> GetCollection(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<BsonDocument>(collection);
        }
    }
}
