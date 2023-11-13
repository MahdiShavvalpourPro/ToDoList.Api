using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;

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
            _userTasksRepository= userTasksRepository;
        }
        public Task DeleteTaskAsync(Tasks tasks)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync(int projectId)
        {
            return await _context.Tbl_Task
                .Include(t => t.UserTasks)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public Task<Tasks> GetTaskAsync(int peopleId, int projectId, int taskId)
        {
            throw new NotImplementedException();
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

        public Task<bool> TaskExistsAsync(int peopleId, int projectId, int taskId)
        {
            throw new NotImplementedException();
        }


    }
}
