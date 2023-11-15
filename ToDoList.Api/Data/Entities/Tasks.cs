namespace ToDoList.Api.Data.Entities
{
    public class Tasks : BaseEntity
    {
        public string Name { get; set; }
        public Status TaskStatus { get; set; }
        public DateTime? StartTime { get; set; } = null;
        public DateTime? ExpiteTime { get; set; } = null;
        public bool Expireded { get; set; }
        public bool IsCompeled => Status.Done == 0;
        public PriorityLevel PriorityLevel { get; set; }
        public string? Description { get; set; }


        //Relations
        public Projects Project { get; set; }
        public int ProjectId { get; set; }

        public ICollection<UserTasks> UserTasks { get; set; }
    }
}
