// MvcTools.CrudControllerFilter.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.MongoDb
{
    using MongoDB.Driver;

    /// <summary>
    /// Provides filtering an paging for GET requests.
    /// </summary>
    public class CrudControllerFilter<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudControllerFilter{TDocument}" /> class.
        /// </summary>
        public CrudControllerFilter()
        {
            Filter = FilterDefinition<TDocument>.Empty;
            PageNumber = -1;
            PageSize = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudControllerFilter{TDocument}" /> class.
        /// </summary>
        /// <param name="filter">A filter that gets all or some documents that the current user has access to.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        public CrudControllerFilter(FilterDefinition<TDocument> filter, int pageNumber, int pageSize)
        {
            Filter = filter;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets a filter to get all or some documents that the current user has access to.
        /// </summary>
        public FilterDefinition<TDocument> Filter { get; }

        /// <summary>
        /// Gets the current page number.
        /// -1 indicates no pages.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page size.
        /// -1 indicates no pages.
        /// </summary>
        public int PageSize { get; }
    }

    /// <summary>
    /// Extension methods to help with <see cref="CrudControllerFilter{TDocument}" />.
    /// </summary>
    public static class CrudControllerFilterExtensions
    {
        /// <summary>
        /// Creates a <see cref="CrudControllerFilter{TDocument}" />.
        /// </summary>
        /// <typeparam name="TDocument">The type of the documents in the collection.</typeparam>
        /// <param name="filter">A filter that gets all or some documents that the current user has access to.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A result for <see cref="CrudControllerFilter{TDocument}" />.Get().</returns>
        public static CrudControllerFilter<TDocument> CrudFilter<TDocument>(this FilterDefinition<TDocument> filter, int pageNumber = -1, int pageSize = -1)
        {
            return new CrudControllerFilter<TDocument>(filter, pageNumber, pageSize);
        }
    }
}
