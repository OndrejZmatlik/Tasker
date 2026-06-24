using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Web.Data.Entities
{
    public class SchoolTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

        public DateOnly? DeadlineFrom { get; set; }

        [Required]
        public DateOnly DeadlineTo { get; set; }

        [Required, ForeignKey(nameof(TaskType))]
        public Guid TaskTypeId { get; set; }

        [ForeignKey(nameof(TaskTypeId))]
        public TaskType TaskType { get; set; } = null!;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public bool Deleted { get; set; } = false;
<<<<<<< Updated upstream
        public bool Important { get; set; } = false;
=======

        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        [NotMapped]
        public bool Important => Priority >= TaskPriority.High;
>>>>>>> Stashed changes

        [NotMapped]
        public string SubjectsDisplay =>
            Subjects.Any()
                ? string.Join(", ", Subjects.Select(s => s.TitleShort))
                : "—";
    }
}
