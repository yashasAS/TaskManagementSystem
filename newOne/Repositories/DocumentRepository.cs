using Microsoft.EntityFrameworkCore;
using newOne.Models;
using newOne.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly newOneContext _dbContext;
        public DocumentRepository (newOneContext db)
        {
            _dbContext = db;
        }

        public async Task<IEnumerable<Document>> GetByDocumentId(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Documents
                .AsQueryable()
                .Where(d => Ids.Contains(d.DocumentId))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<Document>> GetByTaskId(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Documents
                .AsQueryable()
                .Where(d =>d.TaskId.HasValue && Ids.Contains(d.TaskId.GetValueOrDefault()))
                .ToListAsync();

            return dbItems;
        }

        public async Task DeleteDocument(int documentId)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);

            if (document != null)
            {
                _dbContext.Documents.Remove(document);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("document not found");
            }
        }

        public async Task UpdateDocument(Document file)
        {
            var documentId = file.DocumentId;

            var document = await _dbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);

            if(document != null)
            {
                _dbContext.Update(document);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("document not found");
            }
        }

        public async Task AddDocument(Document document)
        {
            _dbContext.Add(document);
            await _dbContext.SaveChangesAsync();
        }
    }
}
