using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Project
{
    public class ProjectForCreationDto
    {
        public string Name { get; set; }
        public Status ProjectStatus { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string? Descrption { get; set; } = null;
    }
}
