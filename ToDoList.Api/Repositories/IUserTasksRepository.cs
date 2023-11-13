using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public interface IUserTasksRepository
    {
        public Task InsertToUserTask(UserTasks userTasks);
    }
}
