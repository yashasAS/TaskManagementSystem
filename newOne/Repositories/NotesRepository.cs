using Microsoft.EntityFrameworkCore;
using newOne.Models;
using newOne.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories
{
    public class NotesRepository :  INotesRepository
    {

        private readonly newOneContext _dbContext;
        public NotesRepository(newOneContext db)
        {
            _dbContext = db;
        }

        public async Task<IEnumerable<Note>> GetByNoteIds(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Notes
                .AsQueryable()
                .Where(n => Ids.Contains(n.NoteId))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<Note>> GetByTaskId(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Notes
                .AsQueryable()
                .Where(n => Ids.Contains(n.NoteId))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<Note>> GetNotesByString(string stringNoteText)
        {
            var dbItems = await _dbContext.Notes
                .AsQueryable()
                .Where(n => stringNoteText.Contains(n.Content))
                .ToListAsync();

            return dbItems;
        }

        public async Task DeleteNotes(int noteId)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.NoteId == noteId);

            if (note != null)
            {
                _dbContext.Notes.Remove(note);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("note not found");
            }
        }

        public async Task UpdateNotes(int noteId, string content)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.NoteId == noteId);
             
            if (note != null)
            {
                // update note variable
                note.Content = content;

                _dbContext.Update(note);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("note not found");
            }
        }

        public async Task UpdateNotes(Note note)
        {
            if (note != null)
            {
                _dbContext.Update(note);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("note not found");
            }
        }

        public async Task AddNote(int noteId, int taskId, string content)
        {
            var noteInDb = (await GetByNoteIds(new List<int>() { noteId })).First();

            if(noteInDb != null)
            {
                throw new Exception("note with same noteId exists and cannot insert to database");
            }


            var note = new Note
            {
                TaskId = taskId,
                NoteId = noteId,
                Content = content
            };

            _dbContext.Add(note);
            await _dbContext.SaveChangesAsync();
        }
    }
}
