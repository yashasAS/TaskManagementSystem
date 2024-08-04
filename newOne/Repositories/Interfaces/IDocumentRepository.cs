using newOne.Models;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        /// <summary>
        /// Get documents by Ids
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Document>> GetByDocumentId(IEnumerable<int> Ids);

        /// <summary>
        /// Get documents by tasks
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Document>> GetByTaskId(IEnumerable<int> Ids);

        /// <summary>
        /// delete document
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task DeleteDocument(int documentId);

        /// <summary>
        /// Update document
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task UpdateDocument(Document file);

        /// <summary>
        /// create document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task AddDocument(Document document);
    }
}
