using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasker.Web.Data.Entities;

namespace Tasker.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<SchoolTask> Tasks => Set<SchoolTask>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<TaskType> TaskTypes => Set<TaskType>();
    }
}
