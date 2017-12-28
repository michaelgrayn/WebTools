// MvcTools.Domain.PagingFilter.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Domain.MongoDb
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides filtering an paging for repositories.
    /// </summary>
    public class PagingFilter<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingFilter{TDocument}" /> class.
        /// </summary>
        /// <param name="filter">A filter that gets all or some documents that the current user has access to.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        public PagingFilter(Expression<Func<TDocument, bool>> filter = null, int pageNumber = -1, int pageSize = -1)
        {
            Filter = filter;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets a filter to get all or some documents that the current user has access to.
        /// </summary>
        public Expression<Func<TDocument, bool>> Filter { get; }

        /// <summary>
        /// Gets the current page number.
        /// -1 or lower indicates no paging.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the page size.
        /// -1 or lower indicates no paging.
        /// </summary>
        public int PageSize { get; }
    }
}
