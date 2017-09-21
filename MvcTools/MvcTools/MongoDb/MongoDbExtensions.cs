// MvcTools.MongoDbExtensions.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
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
        /// Saves the document by inserting it into the collection or replacing an existing document with the same id.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The <see cref="IMongoCollection{TDocument}" />.</param>
        /// <param name="document">The document.</param>
        /// <returns>A task to await.</returns>
        public static async Task Save<TDocument>(this IMongoCollection<TDocument> collection, TDocument document) where TDocument : MongoDbDocument
        {
            await collection.ReplaceOneAsync(existing => existing.Id == document.Id, document, Upsert);
        }
    }
}
