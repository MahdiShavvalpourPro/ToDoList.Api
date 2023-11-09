using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.People
{
    public class PeopleDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;

        public int NumberOfTasks
        {
            get
            {
                return UserTasks.Count;
            }
        }

        public int NumberOfProjects
        {
            get
            {
                return Projects.Count;
            }
        }

        public ICollection<UserTasks> UserTasks { get; set; } = new List<UserTasks>();
        public ICollection<Projects> Projects { get; set; } = new List<Projects>();
    }
}
