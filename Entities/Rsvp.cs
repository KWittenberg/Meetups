namespace Meetups.Entities;

public class Rsvp : BaseEntity<Guid>
{
    public Guid EventId { get; set; }

    [JsonIgnore]
    public Event? Event { get; set; }


    public Guid UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }




    public DateTime RsvpDate { get; set; } = DateTime.UtcNow;


    [StringLength(maximumLength: 50)]
    public string Status { get; set; } = string.Empty;
}