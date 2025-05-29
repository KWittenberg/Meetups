namespace Meetups.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Role { get; set; }



    public List<RsvpDto> Rsvps { get; set; } = new();
}