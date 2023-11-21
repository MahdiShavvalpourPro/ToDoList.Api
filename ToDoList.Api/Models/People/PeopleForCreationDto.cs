using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Models.People
{
    public class PeopleForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
    }
}
