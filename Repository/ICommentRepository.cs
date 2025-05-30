namespace Meetups.Repository;

public interface ICommentRepository
{
    Task<Result<List<CommentDto>>> GetCommentsByEventIdAsync(Guid eventId);

    Task<Result> AddAsync(CommentInput input);
}