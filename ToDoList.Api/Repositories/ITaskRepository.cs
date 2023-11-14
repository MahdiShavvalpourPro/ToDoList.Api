using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Infos;

namespace ToDoList.Api.Repositories
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<TaskInfos>> GetAllTaskInfosAsync(int peopleId, int projectId);
        public Task<TaskInfos> GetTaskInfoAsync(int peopleId, int projectId, int taskId);
        public Task<Tasks> GetTaskAsync(int peopleId, int projectId, int taskId);

        public Task<bool> InsertTaskAsync(int peopleId, int projectId, Tasks task);
        public Task<bool> TaskExistsAsync(int peopleId, int projectId, int taskId);
        public Task<bool> DeleteTaskAsync(Tasks tasks);


        public Task<bool> SaveChangesAsync();
    }
}
