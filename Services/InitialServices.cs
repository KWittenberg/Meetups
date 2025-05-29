namespace Meetups.Services;

public static class InitialServices
{
    public static IServiceCollection AddInitialServices(this IServiceCollection services, IConfiguration configuration)
    {



        var connectionString = configuration.GetConnectionString("DevelopmentConnection") ?? throw new InvalidOperationException("Connection string 'DevelopmentConnection' not found!");
        services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        // services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);


        // Add Google Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId not found in configuration.");
                options.ClientSecret = configuration["Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret not found in configuration.");

                options.Events = new OAuthEvents
                {
                    OnTicketReceived = async context =>
                    {
                        if (context.Principal is not null)
                        {
                            await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, context.Principal, context.Properties);
                            context.Response.Redirect("/signin-callback");
                            context.HandleResponse(); // Suppress the default response
                        }
                    }
                };

            });


        //services.AddCascadingAuthenticationState();
        //services.AddScoped<IdentityUserAccessor>();
        //services.AddScoped<IdentityRedirectManager>();
        //services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        //services.AddAuthorization();
        //services.AddAuthentication(options =>
        //    {
        //        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //    })
        //    .AddIdentityCookies();

        //// Add database
        //var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        //services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        //services.AddDatabaseDeveloperPageExceptionFilter();

        //services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //    .AddRoles<IdentityRole<Guid>>() // Add for Seed with Guid
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddSignInManager()
        //    .AddDefaultTokenProviders();



        return services;
    }
}