using System.ComponentModel.DataAnnotations.Schema;

namespace Meetups.Entities;

public class Event : BaseAuditableEntity<Guid>
{
    public Event() { }

    //public Event(string? imageUrl, string title, string? details, Address? location, string? meetupLink, int capacity,
    //            DateTime start, DateTime end, bool allDay, Recurrence recurrence, decimal? ticketPrice, bool refundable)
    //{
    //    ImageUrl = imageUrl;
    //    Title = title;
    //    Details = details;
    //    Location = location;
    //    MeetupLink = meetupLink;
    //    Capacity = capacity;
    //    Start = start;
    //    End = end;
    //    AllDay = allDay;
    //    Recurrence = recurrence;
    //    TicketPrice = ticketPrice;
    //    Refundable = refundable;
    //}


    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }


    // Info
    [Required]
    [StringLength(maximumLength: 100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(maximumLength: 200)]
    public string? ImageUrl { get; set; }

    [StringLength(maximumLength: 4000)]
    public string? Details { get; set; }


    // DateTime and Capacity
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool AllDay { get; set; }

    public Recurrence Recurrence { get; set; } = Recurrence.OneTime;

    [Range(0, int.MaxValue)]
    public int? Capacity { get; set; }



    // Location
    public LocationType LocationType { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    [StringLength(maximumLength: 200)]
    public string? MeetupLink { get; set; }



    // Price
    [Range(0, 9999999.99)]
    [Column(TypeName = "decimal(9, 2)")]
    public decimal? TicketPrice { get; set; }

    public bool Refundable { get; set; }






    public List<Rsvp> Rsvps { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
}