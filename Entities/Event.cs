namespace Meetups.Entities;

public class Event : BaseAuditableEntity<Guid>
{
    public Event() { }

    public Event(string? imageUrl, string title, string? description, string? location, string? meetupLink, string? category, int capacity,
                DateTime start, DateTime end, bool allDay)
    {
        ImageUrl = imageUrl;
        Title = title;
        Description = description;
        Location = location;
        MeetupLink = meetupLink;
        Category = category;
        Capacity = capacity;
        Start = start;
        End = end;
        AllDay = allDay;
    }

    [StringLength(maximumLength: 200)]
    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(maximumLength: 100)]
    public string Title { get; set; } = string.Empty;


    [StringLength(maximumLength: 4000)]
    public string? Description { get; set; }


    [StringLength(maximumLength: 200)]
    public string? Location { get; set; }


    [StringLength(maximumLength: 200)]
    public string? MeetupLink { get; set; }


    [StringLength(maximumLength: 100)]
    public string? Category { get; set; }


    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }


    public DateTime Start { get; set; }

    public DateTime End { get; set; }


    public bool AllDay { get; set; }

    public Recurrence Recurrence { get; set; } = Recurrence.OneTime;

    public Guid? OrganizerId { get; set; }



    public List<Rsvp> Rsvps { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
}