using newOne.Models;
using Task = System.Threading.Tasks.Task;

namespace newOne.Repositories.Interfaces
{
    public interface INotesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Note>> GetByNoteIds(IEnumerable<int> Ids);


        /// <summary>
        /// Get by task Ids
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Note>> GetByTaskId(IEnumerable<int> Ids);

        /// <summary>
        /// Update notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        Task UpdateNotes(int noteId, string content);

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        Task DeleteNotes(int noteId);
        /// <summary>
        /// create note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Task AddNote(int noteId, int taskId, string content);

       /// <summary>
       /// Update notes
       /// </summary>
       /// <param name="note"></param>
       /// <returns></returns>
        Task UpdateNotes(Note note);

    }
}
