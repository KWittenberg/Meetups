namespace Meetups.Entities;

public class Category : BaseAuditableEntity<Guid>
{
    public Category() { }

    public Category(string name, string? description, string? icon)
    {
        Name = name;
        Description = description;
        Icon = icon;
    }


    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Icon { get; set; }
}