using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Task
{
    public class TaskForCreationDto
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public Status TaskStatus { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public PriorityLevel PriorityLevel { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
    }
}
