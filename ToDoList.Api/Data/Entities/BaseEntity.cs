using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Data.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
            PersianDate = DateTime.Now.ToShortPersianDateString();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        [StringLength(10)]
        public string PersianDate { get; set; }
        public bool IsRemove { get; set; }
    }
}