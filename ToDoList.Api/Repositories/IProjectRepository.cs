using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public interface IProjectRepository
    {
        public Task<IEnumerable<Projects?>> GetProjectsAsync(int peopleId);
        public Task<Projects?> GetProjectAsync(int peopleId,int projectId);
        public Task AddProjectAsync(int peopleId,Projects project);
        public Task DeleteProjectsAsync(int peopleId);
        public Task DeleteProjectAsync(int peopleId,int projectId);
        //public Task UpdateProjectAsync(int peopleId,int projectId,Projects project);
        public Task<bool> PeopleExists(int peopleId);


        public Task<bool> SaveChangesAsync();
    }
}
