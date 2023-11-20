namespace ToDoList.Api.Data.Entities
{
    public class Tasks : BaseEntity
    {
        public string Name { get; set; }
        public Status TaskStatus { get; set; }
        public DateTime? StartTime { get; set; } = null;
        public DateTime? ExpireTime { get; set; } = null;
        public PriorityLevel PriorityLevel { get; set; }
        public string? Description { get; set; }


        //Relations
        public Projects Project { get; set; }
        public int ProjectId { get; set; }

        public ICollection<UserTasks> UserTasks { get; set; }
    }
}
