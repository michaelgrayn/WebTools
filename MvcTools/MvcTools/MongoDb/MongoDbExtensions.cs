﻿// MvcTools.MongoDbExtensions.cs
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
    /// Extensions for MongoDb.
    /// </summary>
    public static class MongoDbExtensions
    {
        /// <summary>
        /// Update options to do an upsert.
        /// </summary>
        private static readonly UpdateOptions Upsert = new UpdateOptions { IsUpsert = true };

        /// <summary>
        /// The name of MongoDb's id field.
        /// </summary>
        public const string Id = "_id";

        /// <summary>
        /// Adds transient DI for a MongoDb connection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connection">MongoDb connection string.</param>
        /// <returns>The service collection after adding MongoDb DI.</returns>
        public static IServiceCollection AddMongoClientIoC(this IServiceCollection services, string connection)
        {
            return services.AddTransient<IMongoClient>(x => new MongoClient(connection));
        }

        /// <summary>
        /// Adds transient DI for a MongoDb connection and database.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connection">MongoDb connection string.</param>
        /// <param name="database">Database name.</param>
        /// <returns>The service collection after adding MongoDb DI.</returns>
        public static IServiceCollection AddMongoClientIoC(this IServiceCollection services, string connection, string database)
        {
            return services.AddTransient(x => new MongoClient(connection).GetDatabase(database));
        }

        /// <summary>
        /// Counts the number of documents in the collection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <returns>The number of documents in the collection.</returns>
        public static async Task<long> CountAsync<TDocument>(this IMongoCollection<TDocument> collection)
        {
            return await collection.CountAsync(FilterDefinition<TDocument>.Empty);
        }

        /// <summary>
        /// Creates a filter that finds all the documents by _id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="documents">The documents to create a filter for.</param>
        /// <returns>A filter that finds all the documents by _id.</returns>
        public static FilterDefinition<TDocument> CreateMultiIdFilter<TDocument>(IEnumerable<TDocument> documents) where TDocument : MongoDbDocument
        {
            return Builders<TDocument>.Filter.In(document => document.Id, documents.Select(document => document.Id));
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
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="documents">The documents to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        public static async Task<DeleteResult> DeleteManyAsync<TDocument>(this IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents)
            where TDocument : MongoDbDocument
        {
            var filter = CreateMultiIdFilter(documents);
            return await collection.DeleteManyAsync(filter);
        }

        /// <summary>
        /// Gets the entire collection.
        /// </summary>
        /// <remarks>This should not be used for large collections.</remarks>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <returns>An <see cref="IList{T}" /> containing all the elements of the collection.</returns>
        public static async Task<IList<TDocument>> FindAllAsync<TDocument>(this IMongoCollection<TDocument> collection)
        {
            return await collection.Find(FilterDefinition<TDocument>.Empty).ToListAsync();
        }

        /// <summary>
        /// Gets documents by _ids.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="ids">The _ids of the documents to find.</param>
        /// <returns>Documents with an _id in <paramref name="ids" />.</returns>
        public static async Task<IList<TDocument>> FindByIdAsync<TDocument>(this IMongoCollection<TDocument> collection, IEnumerable<ObjectId> ids)
        {
            return await collection.Find(Builders<TDocument>.Filter.In(Id, ids)).ToListAsync();
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
        /// Saves the documents by doing a bulk upsert. If a document has the default value for it's Id field, then a new Id will be generated.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="documents">The documents to save.</param>
        /// <returns>The result of the update operation.</returns>
        public static async Task SaveManyAsync<TDocument>(this IMongoCollection<TDocument> collection, IEnumerable<TDocument> documents) where TDocument : MongoDbDocument
        {
            var models = documents.AsParallel().Select(document =>
            {
                if (document.Id == default) document.Id = ObjectId.GenerateNewId();
                return new ReplaceOneModel<TDocument>(Builders<TDocument>.Filter.Eq(filter => filter.Id, document.Id), document) { IsUpsert = true };
            });
            await collection.BulkWriteAsync(models);
        }
    }
}
