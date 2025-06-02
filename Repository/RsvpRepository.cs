using Meetups.Extensions;

namespace Meetups.Repository;

public class RsvpRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IPaymentService paymentService) : IRsvpRepository
{
    public async Task<Result<RsvpDto>> GetByIdAsync(Guid id)
    {
        await using var db = await contextFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Rsvps.Include(x => x.Event).FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return Result<RsvpDto>.Error("Rsvp not found!");

            return Result<RsvpDto>.Ok(entity.ToDto());
        }
        catch (Exception ex)
        {
            return Result<RsvpDto>.Error($"Database error: {ex.Message}");
        }
    }



    public async Task<Result> AddAsync(string? email, Guid eventId, string? paymentId, string? paymentStatus)
    {
        await using var db = await contextFactory.CreateDbContextAsync();

        try
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null) return Result.Error("User not found!");

            var eventExist = await db.Events.AnyAsync(x => x.Id == eventId);
            if (!eventExist) return Result.Error("Event not found!");

            var rsvpExist = await db.Rsvps.AnyAsync(x => x.EventId == eventId && x.UserId == user.Id);
            if (rsvpExist) return Result.Error("User has already Rsvped for this event!");

            var rsvp = new Rsvp
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                EventId = eventId,
                RsvpDate = DateTime.Now,
                Status = RsvpStatus.Going,
                PaymentId = paymentId,
                PaymentStatus = paymentStatus.FromString()
            };

            await db.Rsvps.AddAsync(rsvp);
            await db.SaveChangesAsync();

            return Result.Ok("Rsvp added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result<Guid>> CancelAsync(Guid userId, Guid eventId)
    {
        await using var db = await contextFactory.CreateDbContextAsync();

        try
        {
            var rsvp = await db.Rsvps.Include(x => x.Event)
                                        .Where(x => x.EventId == eventId && x.UserId == userId && x.Status == RsvpStatus.Going)
                                        .FirstOrDefaultAsync();

            if (rsvp is null) return Result<Guid>.Error("Not found Rsvped for this event!");

            rsvp.Status = RsvpStatus.Canceled;

            if (rsvp.Event is not null && rsvp.Event.TicketPrice > 0 && rsvp.Event.Refundable)
            {
                var refund = await paymentService.CreateRefundAsync(rsvp.ToDto());

                rsvp.RefundId = refund.Id;
                rsvp.RefundStatus = refund.Status.FromRefundString();
            }

            await db.SaveChangesAsync();

            return Result<Guid>.Ok(rsvp.Id, "Rsvp Canceled!");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Error($"Database error: {ex.Message}");
        }
    }



    //public async Task<Result> AddAsync(string? email, Guid eventId)
    //{
    //    await using var db = await contextFactory.CreateDbContextAsync();

    //    try
    //    {
    //        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
    //        if (user is null) return Result.Error("User not found!");

    //        var eventExist = await db.Events.AnyAsync(x => x.Id == eventId);
    //        if (!eventExist) return Result.Error("Event not found!");

    //        var rsvpExist = await db.Rsvps.AnyAsync(x => x.EventId == eventId && x.UserId == user.Id);
    //        if (rsvpExist) return Result.Error("User has already Rsvped for this event!");

    //        var rsvp = new Rsvp
    //        {
    //            Id = Guid.NewGuid(),
    //            UserId = user.Id,
    //            EventId = eventId,
    //            RsvpDate = DateTime.Now,
    //            Status = RsvpStatus.Going
    //        };

    //        await db.Rsvps.AddAsync(rsvp);
    //        await db.SaveChangesAsync();

    //        return Result.Ok("Rsvp added!");
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Error($"Database error: {ex.Message}");
    //    }
    //}



    //public async Task<Result> AddAsync(Guid userId, Guid eventId)
    //{
    //    await using var db = await contextFactory.CreateDbContextAsync();

    //    try
    //    {
    //        var userExist = await db.Users.AnyAsync(x => x.Id == userId);
    //        if (!userExist) return Result.Error("User not found!");

    //        var eventExist = await db.Events.AnyAsync(x => x.Id == eventId);
    //        if (!eventExist) return Result.Error("Event not found!");

    //        var rsvpExist = await db.Rsvps.AnyAsync(x => x.EventId == eventId && x.UserId == userId);
    //        if (!rsvpExist) return Result.Error("Rsvp not found!");

    //        var rsvp = new Rsvp
    //        {
    //            Id = Guid.NewGuid(),
    //            UserId = userId,
    //            EventId = eventId,
    //            RsvpDate = DateTime.Now,
    //            Status = "Going"
    //        };

    //        await db.Rsvps.AddAsync(rsvp);
    //        await db.SaveChangesAsync();

    //        return Result.Ok("Rsvp added!");
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Error($"Database error: {ex.Message}");
    //    }
    //}
}