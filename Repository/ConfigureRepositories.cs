namespace Meetups.Repository;

public static class ConfigureRepositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IRsvpRepository, RsvpRepository>();



        return services;
    }
}