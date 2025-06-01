namespace Meetups.Repository;

public interface IRsvpRepository
{
    Task<Result> AddAsync(string? email, Guid eventId, string? paymentId, string? paymentStatus);
}