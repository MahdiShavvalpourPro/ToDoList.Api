using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Infos;

namespace ToDoList.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskRepository> _logger;
        private readonly IUserTasksRepository _userTasksRepository;
        private readonly IMapper _mapper;

        public TaskRepository(
            ApplicationDbContext context,
            ILogger<TaskRepository> logger,
            IUserTasksRepository userTasksRepository,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _userTasksRepository = userTasksRepository;
            _mapper = mapper;
        }

        public async void CheckExpiredTasks(List<Tasks> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.ExpireTime < DateTime.Now)
                {
                    task.TaskStatus = Status.Canceled;
                }
            }
            await SaveChangesAsync();
        }

        public async Task<bool> CheckStatusTasks(int ownerId, int projectId)
        {
            var tasks = await GetAllTaskInfosAsync(ownerId, projectId);
            foreach (var task in tasks)
            {
                if (task.TaskStatus != Status.Done)
                {
                    return false;
                }
            }
            return true;
        }

        public void DeleteTaskAsync(Tasks tasks)
        {
            try
            {
                _context.Remove(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }
        }

        public async Task<List<TaskInfos>> GetAllTaskInfosAsync(int peopleId, int projectId)
        {
            var tasks = await _context.Tbl_Task
               .Include(u => u.UserTasks)
               .Include(p => p.Project)
               .ThenInclude(o => o.Owner)
               .Where(t=>t.TaskStatus != Status.Canceled)
               .Where(o => o.Project.OwnerId == peopleId)
               .Where(p => p.ProjectId == projectId)
               .ToListAsync();

            CheckExpiredTasks(tasks);

            var taskInfos = _mapper.Map<List<TaskInfos>>(tasks);
            return taskInfos;
        }

        public async Task<Tasks> GetTaskAsync(int peopleId, int projectId, int taskId)
        {
            var taskInfo = await _context.Tbl_Task
              .Include(u => u.UserTasks)
              .Include(p => p.Project)
              .ThenInclude(o => o.Owner)
              .Where(o => o.Project.OwnerId == peopleId)
              .Where(p => p.ProjectId == projectId)
              .FirstOrDefaultAsync(u => u.Id == taskId);

            return taskInfo;
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
                TaskPriorityLevel = taskInfo.Task.PriorityLevel,
                ExpireTime = taskInfo.Task.ExpireTime,
                StartTime = taskInfo.Task.StartTime
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
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> TaskExistsAsync(int peopleId, int projectId, int taskId)
        {
            var taskInfo = await _context.Tbl_Task
               //.Include(u => u.UserTasks)
               //.Include(p => p.Project)
               //.ThenInclude(o => o.Owner)
               .Where(o => o.Project.OwnerId == peopleId)
               .Where(p => p.ProjectId == projectId)
               .AnyAsync(u => u.Id == taskId);

            return taskInfo;
        }

        public async void UpdateProperty(int id, Expression<Func<Tasks, string>> propertyExpression, string newValue)
        {
            var entity = _context.Tbl_Task.Find(id);

            if (entity != null)
            {
                var memberExpression = (MemberExpression)propertyExpression.Body;
                var propertyInfo = (PropertyInfo)memberExpression.Member;

                propertyInfo.SetValue(entity, newValue);

                await _context.SaveChangesAsync();
            }
        }
    }
}
