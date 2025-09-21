namespace Task_Tracker_API.domain;

using System;
using System.Collections.Generic;


public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.New;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    // Автор (обязательная связь)
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;

    // Исполнитель (необязательная связь)
    public Guid? AssigneeId { get; set; }
    public User? Assignee { get; set; }

    // Дедлайн (можно DateOnly, если уже используешь DateOnly — оставь как у тебя)
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Comment> Comments { get; set; } = new();
}

public enum TaskPriority
{
    Low,     // низкий приоритет
    Medium,  // средний (по умолчанию)
    High     // высокий
}
public enum TaskStatus
{
    New,        // только создана
    InProgress, // в работе
    Done        // выполнена
}