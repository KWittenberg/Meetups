namespace Meetups.DTOs;

public class CommentDto
{
    public Guid Id { get; set; } = Guid.Empty;


    public Guid UserId { get; set; } = Guid.Empty;
    public User User { get; set; } = new();


    public Guid EventId { get; set; } = Guid.Empty;
    public Event Event { get; set; } = new();


    public string Message { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}