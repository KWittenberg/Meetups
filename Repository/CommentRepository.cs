namespace Meetups.Repository;

public class CommentRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : ICommentRepository
{
    public async Task<Result<List<CommentDto>>> GetCommentsByEventIdAsync(Guid eventId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Comments.AsNoTracking().Where(x => x.EventId == eventId).ToListAsync();
            if (!entities.Any()) return Result<List<CommentDto>>.Error("Comment not found!");

            var output = entities.ToDtoList();

            return Result<List<CommentDto>>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<List<CommentDto>>.Error($"Error: {ex.Message}");
        }
    }

    public async Task<Result> AddAsync(CommentInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            await db.Comments.AddAsync(input.ToEntity());
            await db.SaveChangesAsync();

            return Result.Ok("Comment added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }
}