// MvcTools.DefaultCrudAuthenticator.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    /// <summary>
    /// Returns all documents for Get requests and allows all Post, Put, and Delete requests.
    /// </summary>
    internal class DefaultCrudAuthenticator : ICrudAuthenticator<BsonDocument>
    {
        /// <inheritdoc />
        public (FilterDefinition<BsonDocument> filter, int pageNumber, int pageSize) Get()
        {
            return FilterDefinition<BsonDocument>.Empty.All();
        }

        /// <inheritdoc />
        public bool CanPost(BsonDocument document)
        {
            return true;
        }

        /// <inheritdoc />
        public bool CanPut(BsonDocument document)
        {
            return true;
        }

        /// <inheritdoc />
        public bool CanDelete(ObjectId document)
        {
            return true;
        }
    }
}
