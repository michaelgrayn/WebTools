﻿// MvcTools.Infrastructure.MongoDbExtensions.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MvcTools.Domain.MongoDb;

    /// <summary>
    /// Extensions for MongoDb.
    /// </summary>
    public static class MongoDbExtensions
    {
        /// <summary>
        /// The name of MongoDb's id field.
        /// </summary>
        public const string Id = "_id";

        /// <summary>
        /// Update options to do an upsert.
        /// </summary>
        private static readonly UpdateOptions _upsert = new UpdateOptions { IsUpsert = true };

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
        /// Converts a <see cref="MongoDbDocument" /> to a <see cref="FilterDefinition{TDocument}" />.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static FilterDefinition<TDocument> Filter<TDocument>(this MongoDbDocument document)
        {
            return Builders<TDocument>.Filter.Eq(Id, document.Id);
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
        /// Gets a document by _id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="id">The _id of the document to find.</param>
        /// <returns>A document with the _id of <paramref name="id" />.</returns>
        /// <exception cref="System.InvalidOperationException">The input sequence contains more than one element or the input sequence is empty.</exception>
        public static async Task<TDocument> FindByIdAsync<TDocument>(this IMongoCollection<TDocument> collection, ObjectId id)
        {
            return await collection.Find(Builders<TDocument>.Filter.Eq(Id, id)).SingleAsync();
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
            return await collection.ReplaceOneAsync(document.Filter<TDocument>(), document, _upsert);
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
                return new ReplaceOneModel<TDocument>(document.Filter<TDocument>(), document) { IsUpsert = true };
            });
            await collection.BulkWriteAsync(models);
        }
    }
}
