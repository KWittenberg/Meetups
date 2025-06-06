namespace Meetups.DTOs;

public class RsvpDto
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }
    public EventDto? Event { get; set; }


    public Guid UserId { get; set; }
    public UserDto? User { get; set; }


    public RsvpStatus? Status { get; set; }



    public string? PaymentId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }


    public string? RefundId { get; set; }

    public RefundStatus? RefundStatus { get; set; }



    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}