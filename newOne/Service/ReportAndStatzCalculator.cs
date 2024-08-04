using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using newOne.Models;
using newOne.Models.CustomClass;
using newOne.Repositories.Interfaces;
using newOne.Service.Interfaces;
using System.Linq;
using System.Runtime.CompilerServices;
using DbTask = newOne.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace newOne.Service
{
    public class ReportAndStatzCalculator : IReportAndStatzCalculator
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUsersRepository _usersRepository;

        public ReportAndStatzCalculator(ITaskRepository taskRepository,
            INotesRepository notesRepository,
            IDocumentRepository documentRepository,
            IUsersRepository usersRepository)
        {
            _taskRepository = taskRepository;
            _notesRepository = notesRepository;
            _documentRepository = documentRepository;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<DbTask>> GetByTaskIds(IEnumerable<int> taskIds)
        {
            return await _taskRepository.GetByTaskIds(taskIds);
        }

        public async Task<IEnumerable<DbTask>> GetTasksByAssignedToUser(IEnumerable<int> userIds)
        {
            return await _taskRepository.GetByAssignedToUserIds(userIds);
        }

        public async Task DeleteTask(int id)
        {
             await _taskRepository.DeleteTaskAsync(id);
        }

        public async Task UpdateCompleteTask(DbTask task, Note note, Document document)
        {
            var updateTaskCall = _taskRepository.UpdateCompleteTaskRecord(task);
            var updateNotesCall = _notesRepository.UpdateNotes(note);
            var updateDocumentCall =  _documentRepository.UpdateDocument(document);

            await Task.WhenAll(updateTaskCall, updateNotesCall, updateDocumentCall);
        }

        public async Task<SuperTaskView> GetCompleteDetailsOfTask(int taskId)
        {
            var taskIds = new List<int>() { taskId };

            // get task from db
            var task = (await _taskRepository.GetByTaskIds(taskIds)).FirstOrDefault();

            // get all documents related to task by taskId
            var allDocuments =  await _documentRepository.GetByTaskId(taskIds);

            // get all notes related to task by taskId
            var allNotes =  await _notesRepository.GetByTaskId(taskIds);

            // get user details for the task
            var userId = 0;
            if (task!.AssignedTo != null || task.AssignedTo != 0)
            {
                var user = ((await _usersRepository.GetByUserIds(new List<int>() { (int)task.AssignedTo })));
                userId = user.First().UserId;
            }

            return  new SuperTaskView()
            {
                Task = task,
                AssignedTo = userId,
                Documents = allDocuments.ToList(),
                Notes = allNotes.ToList()
            };
        }

        public async Task<IEnumerable<DbTask>> GetOverDueTasks(DateTime date, string teamName = null)
        {
            // get all over due tasks from db by date
            var overdueTasks = await _taskRepository.GetOverDueTasksByDate(date);

            // if team name id null then return all overdue tasks
            if(teamName == null)
            {
                return overdueTasks;
            }

            // there is team name , then map team name to task
            // "teamName" is present in user table and it te mapped to task by "assignedTo" colum of task table 

            // get list of all users who has over due task
            var overdueTaskUsers = overdueTasks.Select(o => o.AssignedTo.Value).ToList();

            //get user info from users table
            var overdueUserInfo = await _usersRepository.GetByUserIds(overdueTaskUsers);

            // filter user Info by teamName
            overdueUserInfo = overdueUserInfo.Where(u => u.Team == teamName);
            var overdueIds = overdueUserInfo.Where(u => u.Team == teamName).Select(e => e.UserId).ToList();

            // filter overdue tasks by teamName
            overdueTasks = overdueTasks.Where(o => overdueIds.Contains(o.AssignedTo.Value)).ToList();

            return overdueTasks;
        }

        public async Task<Dictionary<string,List<DbTask>>> GetIncompleteTaskOfTeam(DateTime dueDate)
        {
            var tasks = await _taskRepository.GetOverDueTasksByDate(dueDate);
            var assignedTo = tasks.Select(t => t.AssignedTo.Value).ToList();

            // get users by assigned to data
            var users = (await _usersRepository.GetByUserIds(assignedTo)).ToList();
            var teamNames = users.Select(u => u.Team).ToList();

            // create new dictionary with tasks as key : teamName  and values as tasks
            var taskDict = new Dictionary<string, List<DbTask>>();

            // iterate through each task and construct the dictionary
            foreach (var task in tasks)
            {
                var userForTask = users.Where(u => u.UserId == task.AssignedTo).First();

                // if there is record in dictionary, Add to the value list
                if (taskDict.TryGetValue(userForTask.Team, out List<DbTask> value))
                {
                    value.Add(task);
                }
                // insert to dictionary
                else
                {
                    // create a new list for values and insert to the dictionary
                    var valueList = new List<DbTask>() { task };
                    taskDict.Add(userForTask.Team, valueList);
                }
            }

            return taskDict;
        }

        public async Task<Dictionary<string, List<DbTask>>> GetCompletedTaskOfTeam(DateTime dueDate)
        {
            var tasks = await _taskRepository.GetCompletedTasksByDate(dueDate);
            var assignedTo = tasks.Select(t => t.AssignedTo.Value).ToList();

            // get users by assigned to data
            var users = (await _usersRepository.GetByUserIds(assignedTo)).ToList();
            var teamNames = users.Select(u => u.Team).ToList();

            // create new dictionary with tasks as key : teamName  and values as tasks
            var taskDict = new Dictionary<string, List<DbTask>>();

            // iterate through each task and construct the dictionary
            foreach (var task in tasks)
            {
                var userForTask = users.Where(u => u.UserId == task.AssignedTo).First();

                // if there is record in dictionary, Add to the value list
                if (taskDict.TryGetValue(userForTask.Team, out List<DbTask> value))
                {
                    value.Add(task);
                }
                // insert to dictionary
                else
                {
                    // create a new list for values and insert to the dictionary
                    var valueList = new List<DbTask>() { task };
                    taskDict.Add(userForTask.Team, valueList);
                }
            }

            return taskDict;
        }

        public async Task<TeamReportStatz> GetTeamStatz(string teamName)
        {
            // get users by team
            var users = await _usersRepository.GetUserByTeam(teamName);
            var userCount = users.Count();
            
            if (users == null || userCount == 0)
            {
                throw new Exception("Team not found");
            }

            var reportDate = DateTime.Now;

            // get completed tasks
            var completedTasks = await _taskRepository.GetCompletedTasksByDate(reportDate);
            var completedTaskCount = completedTasks.Count();

            // get incomplete tasks
            var incompleteTask = await _taskRepository.GetOverDueTasksByDate(reportDate);
            var incompleteTaskCount = incompleteTask.Count();

            return new TeamReportStatz()
            {
                TeamName = teamName,
                TeamCount = userCount,
                CompletedTasks = completedTaskCount,
                IncompletedTasks = incompleteTaskCount,
                ReportTime = reportDate
            };
        }

        public async Task<UserStatz> GetUserStatz(int userId)
        {
            // get user
            var user = (await _usersRepository.GetByUserIds(new List<int>() { userId })).First();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var reportDate = DateTime.Now;

            // get completed tasks
            var completedTasks = await _taskRepository.GetCompletedTasksByDate(reportDate);
            completedTasks = completedTasks.Where(task => task.AssignedTo == userId).ToList();
            var completedTaskCount = completedTasks.Count();

            // get incomplete tasks
            var incompleteTask = await _taskRepository.GetOverDueTasksByDate(reportDate);
            incompleteTask = incompleteTask.Where(task => task.AssignedTo == userId).ToList();
            var incompleteTaskCount = incompleteTask.Count();

            return new UserStatz()
            {
                UserId = userId,
                Name = user.UserName,
                Role = user.Role,
                TeamName = user.Team,
                CompletedTasks = completedTasks.ToList(),
                CompletedTaskCount = completedTaskCount,
                IncompletedTasks = incompleteTask.ToList(),
                IncompletedTaskCount = incompleteTaskCount,
                TotalTask = completedTaskCount + incompleteTaskCount,
                Time = reportDate,
            };
        }
    }
}
