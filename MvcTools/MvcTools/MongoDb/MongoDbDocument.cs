// MvcTools.MongoDbDocument.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    /// <summary>
    /// Represents a MongoDb document.
    /// </summary>
    /// <typeparam name="T">The runtime type of the document.</typeparam>
    public abstract class MongoDbDocument<T>
    {
        /// <summary>
        /// The _id of this document.
        /// </summary>
        public virtual ObjectId Id { get; set; }

        /// <summary>
        /// Implicit conversion to use the document as a filter.
        /// </summary>
        /// <param name="document">The document.</param>
        public static implicit operator FilterDefinition<T>(MongoDbDocument<T> document)
        {
            return Builders<T>.Filter.Eq(MongoDbExtensions.Id, document.Id);
        }
    }
}
