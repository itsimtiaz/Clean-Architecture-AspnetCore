using System.Threading.Channels;
using Application.Cache;
using Application.Data;
using Application.Features.Users.Queries.Interfaces;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistent.BackgroundJobs;
using Persistent.CacheService;
using Persistent.Data;
using Persistent.Events;
using Persistent.Interceptors;
using Persistent.Repositories;
using Persistent.Services;


namespace Persistent;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistent(this IServiceCollection services, IConfiguration configuration)
    {
        services.
        Configure<DatabaseConnections>(configuration.GetRequiredSection(DatabaseConnections.ConnectionStrings));

        services.AddSingleton<OutboxInterceptorHandler>();

        services.AddDbContext<WriteDbContext>(
            (sp, options) =>
            {
                var interceptor = sp.GetRequiredService<OutboxInterceptorHandler>();

                var dbConnections = sp.GetRequiredService<IOptions<DatabaseConnections>>().Value;

                options.UseSqlServer(dbConnections.DefaultConnection)
                    .AddInterceptors(interceptor);
            });

        services.AddDbContext<ReadDbContext>((sp, options) =>
        {
            var dbConnections = sp.GetRequiredService<IOptions<DatabaseConnections>>().Value;

            options.UseSqlServer(dbConnections.DefaultConnection);
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<WriteDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGetUserService, GetUserService>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, AppCacheService>();

        services.AddHostedService<OutBoxJobHandler>();

        services.AddSingleton<Channel<BackgroundTriggerEvent>>(Channel.CreateUnbounded<BackgroundTriggerEvent>());

        return services;
    }
}
