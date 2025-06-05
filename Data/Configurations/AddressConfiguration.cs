namespace Meetups.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Country).HasMaxLength(50);

        builder.Property(x => x.Zip).HasMaxLength(10);

        builder.Property(x => x.City).HasMaxLength(100);

        builder.Property(x => x.Street).HasMaxLength(200);


        builder.Property(x => x.Latitude).HasColumnType("decimal(9,6)");

        builder.Property(x => x.Longitude).HasColumnType("decimal(9,6)");

        builder.Property(x => x.PlaceId).HasMaxLength(50);


        // Check constraints
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Address_Latitude", "[Latitude] IS NULL OR ([Latitude] >= -90 AND [Latitude] <= 90)");
            t.HasCheckConstraint("CK_Address_Longitude", "[Longitude] IS NULL OR ([Longitude] >= -180 AND [Longitude] <= 180)");
        });

        // Indexes for faster queries
        builder.HasIndex(x => x.City);
        builder.HasIndex(x => new { x.Latitude, x.Longitude });
        builder.HasIndex(x => x.PlaceId).IsUnique();
    }
}