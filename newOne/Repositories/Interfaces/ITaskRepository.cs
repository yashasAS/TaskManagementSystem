using DbTask = newOne.Models.Task;

namespace newOne.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        /// <summary>
        /// GetByTaskId
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetByTaskIds(IEnumerable<int> Ids);

        /// <summary>
        /// Get who is working on the task
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetByAssignedToUserIds(IEnumerable<int> userIds);

        /// <summary>
        /// Get who created the tasks
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetByCreatedByUserIds(IEnumerable<int> userIds);

        /// <summary>
        /// Get completed Tasks
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetTasksByStatus(string status);

        /// <summary>
        /// Get completed tasks by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetCompletedTasksByDate(DateTime date);

        /// <summary>
        /// Update Tasks
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="newTitle"></param>
        /// <param name="newDescription"></param>
        /// <param name="newDueDate"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateTaskAsync(int taskId, string newTitle, string newDescription, DateTime newDueDate);

        /// <summary>
        /// Delete Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task DeleteTaskAsync(int taskId);

        /// <summary>
        /// Add task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task AddTask(DbTask task);

        /// <summary>
        /// update complete task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateCompleteTaskRecord(DbTask task);

        /// <summary>
        /// get over due tasks by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<DbTask>> GetOverDueTasksByDate(DateTime date);
    }
}
