using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Tracker_API.domain;
using TaskTracker.Infrastructure;

namespace Task_Tracker_API.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) => _db = db;


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            AuthorId = userId
        };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return Created($"/api/tasks/{task.Id}", task);
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] domain.TaskStatus? status, [FromQuery] Guid? assigneeId)
    {
        var q = _db.Tasks.AsQueryable();
        if (status.HasValue) q = q.Where(t => t.Status == status);
        if (assigneeId.HasValue) q = q.Where(t => t.AssigneeId == assigneeId);


        var list = await q.OrderByDescending(t => t.CreatedAt).ToListAsync();
        return Ok(list);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var task = await _db.Tasks.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);
        return task is null ? NotFound() : Ok(task);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return NotFound();


        task.Title = dto.Title ?? task.Title;
        task.Description = dto.Description ?? task.Description;
        task.Status = dto.Status ?? task.Status;
        task.Priority = dto.Priority ?? task.Priority;
        task.UpdatedAt = DateTime.UtcNow;


        await _db.SaveChangesAsync();
        return Ok(task);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return NotFound();
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


public record CreateTaskDto(string Title, string? Description, TaskPriority Priority, DateTime? DueDate);
public record UpdateTaskDto(string? Title, string? Description, domain.TaskStatus? Status, TaskPriority? Priority);