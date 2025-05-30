namespace Meetups.DTOs;

public class CurrentUser
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Role { get; set; }


    public bool IsAuthenticated { get; set; }
}