namespace Meetups.Repository;

public static class ConfigureRepositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IRsvpRepository, RsvpRepository>();



        return services;
    }
}