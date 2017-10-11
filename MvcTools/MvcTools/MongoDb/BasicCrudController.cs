// MvcTools.BasicCrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using System.Collections.Generic;
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
        /// <returns>All documents in <paramref name="database" />.<paramref name="collection" />.</returns>
        [HttpGet]
        public virtual async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            var documents = await GetCollection(database, collection).FindAllAsync();
            return JsonString(documents.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Gets documents by _id from <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="ids">The _id's of the documents to get.</param>
        /// <returns>All documents in <paramref name="database" />.<paramref name="collection" /> with _id's in <paramref name="ids" />.</returns>
        [HttpGet]
        public virtual async Task<IActionResult> GetDocumentsAsync(string database, string collection, [FromBody] IEnumerable<ObjectId> ids)
        {
            var documents = await GetCollection(database, collection).FindByIdAsync(ids);
            return JsonString(documents.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Inserts a document into <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to insert.</param>
        /// <returns>The document after insert.</returns>
        [HttpPost]
        public virtual async Task<IActionResult> PostDocumentAsync(string database, string collection, [FromBody] object document)
        {
            await GetCollection(database, collection).InsertOneAsync(document.ToBsonDocument());
            return JsonString(document.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Updates a document in <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to update. Must have an id.</param>
        /// <returns>The replace result.</returns>
        [HttpPut]
        public virtual async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] object document)
        {
            var bsonDocument = document.ToBsonDocument();
            if (TryIdFilter(bsonDocument, out var filter)) return Json(await GetCollection(database, collection).ReplaceOneAsync(filter, bsonDocument));
            return BadRequest();
        }

        /// <summary>
        /// Deletes a document in <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to delete. Must have an id.</param>
        /// <returns>The delete result.</returns>
        [HttpDelete]
        public virtual async Task<IActionResult> DeleteDocumentAsync(string database, string collection, [FromBody] object document)
        {
            var bsonDocument = document.ToBsonDocument();
            if (TryIdFilter(bsonDocument, out var filter)) return Json(await GetCollection(database, collection).DeleteOneAsync(filter));
            return BadRequest();
        }

        /// <summary>
        /// Gets a MongoDb collection by name.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <returns>The MongoDb collection.</returns>
        [NonAction]
        protected IMongoCollection<BsonDocument> GetCollection(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<BsonDocument>(collection);
        }

        /// <summary>
        /// Tries to get an ObjectId with the name of _id or Id and create an equality filter.
        /// Otherwise, creates an empty filter.
        /// </summary>
        /// <param name="document">The docuemnt.</param>
        /// <param name="filter">The created _id filter.</param>
        /// <returns>true if an equality filter could be made; otherwise, false</returns>
        [NonAction]
        private bool TryIdFilter(BsonDocument document, out FilterDefinition<BsonDocument> filter)
        {
            if (document.TryGetValue(MongoDbExtensions.Id, out var oid) && oid.IsObjectId ||
                document.TryGetValue(nameof(MongoDbDocument.Id), out oid) && oid.IsObjectId)
            {
                filter = Builders<BsonDocument>.Filter.Eq(MongoDbExtensions.Id, oid.AsObjectId);
                return true;
            }
            filter = FilterDefinition<BsonDocument>.Empty;
            return false;
        }
    }
}
