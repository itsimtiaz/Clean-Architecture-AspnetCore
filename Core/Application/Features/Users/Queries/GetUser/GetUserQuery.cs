using Application.DTOs;
using Mediator;

namespace Application.Features.Users.Queries.GetUser;

public record GetUserQuery(int id, CancellationToken CancellationToken = default) : IQuery<UserDto>;
