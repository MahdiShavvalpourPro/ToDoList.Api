namespace ToDoList.Api.Data.Entities
{
    public class Projects : BaseEntity
    {
        public string Name { get; set; }
        public Status ProjectStatus { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string? Descrption { get; set; }


        //Relations
        public People Owner { get; set; }
        public int OwnerId { get; set; }
        public ICollection<Tasks> TasksList { get; set; }

    }
}
