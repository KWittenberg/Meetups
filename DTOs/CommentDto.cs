namespace Meetups.DTOs;

public class CommentDto
{
    public Guid Id { get; set; } = Guid.Empty;

    public Guid UserId { get; set; } = Guid.Empty;

    public string UserName { get; set; } = string.Empty;

    public Guid EventId { get; set; } = Guid.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}