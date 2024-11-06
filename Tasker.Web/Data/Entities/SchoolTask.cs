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

        [Required]
        public DateOnly Deadline { get; set; }

        [Required, ForeignKey(nameof(TaskType))]
        public Guid TaskTypeId { get; set; }

        [ForeignKey(nameof(TaskTypeId))]
        public TaskType TaskType { get; set; } = null!;

        [Required, ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; } = null!;

        public bool Deleted { get; set; } = false;

    }
}
