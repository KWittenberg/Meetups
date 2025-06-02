namespace Meetups.Repository;

public interface IRsvpRepository
{
    Task<Result<RsvpDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(string? email, Guid eventId, string? paymentId, string? paymentStatus);

    Task<Result<Guid>> CancelAsync(Guid userId, Guid eventId);
}