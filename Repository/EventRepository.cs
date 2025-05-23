using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Meetups.Repository;

public class EventRepository(IDbContextFactory<ApplicationDbContext> dbFactory, IWebHostEnvironment webHostEnvironment) : IEventRepository
{
    // private const int MaxAllowedSize = 500 * 1024; // 500 KB
    const int MaxAllowedSize = 5 * 1024 * 1024; // 5 MB

    static readonly string EventFolder = Path.Combine("img", "events");

    #region CRUD
    public async Task<Result<List<EventDto>>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Events.AsNoTracking().ToListAsync();
            if (!entities.Any()) return Result<List<EventDto>>.Error("Event not found!");

            return Result<List<EventDto>>.Ok(entities.ToDtoList());
        }
        catch (Exception ex)
        {
            return Result<List<EventDto>>.Error($"Error: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> GetByIdAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity is null) return Result<EventDto>.Error("Event not found!");

            return Result<EventDto>.Ok(entity.ToDto());
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> AddAsync(EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = input.ToEntity();
            entity.Id = Guid.NewGuid();

            if (input.CoverImage is not null)
            {
                var result = await SaveImageAsync(entity.Id, input, false);
                if (!result.Success) return Result.Error(result.Message);

                entity.ImageUrl = result.Data;
            }

            await db.Events.AddAsync(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> UpdateAsync(Guid id, EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity is null) return Result.Error("Event not found!");

            entity.UpdateFromInput(input);
            db.Events.Update(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event updated!");
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
            var entity = await db.Events.FindAsync(id);
            if (entity is null) return Result.Error("Event not found!");

            db.Events.Remove(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event deleted!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }
    #endregion



    public List<string> GetAllCategories() => Enum.GetNames(typeof(MeetupCategories)).ToList();

    public string ValidateEvent(EventInput input)
    {
        string? errorMessage = string.Empty;

        //errorMessage = input.ValidateDate();
        //if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;

        errorMessage = input.ValidateLocation();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;

        errorMessage = input.ValidateMeetupLink();
        if (!string.IsNullOrWhiteSpace(errorMessage)) return errorMessage;


        return string.Empty;
    }




    public async Task<Result<string>> GenerateImagePreviewAsync(IBrowserFile? file)
    {
        if (file is null) return Result<string>.Error("No file selected!");
        if (file.Size <= 0) return Result<string>.Error("Invalid file size!");
        if (file.Size > MaxAllowedSize) return Result<string>.Error("Image too large! Maximum size is 500KB.");

        try
        {
            await using var stream = file.OpenReadStream(MaxAllowedSize);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();

            using var image = Image.Load(bytes);
            var previewImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(300, 0)
            }));

            using var previewStream = new MemoryStream();
            await previewImage.SaveAsJpegAsync(previewStream);
            var imagePreviewBytes = previewStream.ToArray();
            var base64String = $"data:image/jpeg;base64,{Convert.ToBase64String(imagePreviewBytes)}";

            return Result<string>.Ok(base64String);
        }
        catch (Exception ex)
        {
            return Result<string>.Error($"Error processing image: {ex.Message}");
        }
    }

    private async Task<Result<string>> SaveImageAsync(Guid id, EventInput input, bool isOriginal, int sizePx = 600)
    {
        if (input.CoverImage is null) return Result<string>.Error("No image provided!");

        try
        {
            if (!string.IsNullOrWhiteSpace(input.ImageUrl))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, input.ImageUrl.Replace("/", "\\"));
                if (File.Exists(imagePath)) File.Delete(imagePath);
            }

            var extension = Path.GetExtension(input.CoverImage!.Name);
            var fileName = $"{id}{extension}";
            var saveFolder = Path.Combine(webHostEnvironment.WebRootPath, EventFolder);
            var filePath = Path.Combine(saveFolder, fileName);
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

            if (isOriginal)
            {
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await input.CoverImage.OpenReadStream(MaxAllowedSize).CopyToAsync(fileStream);
            }
            else
            {
                await using var stream = input.CoverImage.OpenReadStream(MaxAllowedSize);
                using var image = await Image.LoadAsync(stream);
                image.Mutate(ctx => ctx.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(sizePx, 0)
                }));

                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await image.SaveAsJpegAsync(fileStream);
            }

            var relativePath = Path.Combine(EventFolder, fileName).Replace("\\", "/");
            input.ImageUrl = relativePath;

            return Result<string>.Ok(relativePath, "ImageSaved");
        }
        catch (Exception ex)
        {
            return Result<string>.Error($"Error saving image: {ex.Message}");
        }
    }






    #region OLD
    public async Task<Result<List<EventDto>>> GetAllAsyncOld()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var query = await db.Events.AsNoTracking().Select(x => new EventDto
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Title = x.Title,
                Description = x.Description,
                Location = x.Location,
                MeetupLink = x.MeetupLink,
                Category = x.Category,
                Capacity = x.Capacity,
                Start = x.Start,
                End = x.End,
                AllDay = x.AllDay,
                OrganizerId = x.OrganizerId
            }).ToListAsync();

            if (!query.Any()) return Result<List<EventDto>>.Error("Event not found!");

            return Result<List<EventDto>>.Ok(query);
        }
        catch (Exception ex)
        {
            return Result<List<EventDto>>.Error($"Error: {ex.Message}");
        }
    }

    public async Task<Result<EventDto>> GetByIdAsyncOld(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity == null) return Result<EventDto>.Error("Event not found!");

            var output = new EventDto
            {
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                Title = entity.Title,
                Description = entity.Description,
                Location = entity.Location,
                MeetupLink = entity.MeetupLink,
                Category = entity.Category,
                Capacity = entity.Capacity,
                Start = entity.Start,
                End = entity.End,
                AllDay = entity.AllDay,
                OrganizerId = entity.OrganizerId
            };

            return Result<EventDto>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<EventDto>.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> AddAsyncOld(EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = new Event(
                input.ImageUrl,
                input.Title,
                input.Description,
                input.Location,
                input.MeetupLink,
                input.Category,
                input.Capacity,
                input.Start,
                input.End,
                input.AllDay);

            await db.Events.AddAsync(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event added!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }

    public async Task<Result> UpdateAsyncOld(Guid id, EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity == null) return Result.Error("Event not found!");

            entity.ImageUrl = input.ImageUrl;
            entity.Title = input.Title;
            entity.Description = input.Description;
            entity.Location = input.Location;
            entity.MeetupLink = input.MeetupLink;
            entity.Category = input.Category;
            entity.Capacity = input.Capacity;
            entity.Start = input.Start;
            entity.End = input.End;
            entity.AllDay = input.AllDay;

            db.Events.Update(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event updated!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }
    #endregion
}