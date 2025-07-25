using Application.DTOs;
using Application.Features.Users.Commands;
using Application.Features.Users.Queries.GetUser;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation;

internal static class UserEndPoint
{
    internal static IEndpointRouteBuilder AddUserEndPoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("users");

        userGroup.MapGet("", WelcomeMessage);

        userGroup.MapGet("/{id:int}", GetUserAsync);

        userGroup.MapPost("", CreateUserAsync);

        return app;
    }

    static string WelcomeMessage() => "Hello from the presentation layer";

    static async Task<UserDto?> GetUserAsync(
    [FromRoute] int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        GetUserQuery query = new(id, cancellationToken);
        var user = await sender.Send(query);

        return user;
    }

    static async Task<Created<UserDto>> CreateUserAsync(
    [FromBody] CreateUserCommand createUserCommand,
    ISender dispatcher,
    CancellationToken cancellationToken)
    {
        var createdUser = await dispatcher.Send(createUserCommand, cancellationToken);

        return TypedResults.Created($"/users/{createdUser.Id}", createdUser);
    }

}
