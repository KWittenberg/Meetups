namespace Meetups.DTOs;

public static class AddressMapping
{
    // GetAllAsync()
    public static List<AddressDto> ToDtoList(this IEnumerable<Address>? entities)
    {
        if (entities is null) return new List<AddressDto>();

        return entities.Select(e => e.ToDto()).Where(dto => dto is not null).ToList();
    }

    // GetByIdAsync()
    public static AddressDto ToDto(this Address? entity)
    {
        if (entity is null) return null;

        return new AddressDto
        {
            Id = entity.Id,
            Country = entity.Country,
            Zip = entity.Zip,
            City = entity.City,
            Street = entity.Street,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude
        };
    }

    // AddAsync()
    public static Address ToEntity(this AddressInput? input)
    {
        if (input == null) return null;

        return new Address
        {
            Country = input.Country,
            Zip = input.Zip,
            City = input.City,
            Street = input.Street,
            Latitude = input.Latitude,
            Longitude = input.Longitude
        };
    }

    // UpdateAsync()
    public static void UpdateFromInput(this Address? entity, AddressInput? input)
    {
        if (entity == null || input == null) return;

        entity.Country = input.Country;
        entity.Zip = input.Zip;
        entity.City = input.City;
        entity.Street = input.Street;
        entity.Latitude = input.Latitude;
        entity.Longitude = input.Longitude;
    }

    // UI -> 
    public static AddressInput ToInput(this AddressDto? dto)
    {
        if (dto is null) return null;

        return new AddressInput
        {
            Country = dto.Country,
            Zip = dto.Zip,
            City = dto.City,
            Street = dto.Street,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }
}