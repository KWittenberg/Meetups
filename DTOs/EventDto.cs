namespace Meetups.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    public string? ImageUrl { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? MeetupLink { get; set; }

    public string? Category { get; set; }

    public int Capacity { get; set; }


    public DateTime Start { get; set; }

    public DateTime End { get; set; }


    public bool AllDay { get; set; }

    public Guid? OrganizerId { get; set; }
}