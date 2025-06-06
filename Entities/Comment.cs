namespace Meetups.Entities;

public class Comment : BaseEntity<Guid>
{
    public Comment(Guid userId, string userName, Guid eventId, string message)
    {
        UserId = userId;
        UserName = userName;
        EventId = eventId;
        Message = message;
    }


    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string UserName { get; set; } = string.Empty;

    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;


    public string Message { get; set; } = string.Empty;

    public bool IsApproved { get; set; }


    public DateTime CreatedUtc { get; set; }
}