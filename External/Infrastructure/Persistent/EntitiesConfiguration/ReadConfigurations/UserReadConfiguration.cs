using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistent.EF.Modals;

namespace Persistent.EntitiesConfiguration.ReadConfigurations;

internal class UserReadConfiguration : IEntityTypeConfiguration<UserReadModal>
{
    public void Configure(EntityTypeBuilder<UserReadModal> builder)
    {
        builder.HasKey(_ => _.Id);
    }
}
