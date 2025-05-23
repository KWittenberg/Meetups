namespace Meetups.DTOs;

public static class EventMapping
{
    // GetAllAsync()
    public static List<EventDto> ToDtoList(this IEnumerable<Event>? entities)
    {
        if (entities is null) return new List<EventDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static EventDto ToDto(this Event? entity)
    {
        if (entity is null) return null;

        return new EventDto
        {
            Id = entity.Id,
            ImageUrl = entity.ImageUrl,
            Title = entity.Title,
            Description = entity.Description,
            Location = entity.Location,
            MeetupLink = entity.MeetupLink,
            Category = entity.Category,
            Capacity = entity.Capacity,
            Start = entity.Start,
            End = entity.End,
            AllDay = entity.AllDay,
            OrganizerId = entity.OrganizerId
        };
    }

    // AddAsync()
    public static Event ToEntity(this EventInput? input)
    {
        if (input == null) return null;

        return new Event(
            imageUrl: input.ImageUrl,
            title: input.Title,
            description: input.Description,
            location: input.Location,
            meetupLink: input.MeetupLink,
            category: input.Category,
            capacity: input.Capacity,
            start: input.Start,
            end: input.End,
            allDay: input.AllDay)
        {
            // Note: OrganizerId is not set here since Event constructor doesn't include it.
            // If OrganizerId is part of the Event entity, set it explicitly.
            OrganizerId = input.OrganizerId
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Event? entity, EventInput? input)
    {
        if (entity == null || input == null) return;

        entity.ImageUrl = input.ImageUrl;
        entity.Title = input.Title;
        entity.Description = input.Description;
        entity.Location = input.Location;
        entity.MeetupLink = input.MeetupLink;
        entity.Category = input.Category;
        entity.Capacity = input.Capacity;
        entity.Start = input.Start;
        entity.End = input.End;
        entity.AllDay = input.AllDay;
        entity.OrganizerId = input.OrganizerId;
    }

    // UI -> 
    public static EventInput ToInput(this EventDto? dto)
    {
        if (dto is null) return null;

        return new EventInput
        {
            ImageUrl = dto.ImageUrl,
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            MeetupLink = dto.MeetupLink,
            Category = dto.Category,
            Capacity = dto.Capacity,
            Start = dto.Start,
            End = dto.End,
            AllDay = dto.AllDay,
            OrganizerId = dto.OrganizerId
        };
    }
}