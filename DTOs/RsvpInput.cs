namespace Meetups.DTOs;

public class RsvpInput
{
    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public DateTime RsvpDate { get; set; } = DateTime.UtcNow;

    public RsvpStatus? Status { get; set; } = RsvpStatus.Going;




    [StringLength(maximumLength: 200)]
    public string? PaymentId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    [StringLength(maximumLength: 50)]
    public string? RefundId { get; set; }

    public RefundStatus? RefundStatus { get; set; }
}