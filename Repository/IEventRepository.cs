namespace Meetups.Repository;

public interface IEventRepository
{
    Task<Result<List<EventDto>>> GetAllAsync();

    Task<Result<EventDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(ImageData inputImage, EventInput input);

    Task<Result> UpdateAsync(Guid id, EventInput input);

    Task<Result> DeleteAsync(Guid id);


    List<string> GetAllCategories();

    string ValidateEvent(EventInput input);




    Task<Result<ImageData>> GenerateImagePreviewAsync(IBrowserFile? file);
}