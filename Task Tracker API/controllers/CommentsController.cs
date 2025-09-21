using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Tracker_API.domain;
using TaskTracker.Infrastructure;

namespace Task_Tracker_API.controllers;

[ApiController]
[Route("api/tasks/{taskId}/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _db;
    public CommentsController(AppDbContext db) => _db = db;


    [HttpPost]
    public async Task<IActionResult> Add(Guid taskId, [FromBody] CreateCommentDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (!await _db.Tasks.AnyAsync(t => t.Id == taskId)) return NotFound();


        var comment = new Comment { TaskId = taskId, AuthorId = userId, Body = dto.Body };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return Created($"/api/tasks/{taskId}/comments/{comment.Id}", comment);
    }


    [HttpGet]
    public async Task<IActionResult> Get(Guid taskId)
    {
        if (!await _db.Tasks.AnyAsync(t => t.Id == taskId)) return NotFound();
        var list = await _db.Comments.Where(c => c.TaskId == taskId).OrderBy(c => c.CreatedAt).ToListAsync();
        return Ok(list);
    }
}


public record CreateCommentDto(string Body);