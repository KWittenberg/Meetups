namespace Meetups.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
        entity.HasIndex(x => x.Email).IsUnique();

        entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
        entity.Property(x => x.FirstName).HasMaxLength(50);
        entity.Property(x => x.LastName).HasMaxLength(50);
        entity.Property(x => x.ImageUrl).HasMaxLength(200);
        entity.Property(x => x.PhoneNumber).HasMaxLength(20);
        entity.Property(x => x.DateOfBirth);
        entity.Property(x => x.Role).HasMaxLength(20);

        entity.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.SetNull);
    }
}