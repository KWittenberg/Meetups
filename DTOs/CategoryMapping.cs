namespace Meetups.DTOs;

public static class CategoryMapping
{
    // GetAllAsync()
    public static List<CategoryDto> ToDtoList(this IEnumerable<Category>? entities)
    {
        if (entities is null) return new List<CategoryDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static CategoryDto ToDto(this Category? entity)
    {
        if (entity is null) return null;

        return new CategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Icon = entity.Icon
        };
    }

    // AddAsync()
    public static Category ToEntity(this CategoryInput? input)
    {
        if (input == null) return null;

        return new Category
        {
            Name = input.Name,
            Description = input.Description,
            Icon = input.Icon
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Category? entity, CategoryInput? input)
    {
        if (entity == null || input == null) return;

        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Icon = input.Icon;
    }

    // UI -> 
    public static CategoryInput ToInput(this CategoryDto? dto)
    {
        if (dto is null) return null;

        return new CategoryInput
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon
        };
    }
}