using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Tasker.Web.Data.Entities;

namespace Tasker.Web.Data.Services
{
    public class TasksService
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public TasksService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<SchoolTask>> GetSchoolTasksAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks.Where(x => x.Deadline.Date > DateTimeOffset.UtcNow.Date.AddDays(-2))
                                        .Include(x => x.TaskType)
                                        .Include(x => x.Subject)
                                        .ThenInclude(x => x.Group)
                                        .OrderBy(x => x.Deadline)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Subjects.Include(x => x.Group).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Groups.ToListAsync();
        }

        public async Task<IEnumerable<TaskType>> GetAssignmentTypesAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TaskTypes.ToListAsync();
        }

        public async Task DeleteGroupAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Groups.FindAsync(Id);
            if (result is not null)
            {
                dbContext.Groups.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAssignmentTypeAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TaskTypes.FindAsync(Id);
            if (result is not null)
            {
                dbContext.TaskTypes.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteSubjectAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subjects.FindAsync(Id);
            if (result is not null)
            {
                dbContext.Subjects.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddSubjectAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Subjects.AddAsync(new Subject { Name = "New Subject" });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddGroupAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Groups.AddAsync(new Group { Name = "New Group" });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddAssignmentTypeAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.TaskTypes.AddAsync(new TaskType { Name = "New Task Type" });
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subjects.FindAsync(subject.Id);
            if (result is not null)
            {
                result.Name = subject.Name;
                result.GroupId = subject.GroupId;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAssignmentTypeAsync(TaskType taskType)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TaskTypes.FindAsync(taskType.Id);
            if (result is not null)
            {
                result.Name = taskType.Name;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupAsync(Group group)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Groups.FindAsync(group.Id);
            if (result is not null)
            {
                result.Name = group.Name;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddTaskAsync(SchoolTask task)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Tasks.Where(x => x.Id == Id).ExecuteDeleteAsync();
        }
    }
}
