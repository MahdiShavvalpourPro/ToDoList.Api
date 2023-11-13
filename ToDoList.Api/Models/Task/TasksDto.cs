using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Task
{
    public class TasksDto
    {
        public string TaskName { get; set; }
        public Status TaskStatus { get; set; }
        public DateTime StartTaskTime { get; set; }
        public DateTime EndTaskTime { get; set; }
        public PriorityLevel PriorityLevelTask { get; set; }
        public string? TaskDescription { get; set; } = null;


    }
}
