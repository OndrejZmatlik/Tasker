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
            return await dbContext.Tasks.Where(x => x.Deadline.ToDateTime(TimeOnly.MinValue) >= DateTime.Now.Date && !x.Deleted)
                                        .Include(x => x.TaskType)
                                        .Include(x => x.Subject)
                                        .ThenInclude(x => x.Group)
                                        .OrderBy(x => x.Deadline)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<SchoolTask>> GetSchoolTasksAsync(DateOnly DateFilter)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks.Where(x => x.Deadline == DateFilter && !x.Deleted)
                                        .Include(x => x.TaskType)
                                        .Include(x => x.Subject)
                                        .ThenInclude(x => x.Group)
                                        .OrderBy(x => x.Deadline)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<SchoolTask>> GetAllSchoolTasksAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks.Include(x => x.TaskType)
                                        .Include(x => x.Subject)
                                        .ThenInclude(x => x.Group)
                                        .OrderByDescending(x => x.Deadline)
                                        .ToListAsync();
        }
        public async Task<SchoolTask?> GetSchoolTaskAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks.Include(x => x.TaskType)
                                        .Include(x => x.Subject)
                                        .ThenInclude(x => x.Group)
                                        .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Subjects.Include(x => x.Group).OrderBy(x => x.ShortName).ThenBy(x => x.Group.Name).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Groups.ToListAsync();
        }

        public async Task<IEnumerable<TaskType>> GetAssignmentTypesAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TaskTypes.OrderBy(x => x.Name).ToListAsync();
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
            await dbContext.Subjects.AddAsync(new Subject { ShortName = "Z - New Subject" });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddGroupAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Groups.AddAsync(new Group { Name = "Z - New Group" });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddAssignmentTypeAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.TaskTypes.AddAsync(new TaskType { Name = "Z - New Task Type" });
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subjects.FindAsync(subject.Id);
            if (result is not null)
            {
                result.ShortName = subject.ShortName;
                result.FullName = subject.FullName;
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
            var res = await dbContext.Tasks.FindAsync(Id);
            if (res is not null)
            {
                res.Deleted = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RestoreTaskAsync(Guid Id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var res = await dbContext.Tasks.FindAsync(Id);
            if (res is not null)
            {
                res.Deleted = false;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateSchoolTaskAsync(SchoolTask schoolTask)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            dbContext.Tasks.Update(schoolTask);
            await dbContext.SaveChangesAsync();
        }
    }
}
