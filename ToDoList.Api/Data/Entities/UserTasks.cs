namespace ToDoList.Api.Data.Entities
{
    public class UserTasks : BaseEntity
    {
        public int ?UserId { get; set; }
        public People ?People { get; set; }
        public int ?TaskId { get; set; }
        public Tasks?  Task{ get; set; }
    }
}
