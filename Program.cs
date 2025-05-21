using Meetups.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add Repo and Services
services.AddInitialServices(configuration);
services.AddRepositories(configuration);

// services.AddTransient<EventService>();


// Add services to the container.
services.AddRazorComponents().AddInteractiveServerComponents();


// Configure DbContext and DI
// var connectionString = configuration.GetConnectionString("DevelopmentConnection") ?? throw new InvalidOperationException("Connection string 'DevelopmentConnection' not found!");
// services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));





#region app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
#endregion