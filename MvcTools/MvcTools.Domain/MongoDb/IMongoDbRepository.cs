// MvcTools.Domain.IMongoDbRepository.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.Domain.MongoDb
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;

    public interface IMongoDbRepository<TDocument> 
    {
        /// <summary>
        /// Gets all documents matching the filter.
        /// </summary>
        /// <param name="pagingFilter">Paging options and filtering for the find.</param>
        /// <returns>All documents matching the filter.</returns>
        Task<IList<TDocument>> GetDocumentsAsync(PagingFilter<TDocument> pagingFilter = default);

        /// <summary>
        /// Inserts and returns a document.
        /// </summary>
        /// <param name="document">The document to insert.</param>
        /// <returns>The document after insert.</returns>
        Task<TDocument> InsertDocumentAsync(TDocument document);

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <param name="document">The document to update. Must have an id property.</param>
        /// <returns>A task to await.</returns>
        Task UpdateDocumentAsync(TDocument document);

        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <param name="document">The document to delete.</param>
        /// <returns>A task to await.</returns>
        Task DeleteDocumentAsync(ObjectId document);
    }
}
