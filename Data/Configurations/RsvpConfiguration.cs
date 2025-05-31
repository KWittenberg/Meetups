namespace Meetups.Data.Configurations;

public class RsvpConfiguration : IEntityTypeConfiguration<Rsvp>
{
    public void Configure(EntityTypeBuilder<Rsvp> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.RsvpDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();

        entity.Property(x => x.Status).IsRequired();


        entity.HasOne(r => r.User).WithMany(u => u.Rsvps).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(r => r.Event).WithMany(e => e.Rsvps).HasForeignKey(r => r.EventId).OnDelete(DeleteBehavior.Restrict);
    }
}