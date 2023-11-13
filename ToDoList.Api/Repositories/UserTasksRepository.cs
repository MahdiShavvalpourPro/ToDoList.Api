using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public class UserTasksRepository : IUserTasksRepository
    {
        private readonly ApplicationDbContext _context;

        public UserTasksRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task InsertToUserTask(UserTasks userTasks)
        {
            await _context.Tbl_UserTask
                .AddAsync(userTasks);
        }
    }
}
