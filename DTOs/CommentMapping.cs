namespace Meetups.DTOs;

public static class CommentMapping
{
    // GetAllAsync()
    public static List<CommentDto> ToDtoList(this IEnumerable<Comment>? entities)
    {
        if (entities is null) return new List<CommentDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static CommentDto ToDto(this Comment? entity)
    {
        if (entity is null) return null;

        return new CommentDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User,
            EventId = entity.EventId,
            Event = entity.Event,
            Message = entity.Message,
            CreatedUtc = entity.CreatedUtc
        };
    }

    // AddAsync()
    public static Comment ToEntity(this CommentInput? input)
    {
        if (input == null) return null;

        return new Comment(
            userId: input.UserId,
            eventId: input.EventId,
            message: input.Message);
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Comment? entity, CommentInput? input)
    {
        if (entity == null || input == null) return;

        entity.UserId = input.UserId;
        entity.EventId = input.EventId;
        entity.Message = input.Message;
    }

    // UI -> 
    public static CommentInput ToInput(this CommentDto? dto)
    {
        if (dto is null) return null;

        return new CommentInput
        {
            UserId = dto.UserId,
            EventId = dto.EventId,
            Message = dto.Message
        };
    }
}