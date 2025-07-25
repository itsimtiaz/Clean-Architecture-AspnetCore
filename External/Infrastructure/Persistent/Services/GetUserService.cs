using Application.DTOs;
using Application.Features.Users.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistent.Data;

namespace Persistent.Services;

internal class GetUserService : IGetUserService
{
    private readonly ReadDbContext readDbContext;

    public GetUserService(ReadDbContext readDbContext)
    {
        this.readDbContext = readDbContext;
    }
    public async Task<UserDto?> GetUserById(int id, CancellationToken cancellationToken = default)
    {
        var user = await readDbContext.Users.FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);
        if (user is not null)
        {
            return new UserDto(user.Id, user.Name, user.Age);
        }

        return null;
    }
}
