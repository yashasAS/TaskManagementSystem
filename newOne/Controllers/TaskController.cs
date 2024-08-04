using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newOne.Models;
using newOne.Service.Interfaces;
using NuGet.Packaging.Signing;
using DbTask = newOne.Models.Task;

namespace newOne.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IReportAndStatzCalculator _taskService;

        public TaskController(IReportAndStatzCalculator taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// using this API , manager and empoyees can track their progress on tasks 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getTaskDetails")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTaskDetails([FromQuery ]IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return BadRequest();
            }

            var tasksDetails = await _taskService.GetByTaskIds(ids);

            if (tasksDetails == null || tasksDetails.Count() == 0)
            {
                return NotFound();
            }

            return Ok(tasksDetails);
        }

        /// <summary>
        /// gets task by assignement
        /// using this API , manager and empoyees can track their progress on tasks 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("getTaskDetailsForUsers")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAssignedTasksForUsers([FromQuery] IEnumerable<int> ids)
        {
            if(ids == null || ids.Count() == 0)
            {
                return BadRequest("Provide atleast one taskId");
            }

            var tasksDetails = await _taskService.GetTasksByAssignedToUser(ids);

            if(tasksDetails == null || tasksDetails.Count() == 0)
            {
                return NotFound();
            }

            return Ok(tasksDetails);
        }

        /// <summary>
        /// deletes task
        /// Any one can delete task, irrespective of level of authorisation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("deleteTask/taskId/{taskId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                // delete Task from db
                await _taskService.DeleteTask(id);
                return Ok();
            }
            catch(HttpRequestException ex)
            {
                // this if return appropriate status code task is not in DB
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                throw;
            }
        }

        /// <summary>
        /// update documents, notes and document of task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateTask([FromBody] DbTask task, [FromBody] Note note, [FromBody] Document document)
        {
            if(task.TaskId != note.TaskId || note.TaskId != document.TaskId || task.TaskId != document.TaskId || task.TaskId == 0)
            {
                return BadRequest("Task Ids should be same or it is 0");
            }
            try
            {
                // update task, notes, and document
                await _taskService.UpdateCompleteTask(task, note, document);
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                // this if return appropriate status code id task is not in DB
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                throw;
            }
        }

        /// <summary>
        /// get all documents, notes and document related to task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getSuperTask/taskId/{taskId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompleteTaskDetails(int taskId)
        {
            if (taskId <= 0)
            {
                return BadRequest("incorrect taskId");
            }

            try
            {
                // update task, notes, and document
                await _taskService.GetCompleteDetailsOfTask(taskId);
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                // this if return appropriate status code id task is not in DB
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                throw;
            }
        }

        /// <summary>
        /// get all documents, notes and document of task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getOverDueTasks/date/{date}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetOverDueTaskDetails(DateTime date, [FromQuery] string teamName = null)
        {
            var overdueTasks = await _taskService.GetOverDueTasks(date, teamName);

            if (!overdueTasks.Any())
            {
                return NotFound("All tasks are completed for the given date");
            }

            return Ok(overdueTasks);
        }
    }
}
