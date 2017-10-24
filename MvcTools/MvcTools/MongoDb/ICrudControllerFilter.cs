// MvcTools.ICrudControllerFilter.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using MongoDB.Driver;

    /// <summary>
    /// Provides authentication and filtering for a crud controller.
    /// </summary>
    /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
    public interface ICrudControllerFilter<TDocument>
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
        (FilterDefinition<TDocument> filter, int pageNumber, int pageSize) Filter();
    }

    /// <summary>
    /// Extension methods to help with <see cref="ICrudControllerFilter{TDocument}" />.
    /// </summary>
    public static class CrudControllerFilterExtensions
    {
        /// <summary>
        /// Creates a result for <see cref="ICrudControllerFilter{TDocument}" />.Get() that returns all documents found by <paramref name="filter" />.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
        /// <param name="filter">A filter that gets all or some documents that the current user has access to.</param>
        /// <returns>A result for <see cref="ICrudControllerFilter{TDocument}" />.Get().</returns>
        public static (FilterDefinition<TDocument> filter, int pageNumber, int pageSize) All<TDocument>(this FilterDefinition<TDocument> filter)
        {
            return (filter, -1, -1);
        }
    }

    /// <summary>
    /// Returns all documents for Get requests and allows all Post, Put, and Delete requests.
    /// </summary>
    public class DefaultCrudControllerFilter : ICrudControllerFilter<object>
    {
        /// <inheritdoc />
        public (FilterDefinition<object> filter, int pageNumber, int pageSize) Filter()
        {
            return FilterDefinition<object>.Empty.All();
        }
    }
}
