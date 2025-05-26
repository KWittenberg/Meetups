namespace Meetups.DTOs;

public class EventInput
{
    [StringLength(maximumLength: 200)]
    public string? ImageUrl { get; set; }


    [Required]
    [StringLength(maximumLength: 100)]
    public string Title { get; set; } = string.Empty;


    [StringLength(maximumLength: 4000)]
    public string? Description { get; set; }


    [StringLength(maximumLength: 200)]
    public string? Location { get; set; }


    [StringLength(maximumLength: 200)]
    public string? MeetupLink { get; set; }


    [StringLength(maximumLength: 100)]
    public string? Category { get; set; }


    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }


    public DateTime Start { get; set; } = DateTime.Now;

    public DateTime End { get; set; } = DateTime.Now.AddMinutes(30);


    public bool AllDay { get; set; }

    public Guid? OrganizerId { get; set; }


    //public EventInput()
    //{
    //    var now = DateTime.Now;

    //    StartDate = DateOnly.FromDateTime(now);
    //    StartTime = TimeOnly.FromDateTime(now);
    //    EndDate = DateOnly.FromDateTime(now);
    //    EndTime = TimeOnly.FromDateTime(now.AddHours(1));

    //    //Category = MeetupCategories.InPerson.ToString();
    //}


    //public string? ValidateDate()
    //{
    //    // if (StartDate > EndDate) return "Start Date should be earlier then End Date!";
    //    if (StartDate == EndDate && StartTime > EndTime) return "Start Time should be earlier then End Time!";

    //    DateTime combinedStartDateTime = new DateTime(StartDate.DayOfYear, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
    //    if (combinedStartDateTime == DateTime.Now) return "Start Date and Time should be in the future!";

    //    DateTime combinedEndDateTime = new DateTime(EndDate.DayOfYear, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
    //    if (combinedEndDateTime <= combinedStartDateTime) return "End Date and Time should be later than Start Date and Time!";

    //    if (combinedEndDateTime - combinedStartDateTime > TimeSpan.FromDays(1)) return "The duration of the event should not exceed 24 hours!";


    //    return string.Empty;
    //}

    public string? ValidateLocation()
    {
        if (Category == MeetupCategories.InPerson.ToString() && string.IsNullOrWhiteSpace(Location)) return "Location is required for In-Person Meetup!";

        return string.Empty;
    }

    public string? ValidateMeetupLink()
    {
        if (Category == MeetupCategories.Online.ToString() && string.IsNullOrWhiteSpace(MeetupLink)) return "The Meetup Link is required for Online Meetups!";

        return string.Empty;
    }
}