namespace Meetups.DTOs;

public class UserInput
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? Role { get; set; }
}