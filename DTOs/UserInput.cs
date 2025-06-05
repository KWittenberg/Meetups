namespace Meetups.DTOs;

public class UserInput
{
    [StringLength(maximumLength: 200)]
    public string Email { get; set; } = string.Empty;



    [StringLength(maximumLength: 100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(maximumLength: 50)]
    public string? FirstName { get; set; }

    [StringLength(maximumLength: 50)]
    public string? LastName { get; set; }


    [StringLength(maximumLength: 200)]
    public string? ImageUrl { get; set; }

    public DateOnly? DateOfBirth { get; set; }


    [StringLength(maximumLength: 20)]
    public string? Role { get; set; }



    public Guid? AddressId { get; set; }
}