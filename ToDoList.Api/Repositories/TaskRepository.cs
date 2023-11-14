using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Infos;

namespace ToDoList.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<TaskRepository> _logger;
        private readonly IUserTasksRepository _userTasksRepository;

        public TaskRepository(
            ApplicationDbContext context,
            IProjectRepository projectRepository,
            ILogger<TaskRepository> logger,
            IUserTasksRepository userTasksRepository
            )
        {
            _context = context;
            _projectRepository = projectRepository;
            _logger = logger;
            _userTasksRepository = userTasksRepository;
        }
        public async Task<bool> DeleteTaskAsync(Tasks tasks)
        {
            try
            {
                _context.Remove(tasks);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return false;
            }
        }

        public async Task<IEnumerable<TaskInfos>> GetAllTaskInfosAsync(int peopleId, int projectId)
        {
            return await _context.Tbl_Task
                  .Join(
                     _context.Tbl_Project,
                     task => task.ProjectId,
                     project => project.Id,
                     (task, project) => new { Task = task, Project = project })
                 .Join(
                     _context.Tbl_UserTask,
                     taskProject => taskProject.Task.Id,
                     userTask => userTask.TaskId,
                     (taskProject, userTask) => new { taskProject.Task, taskProject.Project, UserTask = userTask })
                 .Join(
                     _context.Tbl_People,
                     taskProjectUserTask => taskProjectUserTask.UserTask.UserId,
                     people => people.Id,
                     (taskProjectUserTask, people) => new { taskProjectUserTask.Task, taskProjectUserTask.Project, taskProjectUserTask.UserTask, People = people })
                 .Where(joined => joined.People.Id == peopleId && joined.Project.Id == projectId)
                 .Select(x => new TaskInfos
                 {
                     FirstName = x.People.FirstName,
                     LastName = x.People.LastName,
                     MobileNumber = x.People.MobileNumber,
                     PersianDate = x.People.PersianDate,
                     ProjectName = x.Project.Name,
                     ProjectStatus = x.Project.ProjectStatus,
                     ProjectPriorityLevel = x.Project.PriorityLevel,
                     TaskName = x.Task.Name,
                     TaskStatus = x.Task.TaskStatus,
                     TaskPriorityLevel = x.Task.PriorityLevel
                 })
                 .ToListAsync();
        }

        public async Task<Tasks> GetTaskAsync(int peopleId, int projectId, int taskId)
        {
            var taskInfo = await _context.Tbl_Task
                .Join(
                   _context.Tbl_Project,
                   task => task.ProjectId,
                   project => project.Id,
                   (task, project) => new { Task = task, Project = project })
               .Join(
                   _context.Tbl_UserTask,
                   taskProject => taskProject.Task.Id,
                   userTask => userTask.TaskId,
                   (taskProject, userTask) => new { taskProject.Task, taskProject.Project, UserTask = userTask })
               .Join(
                   _context.Tbl_People,
                   taskProjectUserTask => taskProjectUserTask.UserTask.UserId,
                   people => people.Id,
                   (taskProjectUserTask, people) => new { taskProjectUserTask.Task, taskProjectUserTask.Project, taskProjectUserTask.UserTask, People = people })
               .FirstOrDefaultAsync(joined => joined.People.Id == peopleId && joined.Project.Id == projectId && joined.Task.Id == taskId);

            return new Tasks()
            {
                Id = taskInfo!.Task.Id,
                Name = taskInfo.Task.Name,
                TaskStatus = taskInfo.Task.TaskStatus,
                StartTime = taskInfo.Task.StartTime,
                EndTime = taskInfo.Task.EndTime,
                PriorityLevel = taskInfo.Task.PriorityLevel,
                Description = taskInfo.Task.Description,
                ProjectId = taskInfo.Task.ProjectId,
                CreationDate = taskInfo.Task.CreationDate,
                ModificationDate = taskInfo.Task.ModificationDate,
                PersianDate = taskInfo.Task.PersianDate,
                IsRemove = taskInfo.Task.IsRemove
            };
        }

        public async Task<TaskInfos> GetTaskInfoAsync(int peopleId, int projectId, int taskId)
        {
            var taskInfo = await _context.Tbl_Task
                 .Join(
                    _context.Tbl_Project,
                    task => task.ProjectId,
                    project => project.Id,
                    (task, project) => new { Task = task, Project = project })
                .Join(
                    _context.Tbl_UserTask,
                    taskProject => taskProject.Task.Id,
                    userTask => userTask.TaskId,
                    (taskProject, userTask) => new { taskProject.Task, taskProject.Project, UserTask = userTask })
                .Join(
                    _context.Tbl_People,
                    taskProjectUserTask => taskProjectUserTask.UserTask.UserId,
                    people => people.Id,
                    (taskProjectUserTask, people) => new { taskProjectUserTask.Task, taskProjectUserTask.Project, taskProjectUserTask.UserTask, People = people })
                .FirstOrDefaultAsync(joined => joined.People.Id == peopleId && joined.Project.Id == projectId && joined.Task.Id == taskId);

            return new TaskInfos()
            {
                FirstName = taskInfo!.People.FirstName,
                LastName = taskInfo.People.LastName,
                MobileNumber = taskInfo.People.MobileNumber,
                PersianDate = taskInfo.People.PersianDate,
                ProjectName = taskInfo.Project.Name,
                ProjectStatus = taskInfo.Project.ProjectStatus,
                ProjectPriorityLevel = taskInfo.Project.PriorityLevel,
                TaskName = taskInfo.Task.Name,
                TaskStatus = taskInfo.Task.TaskStatus,
                TaskPriorityLevel = taskInfo.Task.PriorityLevel
            };
        }

        public async Task<bool> InsertTaskAsync(int peopleId, int projectId, Tasks task)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Save Data Into Tbl Tasks
                    task.ProjectId = projectId;
                    await _context.Tbl_Task
                        .AddAsync(task);
                    await SaveChangesAsync();


                    //Save Data Into Tbl UserTasks
                    var userTasks = new UserTasks()
                    {
                        UserId = peopleId,
                        TaskId = task.Id,

                    };
                    await _userTasksRepository.InsertToUserTask(userTasks);
                    await SaveChangesAsync();


                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TaskExistsAsync(int peopleId, int projectId, int taskId)
        {
            var data = await _context.Tbl_Task
                 .Join(
                    _context.Tbl_Project,
                    task => task.ProjectId,
                    project => project.Id,
                    (task, project) => new { Task = task, Project = project })
                .Join(
                    _context.Tbl_UserTask,
                    taskProject => taskProject.Task.Id,
                    userTask => userTask.TaskId,
                    (taskProject, userTask) => new { taskProject.Task, taskProject.Project, UserTask = userTask })
                .Join(
                    _context.Tbl_People,
                    taskProjectUserTask => taskProjectUserTask.UserTask.UserId,
                    people => people.Id,
                    (taskProjectUserTask, people) => new { taskProjectUserTask.Task, taskProjectUserTask.Project, taskProjectUserTask.UserTask, People = people })
                .FirstOrDefaultAsync(joined => joined.People.Id == peopleId && joined.Project.Id == projectId && joined.Task.Id == taskId);
            if (data is null)
                return false;
            return true;
        }
    }
}
