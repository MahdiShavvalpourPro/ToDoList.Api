using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Models.People
{
    public class PeopleForDeleteDto
    {
        [Required]
        public bool IsRemove { get; set; }
    }
}