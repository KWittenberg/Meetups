namespace Meetups.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)


    public DbSet<Event> Events { get; set; } = null!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    foreach (var entry in base.ChangeTracker.Entries<BaseAuditableEntity<Guid>>().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
    //    {
    //        entry.Entity.LastModifiedUtc = DateTime.UtcNow;
    //        entry.Entity.LastModifiedId = GetCurrentUserId();

    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedUtc = DateTime.UtcNow;
    //            entry.Entity.CreatedId = GetCurrentUserId();
    //        }
    //    }

    //    return base.SaveChangesAsync(cancellationToken);
    //}

    // Guid GetCurrentUserId() => Guid.TryParse(contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId) ? userId : Guid.Empty;
}