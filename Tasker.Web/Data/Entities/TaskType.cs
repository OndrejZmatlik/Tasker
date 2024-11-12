using System.ComponentModel.DataAnnotations;

namespace Tasker.Web.Data.Entities
{
    public class TaskType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        override public string ToString() => Name;
    }
}
