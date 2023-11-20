using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public interface IProjectRepository
    {
        public Task<IEnumerable<Projects?>> GetProjectsAsync(int peopleId);
        public Task<Projects?> GetProjectAsync(int peopleId, int projectId);
        public Task<Projects?> GetProjectAsync(int projectId);
        public Task AddProjectAsync(int peopleId, Projects project);
        public Task DeleteProjectsAsync(int peopleId);
        public void DeleteProjectAsync(Projects projects);

        //public Task UpdateProjectAsync(int peopleId,int projectId,Projects project);
        //public Task<bool> PeopleExistsAsync(int peopleId);
        public Task<bool> ProjectExistsAsync(int peopleId, int projectId);

        public void ChangeProjectStatus(Projects project, Status status);


        public Task<bool> SaveChangesAsync();
    }
}
