// MvcTools.Infrastructure.MongoDbRepository.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MvcTools.Domain.MongoDb;

    /// <summary>
    /// A repository for MongoDb collections that handles basic crud operations.
    /// </summary>
    /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
    public class MongoDbRepository<TDocument> : IMongoDbRepository<TDocument>
    {
        /// <summary>
        /// The MongoDb collection.
        /// </summary>
        private readonly IMongoCollection<TDocument> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository{TDocument}" /> class.
        /// </summary>
        /// <param name="collection">MongoDb collection of <typeparamref name="TDocument" />.</param>
        public MongoDbRepository(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository{TDocument}" /> class.
        /// </summary>
        /// <param name="client">MongoDb client.</param>
        /// <param name="database">Database name.</param>
        /// <param name="collection">Collection name.</param>
        public MongoDbRepository(IMongoClient client, string database, string collection)
        {
            _collection = client.GetDatabase(database).GetCollection<TDocument>(collection);
        }

        /// <inheritdoc />
        public async Task<IList<TDocument>> GetDocumentsAsync(PagingFilter<TDocument> pagingFilter = default)
        {
            var find = _collection.Find(pagingFilter.Filter ?? FilterDefinition<TDocument>.Empty).Sort(Builders<TDocument>.Sort.Ascending(MongoDbExtensions.Id));
            if (pagingFilter.PageNumber > -1 && pagingFilter.PageSize > -1) find = find.Skip(pagingFilter.PageNumber * pagingFilter.PageSize).Limit(pagingFilter.PageSize);
            return await find.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<TDocument> InsertDocumentAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(TDocument document)
        {
            FilterDefinition<TDocument> idFilter;
            if (document is MongoDbDocument documentFilter)
            {
                idFilter = documentFilter.Filter<TDocument>();
            }
            else
            {
                var id = document.ToBsonDocument().GetValue(MongoDbExtensions.Id).AsObjectId;
                idFilter = Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, id);
            }

            await _collection.ReplaceOneAsync(idFilter, document);
        }

        /// <inheritdoc />
        public async Task DeleteDocumentAsync(ObjectId document)
        {
            await _collection.DeleteOneAsync(Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, document));
        }
    }
}
