namespace Meetups.Shared;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/authentication/{provider}",
            async (string provider, HttpContext context) =>
            {
                var redirectUrl = "/signin-callback";
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                await context.ChallengeAsync(provider, properties);
            });

        app.MapGet("/signin-callback", async (HttpContext context, IDbContextFactory<ApplicationDbContext> contextFactory) =>
        {
            if (context.User is null || context.User.Identity is null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/");
                return;
            }

            var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (emailClaim is not null && nameClaim is not null)
            {
                await using var dbContext = contextFactory.CreateDbContext();

                var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == emailClaim.Value);
                if (existingUser is null)
                {
                    var user = new User
                    {
                        Name = nameClaim.Value,
                        Email = emailClaim.Value,
                        Role = ApplicationRole.Attendee
                    };

                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    if (existingUser.Name != nameClaim.Value)
                    {
                        existingUser.Name = nameClaim.Value;
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                context.Response.Redirect("/");
                return;
            }

            context.Response.Redirect("/");
        });

        app.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/");
        });
    }
}