using Application.DTOs;
using Application.Features.Users.Queries.Interfaces;
using Domain.DomainExceptions;
using Mediator;

namespace Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
{
    private readonly IGetUserService _userService;

    public GetUserQueryHandler(IGetUserService userService)
    {
        _userService = userService;
    }

    public async ValueTask<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserById(query.id, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(query.id);

        return user;
    }
}