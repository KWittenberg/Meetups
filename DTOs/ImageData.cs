namespace Meetups.DTOs;

public class ImageData
{
    //public IBrowserFile? File { get; set; }

    public byte[]? FileBytes { get; set; }

    public string? ImagePreviewBase64 { get; set; }



    public string? Name { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public long? Size { get; set; }

    public string? FormattedSize { get; set; }

    public string? ContentType { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}