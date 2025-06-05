namespace Meetups.DTOs;

public class AddressInput
{
    public string? Country { get; set; }

    public string? Zip { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }




    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? PlaceId { get; set; }
}