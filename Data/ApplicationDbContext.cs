namespace Meetups.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : DbContext(options)
{

    // public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)


    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Rsvp> Rsvps { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseAuditableEntity<Guid>>().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.LastModifiedUtc = DateTime.UtcNow;
            entry.Entity.LastModifiedId = GetCurrentUserId();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedUtc = DateTime.UtcNow;
                entry.Entity.CreatedId = GetCurrentUserId();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    Guid GetCurrentUserId() => Guid.TryParse(contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId) ? userId : Guid.Empty;
}