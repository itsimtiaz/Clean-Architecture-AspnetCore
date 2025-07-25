using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistent.EntitiesConfiguration;
using Persistent.Interceptors;

namespace Persistent.Data;

internal class WriteDbContext : DbContext, IUnitOfWork
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<User>(new UserConfiguration());
    }

    internal DbSet<User> Users { get; set; }
    internal DbSet<OutBoxMessage> OutBoxMessages { get; set; }
}
