// MvcTools.BasicCrudController.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    /// <summary>
    /// Provides authentication and filtering for a crud controller.
    /// </summary>
    /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
    public interface ICrudAuthenticator<TDocument>
    {
        /// <summary>
        /// Provides a filter to get all or some documents that the current user has access to.
        /// Optionally you can return a subset of the documents in the form of pages.
        /// Page number starts at zero. If either the page number or size are negative
        /// then all documents are returned.
        /// </summary>
        /// <returns>
        /// A filter to the current user's documents,
        /// a page number starting at zero,
        /// and a page size.
        /// </returns>
        (FilterDefinition<TDocument> filter, int pageNumber, int pageSize) Get();

        /// <summary>
        /// Determines if the current user can post this <paramref name="document"/>.
        /// </summary>
        /// <param name="document">The document being posted.</param>
        /// <returns>true if the current user can post this <paramref name="document"/>; otherwise, false.</returns>
        bool CanPost(TDocument document);

        /// <summary>
        /// Determines if the current user can post this <paramref name="document"/>.
        /// </summary>
        /// <param name="document">The document being put.</param>
        /// <returns>true if the current user can put this <paramref name="document"/>; otherwise, false.</returns>
        bool CanPut(TDocument document);

        /// <summary>
        /// Determines if the current user can delete this <paramref name="document"/>.
        /// </summary>
        /// <param name="document">The document being deleted.</param>
        /// <returns>true if the current user can delete this <paramref name="document"/>; otherwise, false.</returns>
        bool CanDelete(ObjectId document);
    }

    /// <summary>
    /// Extension methods to help with <see cref="ICrudAuthenticator{TDocument}"/>.
    /// </summary>
    public static class CrudAuthenticatorExtensions
    {
        /// <summary>
        /// Creates a result for <see cref="ICrudAuthenticator{TDocument}"/>.Get() that returns all documents found by <paramref name="filter"/>.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
        /// <param name="filter">A filter that gets all or some documents that the current user has access to.</param>
        /// <returns>A result for <see cref="ICrudAuthenticator{TDocument}"/>.Get().</returns>
        public static (FilterDefinition<TDocument> filter, int pageNumber, int pageSize) All<TDocument>(this FilterDefinition<TDocument> filter)
        {
            return (filter, -1, -1);
        }
    }
}
