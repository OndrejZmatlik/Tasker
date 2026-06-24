using Microsoft.EntityFrameworkCore;
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
            return await dbContext.Tasks
                .Where(x => x.DeadlineTo.ToDateTime(TimeOnly.MinValue) >= DateTime.Now.Date && !x.Deleted)
                .Include(x => x.TaskType)
                .Include(x => x.Subjects).ThenInclude(s => s.Group)
                .OrderBy(x => x.DeadlineTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<SchoolTask>> GetSchoolTasksAsync(DateOnly dateFilter)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks
                .Where(x => !x.Deleted && (
                    x.DeadlineFrom == null
                        ? x.DeadlineTo == dateFilter
                        : x.DeadlineFrom <= dateFilter && x.DeadlineTo >= dateFilter))
                .Include(x => x.TaskType)
                .Include(x => x.Subjects).ThenInclude(s => s.Group)
                .OrderBy(x => x.DeadlineTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<SchoolTask>> GetAllSchoolTasksAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks
                .Include(x => x.TaskType)
                .Include(x => x.Subjects).ThenInclude(s => s.Group)
                .OrderByDescending(x => x.DeadlineTo)
                .ToListAsync();
        }

        public async Task<SchoolTask?> GetSchoolTaskAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Tasks
                .Include(x => x.TaskType)
                .Include(x => x.Subjects).ThenInclude(s => s.Group)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Subjects
                .Include(x => x.Group)
                .OrderBy(x => x.ShortName).ThenBy(x => x.Group.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Groups.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<TaskType>> GetAssignmentTypesAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.TaskTypes.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task DeleteGroupAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Groups.FindAsync(id);
            if (result is not null)
            {
                dbContext.Groups.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAssignmentTypeAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TaskTypes.FindAsync(id);
            if (result is not null)
            {
                dbContext.TaskTypes.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteSubjectAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.Subjects.FindAsync(id);
            if (result is not null)
            {
                dbContext.Subjects.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddSubjectAsync(string shortName, string fullName, Guid groupId = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Subjects.AddAsync(new Subject { ShortName = shortName, FullName = fullName, GroupId = groupId });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddGroupAsync(string name)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Groups.AddAsync(new Group { Name = name });
            await dbContext.SaveChangesAsync();
        }

        public async Task AddAssignmentTypeAsync(string name)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.TaskTypes.AddAsync(new TaskType { Name = name });
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

        public async Task AddTaskAsync(SchoolTask task, IEnumerable<Guid> subjectIds)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            task.Subjects = await dbContext.Subjects
                .Where(s => subjectIds.Contains(s.Id))
                .ToListAsync();
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var res = await dbContext.Tasks.FindAsync(id);
            if (res is not null)
            {
                res.Deleted = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RestoreTaskAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var res = await dbContext.Tasks.FindAsync(id);
            if (res is not null)
            {
                res.Deleted = false;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateSchoolTaskAsync(SchoolTask schoolTask, IEnumerable<Guid> subjectIds)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var existing = await dbContext.Tasks
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == schoolTask.Id);
            if (existing is null) return;

            existing.Name = schoolTask.Name;
            existing.Description = schoolTask.Description;
            existing.DeadlineFrom = schoolTask.DeadlineFrom;
            existing.DeadlineTo = schoolTask.DeadlineTo;
            existing.TaskTypeId = schoolTask.TaskTypeId;
            existing.Priority = schoolTask.Priority;
            existing.Deleted = schoolTask.Deleted;

            var newSubjects = await dbContext.Subjects
                .Where(s => subjectIds.Contains(s.Id))
                .ToListAsync();
            existing.Subjects.Clear();
            foreach (var s in newSubjects)
                existing.Subjects.Add(s);

            await dbContext.SaveChangesAsync();
        }
    }
}
