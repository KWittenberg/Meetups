namespace Meetups.DTOs;

public class CommentInput
{
    public Guid UserId { get; set; } = Guid.Empty;

    public Guid EventId { get; set; } = Guid.Empty;

    public string Message { get; set; } = string.Empty;
}