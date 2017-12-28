// MvcTools.MongoDbDocument.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Domain.MongoDb
{
    using MongoDB.Bson;

    /// <summary>
    /// Represents a MongoDb document.
    /// </summary>
    public abstract class MongoDbDocument
    {
        /// <summary>
        /// The _id of this document.
        /// </summary>
        public virtual ObjectId Id { get; set; }
    }
}
