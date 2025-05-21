namespace Meetups.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? MeetupLink { get; set; }

    public string? Category { get; set; }

    public int Capacity { get; set; }


    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }

    public DateOnly EndDate { get; set; }
    public TimeOnly EndTime { get; set; }


    public bool AllDay { get; set; }

    public Guid? OrganizerId { get; set; }
}