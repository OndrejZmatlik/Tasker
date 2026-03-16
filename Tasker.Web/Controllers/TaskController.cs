using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Web.Data.Entities;
using Tasker.Web.Data.Services;

namespace Tasker.Web.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class TaskController
{
    private readonly TasksService tasksService;

    public TaskController(TasksService tasksService)
    {
        this.tasksService = tasksService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<SchoolTask>> GetAllCurrentAsync()
    {
        return await tasksService.GetSchoolTasksAsync();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<SchoolTask>> GetAllHistoryAsync()
    {
        return await tasksService.GetAllSchoolTasksAsync();
    }
}