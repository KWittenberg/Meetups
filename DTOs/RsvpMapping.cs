namespace Meetups.DTOs;

public static class RsvpMapping
{

    // GetAllAsync()
    public static List<RsvpDto> ToDtoList(this IEnumerable<Rsvp>? entities)
    {
        if (entities is null) return new List<RsvpDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static RsvpDto ToDto(this Rsvp? entity)
    {
        if (entity is null) return null;

        return new RsvpDto
        {
            Id = entity.Id,
            EventId = entity.EventId,
            Event = null,
            // Event = entity.Event?.ToDto(), // izbjegavaj ciklus!
            UserId = entity.UserId,
            User = entity.User?.ToDto(),
            RsvpDate = entity.RsvpDate,
            Status = entity.Status,
            PaymentId = entity.PaymentId,
            PaymentStatus = entity.PaymentStatus,
            RefundId = entity.RefundId,
            RefundStatus = entity.RefundStatus
        };
    }

    // AddAsync()
    public static Rsvp ToEntity(this RsvpInput? input)
    {
        if (input == null) return null;

        return new Rsvp
        {
            EventId = input.EventId,
            UserId = input.UserId,
            RsvpDate = input.RsvpDate,
            Status = input.Status,
            PaymentId = input.PaymentId,
            PaymentStatus = input.PaymentStatus,
            RefundId = input.RefundId,
            RefundStatus = input.RefundStatus
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Rsvp? entity, RsvpInput? input)
    {
        if (entity == null || input == null) return;

        entity.EventId = input.EventId;
        entity.UserId = input.UserId;
        entity.RsvpDate = input.RsvpDate;
        entity.Status = input.Status;
        entity.PaymentId = input.PaymentId;
        entity.PaymentStatus = input.PaymentStatus;
        entity.RefundId = input.RefundId;
        entity.RefundStatus = input.RefundStatus;
    }

    // UI ->
    public static RsvpInput ToInput(this RsvpDto? dto)
    {
        if (dto is null) return null;

        return new RsvpInput
        {
            EventId = dto.EventId,
            UserId = dto.UserId,
            RsvpDate = dto.RsvpDate,
            Status = dto.Status,
            PaymentId = dto.PaymentId,
            PaymentStatus = dto.PaymentStatus,
            RefundId = dto.RefundId,
            RefundStatus = dto.RefundStatus
        };
    }
}