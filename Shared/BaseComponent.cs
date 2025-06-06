namespace Meetups.Shared;

public class BaseComponent : ComponentBase
{
    [Inject] protected AppState App { get; set; } = default!;

    [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;



    protected AuthenticationState? AuthenticationState;

    protected bool isAuthenticated = false;




    protected override async Task OnInitializedAsync()
    {
        AuthenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        isAuthenticated = AuthenticationState.User.Identity?.IsAuthenticated ?? false;
        UpdateCurrentUser();
    }

    private void UpdateCurrentUser()
    {
        if (isAuthenticated)
        {
            Console.WriteLine("Claims:");
            foreach (var claim in AuthenticationState.User.Claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }



            var userIdString = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            App.CurrentUser.Id = Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;

            App.CurrentUser.Email = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            App.CurrentUser.Name = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

            App.CurrentUser.FirstName = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? string.Empty;
            App.CurrentUser.LastName = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? string.Empty;
            App.CurrentUser.ImageUrl = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value ?? string.Empty;

            App.CurrentUser.Role = AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            App.CurrentUser.IsAuthenticated = true;
        }
        else
        {
            App.ResetCurrentUser();
        }
    }





    protected bool IsAuthenticated => isAuthenticated;

    protected string UserId
    {
        get
        {
            return isAuthenticated ? AuthenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value : string.Empty;
        }
    }

    protected string UserName
    {
        get
        {
            return isAuthenticated ? AuthenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value : string.Empty;
        }
    }

    protected string UserEmail
    {
        get
        {
            return isAuthenticated ? AuthenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value : string.Empty;
        }
    }

    protected string UserRole
    {
        get
        {
            return isAuthenticated ? AuthenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value : string.Empty;
        }
    }

    protected bool IsOrganizer
    {
        get
        {
            if (isAuthenticated) return AuthenticationState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == ApplicationRole.Organizer;
            return false;
        }
    }




    protected string GetCurrentUserId()
    {
        return AuthenticationState?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}