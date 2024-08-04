using newOne.Models;
using newOne.Models.CustomClass;
using DbTask = newOne.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace newOne.Service.Interfaces
{
    public interface IReportAndStatzCalculator
    {

        /// <summary>
        /// Get task details by taskId
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetByTaskIds(IEnumerable<int> taskIds);

        /// <summary>
        /// Get all tasks for an user
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetTasksByAssignedToUser(IEnumerable<int> userIds);

        /// <summary>
        /// Delete Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTask(int id);

        /// <summary>
        /// update task , notes and documents related to task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="note"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        Task UpdateCompleteTask(DbTask task, Note note, Document document);

        /// <summary>
        /// get all gets of task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<SuperTaskView> GetCompleteDetailsOfTask(int taskId);

        /// <summary>
        /// Get over due task 
        /// here the user can filter tasks by teamName
        /// </summary>
        /// <param name="date"></param>
        /// <param name="teamName"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetOverDueTasks(DateTime date, string teamName = null);

        /// <summary>
        /// Get incomplete tasks for all team 
        /// </summary>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<DbTask>>> GetIncompleteTaskOfTeam(DateTime dueDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<DbTask>>> GetCompletedTaskOfTeam(DateTime dueDate);

        /// <summary>
        /// get team stats
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        Task<TeamReportStatz> GetTeamStatz(string teamName);

        /// <summary>
        /// Get user statz
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserStatz> GetUserStatz(int userId);

    }
}
