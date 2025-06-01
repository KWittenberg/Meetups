namespace Meetups.DTOs;

public class RsvpDto
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    [JsonIgnore]
    public EventDto? Event { get; set; }


    public Guid UserId { get; set; }

    [JsonIgnore]
    public UserDto? User { get; set; }


    public DateTime RsvpDate { get; set; } = DateTime.UtcNow;

    public RsvpStatus? Status { get; set; }



    public string? PaymentId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }


    public string? RefundId { get; set; }

    public RefundStatus? RefundStatus { get; set; }
}