namespace Meetups.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? IconHtml { get; set; }
}