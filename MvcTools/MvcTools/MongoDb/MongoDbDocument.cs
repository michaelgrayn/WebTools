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
    /// <typeparam name="TDocument">The runtime type of the document.</typeparam>
    public abstract class MongoDbDocument<TDocument>
    {
        /// <summary>
        /// The _id of this document.
        /// </summary>
        public virtual ObjectId Id { get; set; }

        /// <summary>
        /// Implicit conversion to use the document as a filter.
        /// </summary>
        /// <param name="document">The document.</param>
        public static implicit operator FilterDefinition<TDocument>(MongoDbDocument<TDocument> document)
        {
            return Builders<TDocument>.Filter.Eq(MongoDbExtensions.Id, document.Id);
        }
    }
}
