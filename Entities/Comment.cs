namespace Meetups.Entities;

public class Comment : BaseEntity<Guid>
{
    public Comment(Guid userId, Guid eventId, string message)
    {
        UserId = userId;
        EventId = eventId;
        Message = message;
    }


    public Guid UserId { get; set; } = Guid.Empty;
    public User User { get; set; } = new();


    public Guid EventId { get; set; } = Guid.Empty;
    public Event Event { get; set; } = new();



    [Required]
    [StringLength(maximumLength: 1000)]
    public string Message { get; set; } = string.Empty;


    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}