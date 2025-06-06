namespace Meetups.DTOs;

public static class EventMapping
{
    // GetAllAsync()
    public static List<EventDto> ToDtoList(this IEnumerable<Event>? entities)
    {
        if (entities is null) return new List<EventDto>();

        // return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();

        return entities.Select(e => new EventDto
        {
            Id = e.Id,
            UserId = e.UserId,
            User = e.User?.ToDto(),
            CategoryId = e.CategoryId,
            Category = e.Category?.ToDto(),

            Title = e.Title,
            ImageUrl = e.ImageUrl,
            Details = e.Details,

            Start = e.Start,
            End = e.End,
            AllDay = e.AllDay,
            Recurrence = e.Recurrence,
            Capacity = e.Capacity,

            LocationType = e.LocationType,
            AddressId = e.AddressId,
            Address = e.Address?.ToDto(),
            MeetupLink = e.MeetupLink,

            TicketPrice = e.TicketPrice,
            Refundable = e.Refundable,

            Rsvps = e.Rsvps
                .Where(r => r.Status == RsvpStatus.Going && r.User != null)
                .Select(r => new RsvpDto
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    UserId = r.UserId,
                    User = new UserDto
                    {
                        Id = r.User.Id,
                        Name = r.User.Name,
                        Email = r.User.Email,
                        ImageUrl = r.User.ImageUrl,
                    },
                    Status = r.Status,
                    PaymentId = r.PaymentId,
                    PaymentStatus = r.PaymentStatus,
                    RefundId = r.RefundId,
                    RefundStatus = r.RefundStatus,
                    CreatedUtc = r.CreatedUtc
                }).ToList(),
            // Comments = entity.Comments?.Select(c => c.ToDto()).ToList() ?? new List<CommentDto>()
        }).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static EventDto ToDto(this Event? entity)
    {
        if (entity is null) return null;

        return new EventDto
        {
            Id = entity.Id,

            UserId = entity.UserId,
            User = entity.User?.ToDto(),

            CategoryId = entity.CategoryId,
            Category = entity.Category?.ToDto(),

            Title = entity.Title,
            ImageUrl = entity.ImageUrl,
            Details = entity.Details,

            Start = entity.Start,
            End = entity.End,
            AllDay = entity.AllDay,
            Recurrence = entity.Recurrence,
            Capacity = entity.Capacity,

            LocationType = entity.LocationType,
            AddressId = entity.AddressId,
            Address = entity.Address?.ToDto(),
            MeetupLink = entity.MeetupLink,

            TicketPrice = entity.TicketPrice,
            Refundable = entity.Refundable,

            Rsvps = entity.Rsvps.Select(r => r.ToDto()).ToList()
            //Comments = entity.Comments?.Select(c => c.ToDto()).ToList()
        };
    }

    // AddAsync()
    public static Event ToEntity(this EventInput? input)
    {
        if (input == null) return null;

        return new Event
        {
            UserId = input.UserId,
            CategoryId = input.CategoryId,

            Title = input.Title,
            ImageUrl = input.ImageUrl,
            Details = input.Details,

            Start = input.Start,
            End = input.End,
            AllDay = input.AllDay,
            Recurrence = input.Recurrence,
            Capacity = input.Capacity,

            LocationType = input.LocationType,
            AddressId = input.AddressId,
            MeetupLink = input.MeetupLink,

            TicketPrice = input.TicketPrice,
            Refundable = input.Refundable
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Event? entity, EventInput? input)
    {
        if (entity == null || input == null) return;

        entity.UserId = input.UserId;
        entity.CategoryId = input.CategoryId;

        entity.Title = input.Title;
        entity.ImageUrl = input.ImageUrl;
        entity.Details = input.Details;

        entity.Start = input.Start;
        entity.End = input.End;
        entity.AllDay = input.AllDay;
        entity.Recurrence = input.Recurrence;
        entity.Capacity = input.Capacity;

        entity.LocationType = input.LocationType;
        entity.AddressId = input.AddressId;
        entity.MeetupLink = input.MeetupLink;

        entity.TicketPrice = input.TicketPrice;
        entity.Refundable = input.Refundable;
    }

    // UI -> 
    public static EventInput ToInput(this EventDto? dto)
    {
        if (dto is null) return null;

        return new EventInput
        {
            UserId = dto.UserId,
            CategoryId = dto.CategoryId,

            Title = dto.Title,
            ImageUrl = dto.ImageUrl,
            Details = dto.Details,

            Start = dto.Start,
            End = dto.End,
            AllDay = dto.AllDay,
            Recurrence = dto.Recurrence,
            Capacity = dto.Capacity,

            LocationType = dto.LocationType,
            AddressId = dto.AddressId,
            MeetupLink = dto.MeetupLink,

            TicketPrice = dto.TicketPrice,
            Refundable = dto.Refundable
        };
    }
}