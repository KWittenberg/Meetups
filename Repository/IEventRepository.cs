using Microsoft.AspNetCore.Components.Forms;

namespace Meetups.Repository;

public interface IEventRepository
{
    Task<Result<List<EventDto>>> GetAllAsync();

    Task<Result<EventDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(EventInput input);

    Task<Result> UpdateAsync(Guid id, EventInput input);

    Task<Result> DeleteAsync(Guid id);


    List<string> GetAllCategories();

    string ValidateEvent(EventInput input);




    Task<Result<string>> GenerateImagePreviewAsync(IBrowserFile? file);
}