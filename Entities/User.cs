namespace Meetups.Entities;

public class User : BaseAuditableEntity<Guid>
{
    public string Email { get; set; } = string.Empty;


    public string Name { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }



    public string? ImageUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }


    public string? Role { get; set; }



    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }



    public bool IsSuspended { get; set; }
    public DateTime? SuspendedUntil { get; set; }



    public List<Rsvp> Rsvps { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
}