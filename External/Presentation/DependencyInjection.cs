using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class DependencyInjection
{

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder AddEndPoints(this IEndpointRouteBuilder app)
    {
        app.AddUserEndPoints();
        return app;
    }
}
