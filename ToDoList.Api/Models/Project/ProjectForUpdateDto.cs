using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Project
{
    public class ProjectForUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Status ProjectStatus { get; set; }

        [Required]
        public PriorityLevel PriorityLevel { get; set; }

        [MaxLength(400)]
        public string? Descrption { get; set; } = null;
    }
}