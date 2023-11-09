using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Models.People
{
    public class PeopleForUpdateDto
    {
        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [ValidIranianMobileNumber]
        public string MobileNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; } = null;


        public string PersianDate { get; set; } = DateTime.Now.ToShortPersianDateString();
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsRemove { get; set; } = false;
    }
}
