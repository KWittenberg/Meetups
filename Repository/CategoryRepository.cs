namespace Meetups.Repository;

public class CategoryRepository(IDbContextFactory<ApplicationDbContext> dbFactory) : ICategoryRepository
{
    public async Task<Result<List<CategoryDto>>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Categories.AsNoTracking().ToListAsync();
            if (!entities.Any()) return Result<List<CategoryDto>>.Error("Category Not Found!");

            return Result<List<CategoryDto>>.Ok(entities.ToDtoList());
        }
        catch (Exception ex)
        {
            return Result<List<CategoryDto>>.Error($"Error: {ex.Message}");
        }
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Categories.FindAsync(id);
            if (entity is null) return Result<CategoryDto>.Error("Category Not Found!");

            return Result<CategoryDto>.Ok(entity.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result<CategoryDto>> GetByNameAsync(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (entity is null) return Result<CategoryDto>.Error("Category Not Found!");

            return Result<CategoryDto>.Ok(entity.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> AddAsync(CategoryInput input)
    {
        if (await IsNameAvailableAsync(input.Name)) return Result.Error("Category with this Name already exists!");

        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = input.ToEntity();

            await db.Categories.AddAsync(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Category Added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> UpdateAsync(Guid id, CategoryInput input)
    {
        if (await IsNameAvailableAsync(input.Name)) return Result.Error("Category with this Name already exists!");

        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Categories.FindAsync(id);
            if (entity is null) return Result.Error("Category Not Found!");

            entity.UpdateFromInput(input);

            db.Categories.Update(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Category Updated!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Categories.FindAsync(id);
            if (entity is null) return Result.Error("Category Not Found!");

            db.Categories.Remove(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Category Deleted!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    private async Task<bool> IsNameAvailableAsync(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        return await db.Categories.AnyAsync(x => x.Name.ToLower() == name.ToLower());
    }
}