namespace Meetups.Data.Configurations;

public class RsvpConfiguration : IEntityTypeConfiguration<Rsvp>
{
    public void Configure(EntityTypeBuilder<Rsvp> entity)
    {
        entity.ToTable("Rsvps");

        entity.HasKey(e => e.Id);

        entity.HasIndex(r => r.EventId);
        entity.HasIndex(r => r.UserId);

        entity.Property(x => x.PaymentId).HasMaxLength(50);

        entity.Property(x => x.RefundId).HasMaxLength(50);

        entity.Property(x => x.CreatedUtc).HasDefaultValueSql("GETUTCDATE()").IsRequired();


        entity.HasOne(r => r.User).WithMany(u => u.Rsvps).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(r => r.Event).WithMany(e => e.Rsvps).HasForeignKey(r => r.EventId).OnDelete(DeleteBehavior.Cascade);
    }
}