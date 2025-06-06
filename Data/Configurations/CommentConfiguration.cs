namespace Meetups.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> entity)
    {
        entity.ToTable("Comments");

        entity.HasKey(e => e.Id);

        entity.HasIndex(x => new { x.EventId, x.CreatedUtc });

        entity.Property(x => x.UserId).IsRequired();

        entity.Property(x => x.UserName).HasMaxLength(50).IsRequired();

        entity.Property(x => x.EventId).IsRequired();

        entity.Property(x => x.Message).HasMaxLength(1000);

        entity.Property(x => x.CreatedUtc).HasDefaultValueSql("GETUTCDATE()").IsRequired();


        entity.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(c => c.Event).WithMany(e => e.Comments).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);
    }
}