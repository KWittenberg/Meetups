namespace Meetups.Repository;

public interface IEventRepository
{
    Task<Result<List<EventDto>>> GetAllAsync();

    Task<Result<EventDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(ImageData inputImage, EventInput input);

    Task<Result> UpdateAsync(Guid id, EventInput input, ImageData? imageData = null);

    Task<Result> DeleteAsync(Guid id);


    Task<Result> DeleteImageAsync(Guid id);


    List<string> GetAllCategories();

    string ValidateEvent(EventInput input);




    Task<Result<ImageData>> GenerateImagePreviewAsync(IBrowserFile? file);




    Task<Result<List<EventDto>>> GetEventsAsync(string? filter);
}