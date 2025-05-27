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

        app.MapGet("/signin-callback", async (HttpContext context) =>
        {
            var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded || result.Principal == null)
            {
                context.Response.Redirect("/");
                return;
            }

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Principal);
            context.Response.Redirect("/");
        });

        app.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/");
        });
    }
}