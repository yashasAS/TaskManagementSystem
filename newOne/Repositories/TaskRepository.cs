using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using newOne.Models;
using newOne.Repositories.Interfaces;
using DbTask = newOne.Models.Task;
using System.Threading.Tasks;
using System.Linq;

namespace newOne.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly newOneContext _dbContext;
        public TaskRepository(newOneContext db) 
        {
            _dbContext = db;
        }

        public async Task<IEnumerable<DbTask>> GetByTaskIds(IEnumerable<int> Ids)
        {
            var dbItems = await _dbContext.Tasks
                .Where(t => Ids.Contains(t.TaskId))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<DbTask>> GetByAssignedToUserIds(IEnumerable<int> userIds)
        {
            // Retrieve the task from the database
            var dbItems = await _dbContext.Tasks
                .AsQueryable()
                .Where(t => t.AssignedTo.HasValue && userIds.Contains(t.AssignedTo.Value))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<DbTask>> GetByCreatedByUserIds(IEnumerable<int> userIds)
        {
            // Retrieve the task from the database
            var dbItems = await _dbContext.Tasks
                .AsQueryable()
                .Where(t => t.CreatedBy.HasValue && userIds.Contains(t.CreatedBy.Value))
                .ToListAsync();
            return dbItems;
        }

        public async Task<IEnumerable<DbTask>> GetTasksByStatus(string status)
        {
            // Retrieve the task from the database
            var dbItems = await _dbContext.Tasks
                .AsQueryable()
                .Where(t => status.Contains(t.Status))
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<DbTask>> GetCompletedTasksByDate(DateTime date)
        {
            // Retrieve the task from the database
            var dbItems = await _dbContext.Tasks
                .AsQueryable()
                .Where(t => t.DueDate <= date && t.Status == "completed")
                .ToListAsync();

            return dbItems;
        }

        public async Task<IEnumerable<DbTask>> GetOverDueTasksByDate(DateTime date)
        {
            // Retrieve the task from the database
            var dbItems = await _dbContext.Tasks
                .AsQueryable()
                .Where(t => t.DueDate >= date && t.Status != "completed")
                .ToListAsync();

            return dbItems;
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int taskId, string newTitle, string newDescription, DateTime newDueDate)
        {
            // Retrieve the task from the database
            var task = await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task != null)
            {
                // Update the properties
                task.Title = newTitle;
                task.Description = newDescription;
                task.DueDate = newDueDate;

                // Save changes back to the database
                 await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the task was not found
                throw new Exception("Task not found.");
            }
        }

        public async System.Threading.Tasks.Task UpdateCompleteTaskRecord(DbTask task)
        {
            var taskId = task.TaskId;

            // Retrieve the task from the database
            var taskInDb = await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if(taskInDb != null)
            {
                // update task from context
                _dbContext.Tasks.Update(task);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the task was not found
                throw new Exception("Task not found.");
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int taskId)
        {
            // Retrieve the task from the database
            var task = await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task != null)
            {
                // Remove the task from the context
                _dbContext.Tasks.Remove(task);

                // Save changes back to the database
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the task was not found
                throw new Exception("Task not found.");
            }
        }

        public async System.Threading.Tasks.Task AddTask(DbTask task)
        {
            // Add to database
            _dbContext.Add(task);
            await _dbContext.SaveChangesAsync();
        }
    }
}
