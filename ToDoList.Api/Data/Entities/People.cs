namespace ToDoList.Api.Data.Entities
{
    public class People : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;

        //Relations
        public ICollection<Projects> ProjectsList { get; set; } = new List<Projects>();
        public ICollection<UserTasks> UserTasks { get; set; } = new List<UserTasks>();
        public ICollection<Tasks> TasksList { get; set; } = new List<Tasks>();
    }
}