namespace ToDoList.Api.Data.Entities
{
    public class People : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
        public int TaskId { get; set; }
        public Tasks Task { get; set; }

        //Relations
        public ICollection<Projects> ProjectsList { get; set; } = new List<Projects>();
        public ICollection<UserTasks> UserTasks { get; set; } = new List<UserTasks>();
    }
}