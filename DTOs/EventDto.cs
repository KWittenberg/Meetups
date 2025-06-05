namespace Meetups.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserDto User { get; set; }

    public Guid? CategoryId { get; set; }
    public CategoryDto? Category { get; set; }


    // Info
    public string Title { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? Details { get; set; }


    // DateTime and Capacity
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool AllDay { get; set; }

    public Recurrence Recurrence { get; set; } = Recurrence.OneTime;

    public int? Capacity { get; set; }



    // Location
    public LocationType LocationType { get; set; }

    public Guid? AddressId { get; set; }
    public AddressDto? Address { get; set; }

    public string? MeetupLink { get; set; }



    // Price
    public decimal? TicketPrice { get; set; }

    public bool Refundable { get; set; }








    public List<RsvpDto> Rsvps { get; set; } = new();

    public List<CommentDto> Comments { get; set; } = new();
}