using Domain.Entities;
using Domain.Repositories;
using Persistent.Data;

namespace Persistent.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly WriteDbContext _writeDbContext;

    public UserRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Users.AddAsync(user, cancellationToken);

    }

    public async Task<User?> GetAsync(uint id, CancellationToken cancellationToken = default)
    {
        return await _writeDbContext.Users.FindAsync(id, cancellationToken);
    }
}
