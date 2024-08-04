using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using newOne.Service.Interfaces;

namespace newOne.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "MD")]
    public class ReportAndStatzController : ControllerBase
    {

        private readonly IReportAndStatzCalculator _taskService;

        public ReportAndStatzController(IReportAndStatzCalculator taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get uver due tasks by dueDate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="teamName"></param> teamName is optional param,
        /// if you dont pass teamName then you get overdue tasks of all teams
        /// <returns></returns>
        [HttpGet("getOverDueTasks/dueDate/{dueDate}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetOverDueTaskReport(DateTime dueDate, [FromQuery] string teamName = null)
        {
            var overdueTasks = await _taskService.GetOverDueTasks(dueDate, teamName);

            if (!overdueTasks.Any())
            {
                return NotFound("All tasks are completed for the given dueDate");
            }

            return Ok(overdueTasks);
        }

        /// <summary>
        /// Get uver due tasks by dueDate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="teamName"></param> teamName is optional param,
        /// if you dont pass teamName then you get overdue tasks of all teams
        /// <returns></returns>
        [HttpGet("getTeamStatz/teamname/{teamname}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTeamStatz(string teamname)
        {
            if (string.IsNullOrEmpty(teamname))
            {
                return BadRequest("Team name cannot be null");
            }

            var teamStats = await _taskService.GetTeamStatz(teamname);

            return Ok(teamStats);
        }

        /// <summary>
        /// Get uver due tasks by dueDate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="teamName"></param> teamName is optional param,
        /// if you dont pass teamName then you get overdue tasks of all teams
        /// <returns></returns>
        [HttpGet("getIncompleteTaskReport/dueDate/{dueDate}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetIncompleteTasksReport(DateTime dueDate)
        {
            var overdueTasks = await _taskService.GetIncompleteTaskOfTeam(dueDate);

            return Ok(overdueTasks);
        }

        /// <summary>
        /// Get uver due tasks by dueDate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="teamName"></param> teamName is optional param,
        /// if you dont pass teamName then you get overdue tasks of all teams
        /// <returns></returns>
        [HttpGet("getCompletedTaskReport/dueDate/{dueDate}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompletedTasksReport(DateTime dueDate)
        {
            var completedTasks = await _taskService.GetCompletedTaskOfTeam(dueDate);

            return Ok(completedTasks);
        }

        /// <summary>
        /// Get uver due tasks by dueDate
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="teamName"></param> teamName is optional param,
        /// if you dont pass teamName then you get overdue tasks of all teams
        /// <returns></returns>
        [HttpGet("getUserReport/userId/{userId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUserStatz(int userId)
        {
            if(userId <= 0)
            {
                return BadRequest("Invalid UserId");
            }

            var completedTasks = await _taskService.GetUserStatz(userId);

            return Ok(completedTasks);
        }
    }
}
