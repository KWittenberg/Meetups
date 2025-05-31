namespace Meetups.Repository;

public class EventRepository(IDbContextFactory<ApplicationDbContext> dbFactory, IWebHostEnvironment webHostEnvironment) : IEventRepository
{
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

    public async Task<Result<List<EventDto>>> GetEventsByOrganizerIdAsync(Guid organizerId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Events.AsNoTracking().Where(x => x.OrganizerId == organizerId).ToListAsync();
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

    public async Task<Result> AddAsync(ImageData imageData, EventInput input)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = input.ToEntity();
            entity.Id = Guid.NewGuid();

            if (imageData.FileBytes is not null)
            {
                var result = await SaveImageAsync(entity.Id, input, imageData, false);
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

    public async Task<Result> UpdateAsync(Guid id, EventInput input, ImageData? imageData = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity is null) return Result.Error("Event not found!");

            entity.UpdateFromInput(input);

            if (imageData?.FileBytes is not null)
            {
                var result = await SaveImageAsync(id, input, imageData, false);
                if (!result.Success) return Result.Error(result.Message);

                entity.ImageUrl = result.Data;
            }

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

            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, entity.ImageUrl.Replace("/", "\\"));
                if (File.Exists(filePath)) File.Delete(filePath);
            }

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

    public async Task<Result> DeleteImageAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entity = await db.Events.FindAsync(id);
            if (entity is null) return Result.Error("Event not found!");
            if (string.IsNullOrWhiteSpace(entity.ImageUrl)) return Result.Error("Event Image - Not found!");

            var filePath = Path.Combine(webHostEnvironment.WebRootPath, entity.ImageUrl.Replace("/", "\\"));
            if (File.Exists(filePath)) File.Delete(filePath);

            entity.ImageUrl = string.Empty;

            db.Events.Update(entity);
            await db.SaveChangesAsync();

            return Result.Ok("Event Image deleted!");
        }
        catch (Exception ex)
        {
            return Result.Error($"Database error: {ex.Message}");
        }
    }






    public List<string> GetAllCategories() => Enum.GetNames(typeof(MeetupCategories)).ToList();
    public List<string> GetAllRecurrence() => Enum.GetNames(typeof(Recurrence)).ToList();

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




    public async Task<Result<ImageData>> GenerateImagePreviewAsync(IBrowserFile? file)
    {
        if (file is null) return Result<ImageData>.Error("No file selected!");
        if (file.Size <= 0) return Result<ImageData>.Error("Invalid file size!");
        if (file.Size > MaxAllowedSize) return Result<ImageData>.Error("Image too large! Maximum size is 500KB.");

        try
        {
            await using var stream = file.OpenReadStream(MaxAllowedSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            using var image = Image.Load(fileBytes);

            var previewImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(400, 0)
            }));

            //using var ms = new MemoryStream();
            //await stream.CopyToAsync(ms);
            //var bytes = ms.ToArray();

            //using var image = Image.Load(bytes);
            //var previewImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
            //{
            //    Mode = ResizeMode.Max,
            //    Size = new Size(400, 0)
            //}));

            using var previewStream = new MemoryStream();
            await previewImage.SaveAsJpegAsync(previewStream);
            var imagePreviewBytes = previewStream.ToArray();
            var base64String = $"data:image/jpeg;base64,{Convert.ToBase64String(imagePreviewBytes)}";

            var imageData = new ImageData
            {
                FileBytes = fileBytes,
                ImagePreviewBase64 = base64String,
                Name = file.Name,
                Width = image.Width,
                Height = image.Height,
                Size = file.Size,
                FormattedSize = ToFileSizeString(file.Size),
                ContentType = file.ContentType,
                LastModified = file.LastModified
            };

            return Result<ImageData>.Ok(imageData);
        }
        catch (Exception ex)
        {
            return Result<ImageData>.Error($"Error processing image: {ex.Message}");
        }
    }

    private async Task<Result<string>> SaveImageAsync(Guid id, EventInput input, ImageData imageData, bool isOriginal, int sizePx = 600)
    {
        if (imageData.FileBytes is null) return Result<string>.Error("No image provided!");

        try
        {
            if (!string.IsNullOrWhiteSpace(input.ImageUrl))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, input.ImageUrl.Replace("/", "\\"));
                // if (File.Exists(imagePath)) File.Delete(imagePath);
                if (imagePath.StartsWith(Path.Combine(webHostEnvironment.WebRootPath, EventFolder), StringComparison.OrdinalIgnoreCase) && File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            var extension = Path.GetExtension(imageData.Name);
            var fileName = $"{id}{extension}";
            var saveFolder = Path.Combine(webHostEnvironment.WebRootPath, EventFolder);
            var filePath = Path.Combine(saveFolder, fileName);
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

            if (isOriginal) await File.WriteAllBytesAsync(filePath, imageData.FileBytes);
            else
            {
                using var image = Image.Load(imageData.FileBytes);
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

            return Result<string>.Ok(relativePath, "Image saved!");
        }
        catch (Exception ex)
        {
            return Result<string>.Error($"Error saving image: {ex.Message}");
        }
    }

    string ToFileSizeString(long bytes)
    {
        return bytes switch
        {
            < 1024 => $"{bytes} B",
            < 1_048_576 => $"{bytes / 1024.0:0.##} KB",
            < 1_073_741_824 => $"{bytes / 1_048_576.0:0.##} MB",
            _ => $"{bytes / 1_073_741_824.0:0.##} GB"
        };
    }


    public async Task<Result<List<EventDto>>> GetEventsAsync(string? filter)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            //var entities = await db.Events.AsNoTracking()
            //                                .Where(x => x.Start >= DateTime.Now && (string.IsNullOrEmpty(filter) || x.Title.Contains(filter) || x.Description.Contains(filter) || x.Location.Contains(filter)))
            //                                .OrderByDescending(x => x.Start)
            //                                .ToListAsync();

            var entities = await SearchEvents(filter, db);
            if (!string.IsNullOrWhiteSpace(filter) && entities.Count == 0)
            {
                filter = null;
                entities = await SearchEvents(filter, db);
            }
            if (!entities.Any()) return Result<List<EventDto>>.Error("Event not found!");

            return Result<List<EventDto>>.Ok(entities.ToDtoList());
        }
        catch (Exception ex)
        {
            return Result<List<EventDto>>.Error($"Error: {ex.Message}");
        }
    }

    private static async Task<List<Event>> SearchEvents(string? filter, ApplicationDbContext db)
    {
        return await db.Events.AsNoTracking()
            .Where(x => x.Start >= DateTime.Now && (string.IsNullOrEmpty(filter) || x.Title.Contains(filter) || x.Details.Contains(filter) || x.Location.Contains(filter)))
            .OrderByDescending(x => x.Start)
            .ToListAsync();
    }





    public async Task<Result<List<EventDto>>> GetUserRsvpEventsAsync(Guid userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Events.Include(x => x.Rsvps)
                .AsNoTracking()
                .Where(x => x.Rsvps.Any(x => x.UserId == userId))
                .ToListAsync();

            if (!entities.Any()) return Result<List<EventDto>>.Error("Event not found!");

            return Result<List<EventDto>>.Ok(entities.ToDtoList());
        }
        catch (Exception ex)
        {
            return Result<List<EventDto>>.Error($"Error: {ex.Message}");
        }
    }


    public async Task<Result<List<UserDto>>> GetAttendeesByEventIdAsync(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        try
        {
            var entities = await db.Rsvps.AsNoTracking()
                                            .Include(x => x.User)
                                            .Where(x => x.EventId == id && x.Status == RsvpStatus.Going)
                                            .Select(x => x.User)
                                            .ToListAsync();

            if (!entities.Any()) return Result<List<UserDto>>.Error("Attendees not found!");

            return Result<List<UserDto>>.Ok(entities.ToDtoList());
        }
        catch (Exception ex)
        {
            return Result<List<UserDto>>.Error($"Error: {ex.Message}");
        }
    }



    #region OLD
    //public async Task<Result<List<EventDto>>> GetAllAsyncOld()
    //{
    //    await using var db = await dbFactory.CreateDbContextAsync();

    //    try
    //    {
    //        var query = await db.Events.AsNoTracking().Select(x => new EventDto
    //        {
    //            Id = x.Id,
    //            ImageUrl = x.ImageUrl,
    //            Title = x.Title,
    //            Details = x.Details,
    //            Location = x.Location,
    //            MeetupLink = x.MeetupLink,
    //            Category = x.Category,
    //            Capacity = x.Capacity,
    //            Start = x.Start,
    //            End = x.End,
    //            AllDay = x.AllDay,
    //            OrganizerId = x.OrganizerId
    //        }).ToListAsync();

    //        if (!query.Any()) return Result<List<EventDto>>.Error("Event not found!");

    //        return Result<List<EventDto>>.Ok(query);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result<List<EventDto>>.Error($"Error: {ex.Message}");
    //    }
    //}

    //public async Task<Result<EventDto>> GetByIdAsyncOld(Guid id)
    //{
    //    await using var db = await dbFactory.CreateDbContextAsync();
    //    try
    //    {
    //        var entity = await db.Events.FindAsync(id);
    //        if (entity == null) return Result<EventDto>.Error("Event not found!");

    //        var output = new EventDto
    //        {
    //            Id = entity.Id,
    //            ImageUrl = entity.ImageUrl,
    //            Title = entity.Title,
    //            Details = entity.Details,
    //            Location = entity.Location,
    //            MeetupLink = entity.MeetupLink,
    //            Category = entity.Category,
    //            Capacity = entity.Capacity,
    //            Start = entity.Start,
    //            End = entity.End,
    //            AllDay = entity.AllDay,
    //            OrganizerId = entity.OrganizerId
    //        };

    //        return Result<EventDto>.Ok(output);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result<EventDto>.Error($"Database error: {ex.Message}");
    //    }
    //}

    //public async Task<Result> AddAsyncOld(EventInput input)
    //{
    //    await using var db = await dbFactory.CreateDbContextAsync();

    //    try
    //    {
    //        var entity = new Event(
    //            input.ImageUrl,
    //            input.Title,
    //            input.Details,
    //            input.Location,
    //            input.MeetupLink,
    //            input.Category,
    //            input.Capacity,
    //            input.Start,
    //            input.End,
    //            input.AllDay);

    //        await db.Events.AddAsync(entity);
    //        await db.SaveChangesAsync();

    //        return Result.Ok("Event added!");
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Error($"Database error: {ex.Message}");
    //    }
    //}

    //public async Task<Result> UpdateAsyncOld(Guid id, EventInput input)
    //{
    //    await using var db = await dbFactory.CreateDbContextAsync();
    //    try
    //    {
    //        var entity = await db.Events.FindAsync(id);
    //        if (entity == null) return Result.Error("Event not found!");

    //        entity.ImageUrl = input.ImageUrl;
    //        entity.Title = input.Title;
    //        entity.Details = input.Details;
    //        entity.Location = input.Location;
    //        entity.MeetupLink = input.MeetupLink;
    //        entity.Category = input.Category;
    //        entity.Capacity = input.Capacity;
    //        entity.Start = input.Start;
    //        entity.End = input.End;
    //        entity.AllDay = input.AllDay;

    //        db.Events.Update(entity);
    //        await db.SaveChangesAsync();

    //        return Result.Ok("Event updated!");
    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Error($"Database error: {ex.Message}");
    //    }
    //}
    #endregion
}