namespace Meetups.Entities;

public class Event : BaseAuditableEntity<Guid>
{
    public Event() { }

    public Event(string title, string? description, string? location, string? meetupLink, string? category, int capacity,
                DateOnly startDate, TimeOnly startTime, DateOnly endDate, TimeOnly endTime, bool allDay)
    {
        Title = title;
        Description = description;
        Location = location;
        MeetupLink = meetupLink;
        Category = category;
        Capacity = capacity;
        StartDate = startDate;
        StartTime = startTime;
        EndDate = endDate;
        EndTime = endTime;
        AllDay = allDay;
    }


    [Required]
    [StringLength(maximumLength: 100)]
    public string Title { get; set; } = string.Empty;


    [StringLength(maximumLength: 500)]
    public string? Description { get; set; }


    [StringLength(maximumLength: 200)]
    public string? Location { get; set; }


    [StringLength(maximumLength: 200)]
    public string? MeetupLink { get; set; }


    [StringLength(maximumLength: 100)]
    public string? Category { get; set; }


    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }


    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }

    public DateOnly EndDate { get; set; }
    public TimeOnly EndTime { get; set; }


    public bool AllDay { get; set; }

    public Guid? OrganizerId { get; set; }
}