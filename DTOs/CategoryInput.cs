namespace Meetups.DTOs;

public class CategoryInput
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Icon { get; set; }
}