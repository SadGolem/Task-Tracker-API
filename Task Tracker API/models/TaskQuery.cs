using Task_Tracker_API.domain;

namespace Task_Tracker_API.models;

public class TaskQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Q { get; init; }
    public string? Sort { get; init; }
    public Task_Tracker_API.domain.TaskStatus? Status { get; init; }
    public Guid? AssigneeId { get; init; }
}
public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    string Status,
    string Priority,
    Guid AuthorId,
    Guid? AssigneeId,
    DateOnly? DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int CommentsCount);


public record CreateTaskRequest(
    string Title,
    string? Description,
    TaskPriority? Priority,
    Guid? AssigneeId,
    DateOnly? DueDate);


public record UpdateTaskRequest(
    string? Title,
    string? Description,
    TaskPriority? Priority,
    domain.TaskStatus? Status,
    bool DueDateSet,
    DateOnly? DueDate);


public record ChangeStatusRequest(domain.TaskStatus Status);


public record CreateCommentRequest(string Body);