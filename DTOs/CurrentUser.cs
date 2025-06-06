namespace Meetups.DTOs;

public class CurrentUser
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? ImageUrl { get; set; }

    public string? Role { get; set; }

    public bool IsAuthenticated { get; set; }
}