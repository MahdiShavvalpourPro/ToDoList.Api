using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public interface IPeopleRepository
    {
        public Task<People?> GetPeopleByIdAsync(int peopleId, bool includeProjects = false, bool includeTasks = false);
        public Task<IEnumerable<People>> GetPeopleAsync();
        public Task<bool> PeopleExistsAsync(int peopleId);
        public Task AddPeopleAsync(People people);
        public Task AddManyPeople(IEnumerable<People> people);
        public Task UpdatePeopleAsync(int peopleId,People people);
        public Task<bool> DeletePeopleAsync(int peopleId);


        public Task<bool> SaveChangesAsync();
    }
}
