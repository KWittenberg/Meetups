namespace Meetups.Shared.Components;

public class BaseComponent : ComponentBase
{
    [Inject] protected AppState App { get; set; } = default!;

    protected override void OnInitialized()
    {
        // base.OnInitializedAsync();
        App.SetFooterContent(null);
    }
}