namespace Task_Tracker_API.domain;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaskId { get; set; }
    public Guid AuthorId { get; set; }
    public string Body { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public TaskItem? Task { get; set; }
    public User? Author { get; set; }
}