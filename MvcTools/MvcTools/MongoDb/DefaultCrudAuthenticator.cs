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
    public class DefaultCrudAuthenticator : ICrudAuthenticator<object>
    {
        /// <inheritdoc />
        public (FilterDefinition<object> filter, int pageNumber, int pageSize) Get()
        {
            return FilterDefinition<object>.Empty.All();
        }

        /// <inheritdoc />
        public bool CanPost(object document)
        {
            return true;
        }

        /// <inheritdoc />
        public bool CanPut(object document)
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
