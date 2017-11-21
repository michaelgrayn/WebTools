// MvcTools.CrudControllerBase.cs
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
    public abstract class CrudControllerBase<TDocument> : FluentControllerBase
    {
        /// <summary>
        /// The MongoDb collection.
        /// </summary>
        private readonly IMongoCollection<TDocument> _collection;

        /// <summary>
        /// Settings to correctly convert a <see cref="BsonDocument" /> to JSON.
        /// </summary>
        private readonly JsonWriterSettings _jsonWriterSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudControllerBase{TDocument}" /> class.
        /// </summary>
        /// <param name="client">MongoDb client.</param>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        protected CrudControllerBase(IMongoClient client, string database, string collection)
        {
            _collection = client.GetDatabase(database).GetCollection<TDocument>(collection);
            _jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
        }

        /// <summary>
        /// Gets all documents matching the filter.
        /// </summary>
        /// <param name="filter">Provides filtering and paging options.</param>
        /// <returns>All documents matching the filter.</returns>
        public virtual async Task<IActionResult> GetDocumentsAsync(CrudControllerFilter<TDocument> filter = null)
        {
            return await Action(async () =>
            {
                filter = filter ?? new CrudControllerFilter<TDocument>();
                var find = _collection.Find(filter.Filter).Sort(Builders<TDocument>.Sort.Ascending(MongoDbExtensions.Id));
                if (filter.PageNumber > -1 && filter.PageSize > -1) find = find.Skip(filter.PageNumber * filter.PageSize).Limit(filter.PageSize);
                return (await find.ToListAsync()).ToJson(_jsonWriterSettings);
            }).Success(JsonString).ResponseAsync();
        }

        /// <summary>
        /// Inserts a document.
        /// </summary>
        /// <param name="document">The document to insert.</param>
        /// <returns>The document after insert.</returns>
        public virtual async Task<IActionResult> PostDocumentAsync([FromBody] TDocument document)
        {
            return await Action(async () =>
            {
                await _collection.InsertOneAsync(document);
                return document.ToJson(_jsonWriterSettings);
            }).Success(JsonString).ResponseAsync();
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <param name="document">The document to update. Must have an id property.</param>
        /// <returns>The replace result.</returns>
        public virtual async Task<IActionResult> PutDocumentAsync([FromBody] TDocument document)
        {
            return await Action(async () =>
            {
                FilterDefinition<TDocument> idFilter;
                if (document is MongoDbDocument<TDocument> documentFilter) idFilter = documentFilter;
                else idFilter = IdFilter(document.ToBsonDocument().GetValue(MongoDbExtensions.Id).AsObjectId);
                return await _collection.ReplaceOneAsync(idFilter, document);
            }).Success(Json).ResponseAsync();
        }

        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <param name="document">The document to delete.</param>
        /// <returns>The delete result.</returns>
        public virtual async Task<IActionResult> DeleteDocumentAsync(ObjectId document)
        {
            return await Action(async () => await _collection.DeleteOneAsync(IdFilter(document))).Success(Json).ResponseAsync();
        }

        /// <summary>
        /// Creates a <see cref="FilterDefinition{TDocument}" /> for _id.
        /// </summary>
        /// <param name="id">The ObjectId of the document to find.</param>
        /// <returns>A <see cref="FilterDefinition{TDocument}" /> for _id.</returns>
        private static FilterDefinition<TDocument> IdFilter(ObjectId id)
        {
            return Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, id);
        }
    }
}
