using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasker.Web.Data.Entities;

namespace Tasker.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options), IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<SchoolTask> Tasks => Set<SchoolTask>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<TaskType> TaskTypes => Set<TaskType>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SchoolTask>()
                .HasMany(t => t.Subjects)
                .WithMany(s => s.Tasks)
                .UsingEntity("TaskSubjects",
                    l => l.HasOne(typeof(Subject)).WithMany().HasForeignKey("SubjectId").HasPrincipalKey(nameof(Subject.Id)),
                    r => r.HasOne(typeof(SchoolTask)).WithMany().HasForeignKey("TaskId").HasPrincipalKey(nameof(SchoolTask.Id))
                );
        }
    }
}
