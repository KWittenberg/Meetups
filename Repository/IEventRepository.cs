namespace Meetups.Repository;

public interface IEventRepository
{
    Task<Result> ValidateEvent(EventInput input);
}