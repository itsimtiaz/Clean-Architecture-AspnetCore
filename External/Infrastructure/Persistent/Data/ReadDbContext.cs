using Microsoft.EntityFrameworkCore;
using Persistent.EF.Modals;
using Persistent.EntitiesConfiguration.ReadConfigurations;
using Persistent.Interceptors;

namespace Persistent.Data;

internal class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration<UserReadModal>(new UserReadConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    internal DbSet<UserReadModal> Users { get; private set; }
    internal DbSet<OutBoxMessage> OutBoxMessages { get; private set; }
}
