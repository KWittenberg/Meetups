﻿namespace Meetups.Shared;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        #region Attendee authentication
        app.MapGet("/authentication/{provider}",
            async (string provider, HttpContext context) =>
            {
                var redirectUrl = "/signin-callback";
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                await context.ChallengeAsync(provider, properties);
            });

        app.MapGet("/signin-callback", async (HttpContext context, IDbContextFactory<ApplicationDbContext> contextFactory) =>
        {
            await HandleSignInCallbackAsync(context, contextFactory);

            context.Response.Redirect("/");
        });
        #endregion

        #region Organizer authentication
        app.MapGet("/authentication/{provider}/organizer",
            async (string provider, HttpContext context) =>
            {
                var redirectUrl = "/signin-callback/organizer";
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                await context.ChallengeAsync(provider, properties);
            });

        app.MapGet("/signin-callback/organizer", async (HttpContext context, IDbContextFactory<ApplicationDbContext> contextFactory) =>
        {
            await HandleSignInCallbackAsync(context, contextFactory, true);

            context.Response.Redirect("/");
        });
        #endregion


        app.MapGet("/logout", async (HttpContext context, AppState appState) =>
        {
            appState.ResetCurrentUser();

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/");
        });


        app.MapGet("/rsvp/{eventId:guid}/{paymentId?}",
            async (Guid eventId, string? paymentId, HttpContext context, IRsvpRepository rsvpRepository, IDbContextFactory<ApplicationDbContext> contextFactory) =>
            {
                var emailClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                var result = await rsvpRepository.AddAsync(emailClaim.Value, eventId, paymentId);

                if (result.Success)
                {
                    await using var dbContext = await contextFactory.CreateDbContextAsync();
                    var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == emailClaim.Value);
                    context.Response.Redirect($"/manage-user/{user?.Id}");
                }
                else
                {
                    context.Response.Redirect("/rsvp-error");
                }
            });
    }


    private static async Task HandleSignInCallbackAsync(HttpContext context, IDbContextFactory<ApplicationDbContext> contextFactory, bool isOrganizer = false)
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
                    Role = isOrganizer ? ApplicationRole.Organizer : ApplicationRole.Attendee
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                if (existingUser.Name != nameClaim.Value) existingUser.Name = nameClaim.Value;

                //var expectedRole = isOrganizer ? ApplicationRole.Organizer : ApplicationRole.Attendee;
                //if (existingUser.Role != expectedRole) existingUser.Role = expectedRole;

                if (isOrganizer && existingUser.Role != ApplicationRole.Organizer) existingUser.Role = ApplicationRole.Organizer;

                await dbContext.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUser.Name),
                new Claim(ClaimTypes.Email, existingUser.Email),
                new Claim(ClaimTypes.Role, existingUser.Role ?? ApplicationRole.Attendee)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
        else
        {
            context.Response.Redirect("/");
            return;
        }
    }
}