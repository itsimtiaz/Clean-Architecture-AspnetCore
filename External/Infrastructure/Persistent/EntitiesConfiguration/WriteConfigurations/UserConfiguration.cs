using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistent.EntitiesConfiguration;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var userNameConvertor = new ValueConverter<UserName, string>(i => i.Value, o => (UserName)o);
        var ageConvertor = new ValueConverter<UserAge, int>(i => i.Value, o => (UserAge)o);

        builder.HasKey(_ => _.Id);

        builder.Property(typeof(UserName), "_name").HasConversion(userNameConvertor).HasColumnName("Name").
        HasColumnType("nvarchar").HasMaxLength(20);


        builder.Property(typeof(UserAge), "_age").HasConversion(ageConvertor).HasColumnName("Age");

    }
}
