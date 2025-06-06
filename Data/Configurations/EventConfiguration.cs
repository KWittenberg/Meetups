namespace Meetups.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable("Events");

        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.CategoryId);
        entity.HasIndex(e => new { e.Start, e.End });
        entity.HasIndex(e => e.LocationType);


        entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
        entity.Property(e => e.ImageUrl).HasMaxLength(200);
        entity.Property(e => e.Details).HasMaxLength(4000);

        entity.Property(e => e.Start).IsRequired();
        entity.Property(e => e.End);
        entity.Property(e => e.AllDay);
        entity.Property(e => e.Recurrence).HasDefaultValue(Recurrence.OneTime).IsRequired();
        entity.Property(e => e.Capacity).HasDefaultValue(null);

        entity.Property(e => e.LocationType).IsRequired();
        entity.Property(e => e.MeetupLink).HasMaxLength(200);
        entity.Property(e => e.TicketPrice).HasColumnType("decimal(9,2)");
        entity.Property(e => e.Refundable);



        entity.HasOne(e => e.User).WithMany(u => u.Events).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Category).WithMany(c => c.Events).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(e => e.Address).WithMany().HasForeignKey(e => e.AddressId).OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(e => e.Rsvps).WithOne(r => r.Event).HasForeignKey(r => r.EventId).OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Comments).WithOne(c => c.Event).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Cascade);
    }
}