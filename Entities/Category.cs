namespace Meetups.Entities;

public class Category : BaseAuditableEntity<Guid>
{
    public Category() { }

    //public Category(string name, string? description, string? iconHtml)
    //{
    //    Name = name;
    //    Description = description;
    //    IconHtml = iconHtml;
    //}


    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? IconHtml { get; set; }
}