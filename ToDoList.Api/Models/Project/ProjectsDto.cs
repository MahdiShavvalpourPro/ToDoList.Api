using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Models.Project
{
    public class ProjectsDto
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public Status ProjectStatus { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public string? Descrption { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public string PersianDate { get; set; }


    }
}