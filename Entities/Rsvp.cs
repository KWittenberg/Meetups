namespace Meetups.Entities;

public class Rsvp : BaseEntity<Guid>
{
    public Guid EventId { get; set; }
    public Event? Event { get; set; }


    public Guid UserId { get; set; }
    public User? User { get; set; }


    public RsvpStatus? Status { get; set; }



    public string? PaymentId { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }



    public string? RefundId { get; set; }

    public RefundStatus? RefundStatus { get; set; }


    public DateTime CreatedUtc { get; set; }
}