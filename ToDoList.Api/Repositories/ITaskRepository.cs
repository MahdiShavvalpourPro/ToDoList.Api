using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<Tasks>> GetAllTasksAsync(int projectId);
        public Task<Tasks> GetTaskAsync(int peopleId, int projectId, int taskId);
        public Task<bool> InsertTaskAsync(int peopleId, int projectId, Tasks task);
        public Task<bool> TaskExistsAsync(int peopleId, int projectId, int taskId);
        public Task DeleteTaskAsync(Tasks tasks);


        public Task<bool> SaveChangesAsync();
    }
}
