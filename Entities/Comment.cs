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
    [JsonIgnore]
    public User User { get; set; } = default!;

    [StringLength(maximumLength: 50)]
    public string UserName { get; set; } = string.Empty;

    public Guid EventId { get; set; }
    [JsonIgnore]
    public Event Event { get; set; } = default!;

    [StringLength(maximumLength: 1000)]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}