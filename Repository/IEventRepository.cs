namespace Meetups.Repository;

public interface IEventRepository
{
    Task<Result<List<EventDto>>> GetAllAsync();

    Task<Result<EventDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(EventInput input);

    Task<Result> UpdateAsync(Guid id, EventInput input);

    Task<Result> DeleteAsync(Guid id);



    string ValidateEvent(EventInput input);
}