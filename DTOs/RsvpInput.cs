﻿namespace Meetups.DTOs;

public class RsvpInput
{
    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public DateTime RsvpDate { get; set; } = DateTime.UtcNow;

    public RsvpStatus Status { get; set; } = RsvpStatus.Going;

    [StringLength(maximumLength: 200)]
    public string? PaymentId { get; set; }
}