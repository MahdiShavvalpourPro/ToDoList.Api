using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Infos;

namespace ToDoList.Api.Repositories
{
    public interface IUserTasksRepository
    {
        public Task InsertToUserTask(UserTasks userTasks);
        //public Task<UserTasks> GetUserTasks(/*UserTasks userTasks*/);
    }
}
