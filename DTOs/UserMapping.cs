namespace Meetups.DTOs;

public static class UserMapping
{
    // GetAllAsync()
    public static List<UserDto> ToDtoList(this IEnumerable<User>? entities)
    {
        if (entities is null) return new List<UserDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static UserDto ToDto(this User? entity)
    {
        if (entity is null) return null;

        return new UserDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Role = entity.Role,
            Rsvps = entity.Rsvps?.ToDtoList() ?? new List<RsvpDto>()
        };
    }

    // AddAsync()
    public static User ToEntity(this UserInput? input)
    {
        if (input == null) return null;

        return new User
        {
            Name = input.Name,
            Email = input.Email,
            Role = input.Role
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this User? entity, UserInput? input)
    {
        if (entity == null || input == null) return;

        entity.Name = input.Name;
        entity.Email = input.Email;
        entity.Role = input.Role;
    }

    // UI ->
    public static UserInput ToInput(this UserDto? dto)
    {
        if (dto is null) return null;

        return new UserInput
        {
            Name = dto.Name,
            Email = dto.Email,
            Role = dto.Role
        };
    }
}