namespace Meetups.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)


    public DbSet<Event> Events { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Rsvp> Rsvps { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rsvp>().HasOne(x => x.User).WithMany(x => x.Rsvps).HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Rsvp>().HasOne(x => x.Event).WithMany(x => x.Rsvps).HasForeignKey(x => x.EventId);

        modelBuilder.Entity<Comment>().HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Comment>().HasOne(x => x.Event).WithMany(x => x.Comments).HasForeignKey(x => x.EventId);





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