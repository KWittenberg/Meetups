namespace Meetups.Services;

public static class ConfigureServices
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminOptions>(configuration.GetSection(nameof(AdminOptions)));

        services.AddSingleton<AppState>();

        services.AddScoped<IPaymentService, PaymentService>();



        return services;
    }
}