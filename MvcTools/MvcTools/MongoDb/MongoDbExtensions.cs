// MvcTools.MongoDbExtensions.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;
    using MongoDB.Driver;

    /// <summary>
    /// Extensions for MongoDb
    /// </summary>
    public static class MongoDbExtensions
    {
        /// <summary>
        /// Update options to do an upsert.
        /// </summary>
        private static readonly UpdateOptions Upsert = new UpdateOptions { IsUpsert = true };

        /// <summary>
        /// Adds transient DI for a MongoDb connection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connection">MongoDb connection string.</param>
        public static void AddMongoClientIoC(this IServiceCollection services, string connection)
        {
            services.AddTransient<IMongoClient>(x => new MongoClient(connection));
        }

        /// <summary>
        /// Adds transient DI for a MongoDb connection and database.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connection">MongoDb connection string.</param>
        /// <param name="database">Database name.</param>
        public static void AddMongoClientIoC(this IServiceCollection services, string connection, string database)
        {
            services.AddTransient(x => new MongoClient(connection).GetDatabase(database));
        }

        /// <summary>
        /// Deletes the document, finding it by Id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="document">The document to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        public static async Task<DeleteResult> DeleteOneAsync<TDocument>(this IMongoCollection<TDocument> collection, TDocument document) where TDocument : MongoDbDocument
        {
            return await collection.DeleteOneAsync(existing => existing.Id == document.Id);
        }

        /// <summary>
        /// Deletes multiple documents, finding them by Id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="documents">The documents to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        public static async Task<DeleteResult> DeleteManyAsync<TDocument>(this IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents)
            where TDocument : MongoDbDocument
        {
            var enumerable = documents as ICollection<TDocument> ?? documents.ToList();
            var filter = Builders<TDocument>.Filter.Eq(d => d.Id, enumerable.First().Id);
            filter = enumerable.Aggregate(filter, (current, document) => current | Builders<TDocument>.Filter.Eq(d => d.Id, document.Id));
            return await collection.DeleteManyAsync(filter);
        }

        /// <summary>
        /// Gets the entire collection.
        /// </summary>
        /// <remarks>This should not be used for large collections.</remarks>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <returns>An <see cref="IList{T}" /> containing all the elements of the collection.</returns>
        public static async Task<IList<TDocument>> FindAllAsync<TDocument>(this IMongoCollection<TDocument> collection)
        {
            return await collection.Find(FilterDefinition<TDocument>.Empty).ToListAsync();
        }

        /// <summary>
        /// Saves the document by inserting it into the collection or replacing an existing document with the same id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="document">The document to save.</param>
        /// <returns>The result of the update operation.</returns>
        public static async Task<ReplaceOneResult> SaveAsync<TDocument>(this IMongoCollection<TDocument> collection, TDocument document) where TDocument : MongoDbDocument
        {
            return await collection.ReplaceOneAsync(existing => existing.Id == document.Id, document, Upsert);
        }

        /// <summary>
        /// Saves the documents by deleting all existing documents and then inserting them into the collection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="documents">The documents to save.</param>
        /// <returns>The result of the update operation.</returns>
        public static async Task SaveManyAsync<TDocument>(this IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents) where TDocument : MongoDbDocument
        {
            var enumerable = documents.ToList();
            foreach (var document in enumerable) if (document.Id == default) document.Id = ObjectId.GenerateNewId();
            await collection.DeleteManyAsync(enumerable);
            await collection.InsertManyAsync(enumerable);
        }
    }
}
