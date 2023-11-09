namespace ToDoList.Api.Models.People
{
    public class PeopleForDisplayDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string? Email { get; set; } = null;
        public string PersianDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsRemove { get; set; }
    }
}
