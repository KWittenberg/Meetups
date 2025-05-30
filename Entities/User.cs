namespace Meetups.Entities;

public class User : BaseAuditableEntity<Guid>
{
    [StringLength(maximumLength: 200)]
    public string Name { get; set; } = string.Empty;


    [StringLength(maximumLength: 200)]
    public string Email { get; set; } = string.Empty;


    [StringLength(maximumLength: 50)]
    public string? Role { get; set; }



    public List<Rsvp> Rsvps { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
}