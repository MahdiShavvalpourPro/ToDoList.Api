using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.People
{
    public class PeopleWithIncludeTaskDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
        public bool IsRemove { get; set; }
        public int NumberOfTasks
        {
            get
            {
                return UserTasks.Count;
            }
        }
        public ICollection<TasksDto> UserTasks { get; set; } = new List<TasksDto>();
    }
}