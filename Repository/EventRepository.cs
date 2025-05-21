namespace Meetups.Repository;

public class EventRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : IEventRepository
{
    public async Task<Result<List<EventDto>>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var query = await db.Events.AsNoTracking().Select(x => new EventDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                MeetupLink = x.MeetupLink,
                Category = x.Category,
                Capacity = x.Capacity,
                StartDate = x.StartDate,
                StartTime = x.StartTime,
                EndDate = x.EndDate,
                EndTime = x.EndTime,
                AllDay = x.AllDay,
                OrganizerId = x.OrganizerId
            }).ToListAsync();

            if (!query.Any()) return Result<List<EventDto>>.Error("Event not found!");

            return Result<List<EventDto>>.Ok(query);
        }
        catch (Exception ex)
        {
            return Result<List<EventDto>>.Error($"Error: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> GetByIdAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity == null) return Result<EventDto>.Error("Event not found!");

            var output = new EventDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Location = entity.Location,
                MeetupLink = entity.MeetupLink,
                Category = entity.Category,
                Capacity = entity.Capacity,
                StartDate = entity.StartDate,
                StartTime = entity.StartTime,
                EndDate = entity.EndDate,
                EndTime = entity.EndTime,
                AllDay = entity.AllDay,
                OrganizerId = entity.OrganizerId
            };

            return Result<EventDto>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> AddAsync(EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = new Event(
                input.Title,
                input.Description,
                input.Location,
                input.MeetupLink,
                input.Category,
                input.Capacity,
                input.StartDate,
                input.StartTime,
                input.EndDate,
                input.EndTime,
                input.AllDay);

            await db.Events.AddAsync(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> UpdateAsync(Guid id, EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity == null) return Result.Error("Event not found!");

            entity.Title = input.Title;
            entity.Description = input.Description;
            entity.Location = input.Location;
            entity.MeetupLink = input.MeetupLink;
            entity.Category = input.Category;
            entity.Capacity = input.Capacity;
            entity.StartDate = input.StartDate;
            entity.StartTime = input.StartTime;
            entity.EndDate = input.EndDate;
            entity.EndTime = input.EndTime;
            entity.AllDay = input.AllDay;

            db.Events.Update(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event updated!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        try
        {
            var entity = await db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return Result.Error("Event not found!");

            db.Events.Remove(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event deleted!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }






    public string ValidateEvent(EventInput input)
    {
        string? errorMessage = string.Empty;

        errorMessage = input.ValidateDate();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;

        errorMessage = input.ValidateLocation();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;

        errorMessage = input.ValidateMeetupLink();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;


        return string.Empty;
    }
}