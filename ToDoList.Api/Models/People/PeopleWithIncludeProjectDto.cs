using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Project;

namespace ToDoList.Api.Models.People
{
    public class PeopleWithIncludeProjectDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
        public bool IsRemove { get; set; }
        public int NumberOfProjects
        {
            get
            {
                return ProjectsList.Count;
            }
        }

        public ICollection<ProjectsDto> ProjectsList { get; set; } = new List<ProjectsDto>();
    }
}
