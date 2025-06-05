namespace Meetups.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Country).HasMaxLength(50);

        builder.Property(x => x.Zip).HasMaxLength(10);

        builder.Property(x => x.City).HasMaxLength(100);

        builder.Property(x => x.Street).HasMaxLength(200);
    }
}