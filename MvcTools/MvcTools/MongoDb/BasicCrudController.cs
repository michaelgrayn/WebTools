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
    /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
    [NonController]
    public abstract class BasicCrudController<TDocument> : FluentControllerBase
    {
        /// <summary>
        /// The MongoDb client.
        /// </summary>
        private readonly IMongoClient _client;

        /// <summary>
        /// Provides authentication and filtering for a crud controller.
        /// </summary>
        private readonly ICrudAuthenticator<TDocument> _authenticator;

        /// <summary>
        /// Settings to correctly convert a <see cref="BsonDocument" /> to JSON.
        /// </summary>
        private readonly JsonWriterSettings _jsonWriterSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicCrudController{TDocument}" /> class.
        /// </summary>
        /// <param name="client">MongoDb client.</param>
        /// <param name="authenticator">Provides authentication and filtering for a crud controller.</param>
        protected BasicCrudController(IMongoClient client, ICrudAuthenticator<TDocument> authenticator)
        {
            _client = client;
            _authenticator = authenticator;
            _jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
        }

        /// <summary>
        /// Gets all documents from <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <returns>All documents in <paramref name="database" />.<paramref name="collection" />.</returns>
        public virtual async Task<IActionResult> GetDocumentsAsync(string database, string collection)
        {
            var get = _authenticator.Get();
            var sort = Builders<TDocument>.Sort.Ascending(MongoDbExtensions.Id);
            var find = GetCollection(database, collection).Find(get.filter);
            if (get.pageNumber > -1 && get.pageSize > -1) find = find.Sort(sort).Skip(get.pageNumber * get.pageSize).Limit(get.pageSize);
            var documents = await find.ToListAsync();
            return JsonString(documents.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Inserts a document into <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to insert.</param>
        /// <returns>The document after insert.</returns>
        public virtual async Task<IActionResult> PostDocumentAsync(string database, string collection, [FromBody] TDocument document)
        {
            if (!_authenticator.CanPost(document)) return BadRequest();
            await GetCollection(database, collection).InsertOneAsync(document);
            return JsonString(document.ToJson(_jsonWriterSettings));
        }

        /// <summary>
        /// Updates a document in <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to update. Must have an id property.</param>
        /// <returns>The replace result.</returns>
        public virtual async Task<IActionResult> PutDocumentAsync(string database, string collection, [FromBody] TDocument document)
        {
            bool TryIdFilter(TDocument d, out FilterDefinition<TDocument> f)
            {
                if (d is MongoDbDocument<TDocument> documentFilter)
                {
                    f = documentFilter;
                    return true;
                }
                var bson = document.ToBsonDocument();
                if (bson.TryGetValue(MongoDbExtensions.Id, out var oid))
                {
                    f = Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, oid.AsObjectId);
                    return true;
                }
                f = FilterDefinition<TDocument>.Empty;
                return false;
            }

            if (_authenticator.CanPut(document) && TryIdFilter(document, out var filter))
                return Json(await GetCollection(database, collection).ReplaceOneAsync(filter, document));
            return BadRequest();
        }

        /// <summary>
        /// Deletes a document in <paramref name="database" />.<paramref name="collection" />.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <param name="document">The document to delete. Must have an id.</param>
        /// <returns>The delete result.</returns>
        public virtual async Task<IActionResult> DeleteDocumentAsync(string database, string collection, ObjectId document)
        {
            var filter = Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, document);
            if (_authenticator.CanDelete(document)) return Json(await GetCollection(database, collection).DeleteOneAsync(filter));
            return BadRequest();
        }

        /// <summary>
        /// Gets a MongoDb collection by name.
        /// </summary>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        /// <returns>The MongoDb collection.</returns>
        protected IMongoCollection<TDocument> GetCollection(string database, string collection)
        {
            return _client.GetDatabase(database).GetCollection<TDocument>(collection);
        }
    }
}
