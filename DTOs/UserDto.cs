namespace Meetups.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;


    public string Name { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }



    public string? ImageUrl { get; set; }

    public DateOnly? DateOfBirth { get; set; }


    public string? Role { get; set; }



    public Guid? AddressId { get; set; }
    public AddressDto? Address { get; set; }





    public List<RsvpDto> Rsvps { get; set; } = new();

    public List<CommentDto> Comments { get; set; } = new();
}