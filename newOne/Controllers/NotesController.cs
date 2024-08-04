using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newOne.Repositories.Interfaces;
using newOne.Models;
using Task = System.Threading.Tasks.Task;

namespace newOne.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _notesRepository;
        public NotesController(INotesRepository notesRepository)
        {
            notesRepository = _notesRepository;
        }

        [HttpGet("getNotesForATask/taskId/{taskId}")]
        public async Task<IActionResult> GetAllNotesForTask(int taskId)
        {
            var taskIds = new List<int>() { taskId };
            var notes = await _notesRepository.GetByTaskId(taskIds);

            if(notes.Count() == 0 || !notes.Any())
            {
                return NotFound();
            }
            return Ok(notes);
        }

        [HttpPost("createNotesForATask/taskId/{taskId}")]
        public async Task<IActionResult> CreateNotesForTask(int taskId, [FromQuery]int noteId, [FromQuery] string content )
        {

            if (taskId <= 0 || noteId <= 0)
            {
                return BadRequest("noteId or taskId is invalid");
            }

            var taskIds = new List<int>() { taskId };

            try
            {
                await _notesRepository.AddNote(taskId, noteId, content);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("note already exists");
            }
        }

        [HttpPut("updateNote/noteId/{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromQuery] string content)
        {

            if (noteId <= 0)
            {
                return BadRequest("noteId is invalid");
            }
            try
            {
                await _notesRepository.UpdateNotes(noteId, content);
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                // this if return appropriate status code note is not in DB
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                throw;
            }
        }

        [HttpDelete("deleteNote/noteId/{noteId}")]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            if (noteId <= 0)
            {
                return BadRequest("noteId is invalid");
            }

            await _notesRepository.DeleteNotes(noteId);
            return Ok();
        }
    }
}
