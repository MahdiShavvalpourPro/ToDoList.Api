using ToDoList.Api.Models.Project;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Models.People
{
    public class PeopleWithIncludeProjectAndTaskDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
        public int NumberOfTasks
        {
            get
            {
                return TasksList.Count;
            }
        }

        public int NumberOfProjects
        {
            get
            {
                return ProjectsList.Count;
            }
        }

        public ICollection<TasksDto> TasksList { get; set; } = new List<TasksDto>();
        public ICollection<ProjectsDto> ProjectsList { get; set; } = new List<ProjectsDto>();
    }
}
