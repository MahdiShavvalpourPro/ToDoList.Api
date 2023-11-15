using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Task
{
    public class TaskForCreationDto
    {
        public string Name { get; set; }
        public Status TaskStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireDate { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string? Description { get; set; }
    }
}
