namespace Meetups.Repository;

public class EventRepository : IEventRepository
{

    public async Task<Result> ValidateEvent(EventInput input)
    {
        string? errorMessage = string.Empty;

        errorMessage = input.ValidateDate();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return Result.Error(errorMessage);

        errorMessage = input.ValidateLocation();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return Result.Error(errorMessage);

        errorMessage = input.ValidateMeetupLink();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return Result.Error(errorMessage);


        return Result.Ok("Success");
    }
}