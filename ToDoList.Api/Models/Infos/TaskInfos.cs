using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Infos
{
    public class TaskInfos
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PersianDate { get; set; }

        public string ProjectName { get; set; }
        public Status ProjectStatus { get; set; }
        public PriorityLevel ProjectPriorityLevel { get; set; }
        public bool Compelation { get; set; }

        public string TaskName { get; set; }
        public Status TaskStatus { get; set; }
        public PriorityLevel TaskPriorityLevel { get; set; }
        public bool IsExpired { get; set; }
    }
}
