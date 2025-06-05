namespace Meetups.Repository;

public interface ICategoryRepository
{
    Task<Result<List<CategoryDto>>> GetAllAsync();

    Task<Result<CategoryDto>> GetByIdAsync(Guid id);

    Task<Result> AddAsync(CategoryInput input);

    Task<Result> UpdateAsync(Guid id, CategoryInput input);

    Task<Result> DeleteAsync(Guid id);
}