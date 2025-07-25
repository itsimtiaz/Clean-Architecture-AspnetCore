using Application.DTOs;

namespace Application.Features.Users.Queries.Interfaces;

public interface IGetUserService
{
    Task<UserDto?> GetUserById(int id, CancellationToken cancellationToken = default);
}
