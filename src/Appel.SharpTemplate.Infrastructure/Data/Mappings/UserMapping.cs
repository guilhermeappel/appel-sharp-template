using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appel.SharpTemplate.Infrastructure.Data.Mappings;

public sealed class UserMapping : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasIndex(b => b.ExternalId)
            .IsUnique();

        builder
            .HasIndex(b => b.Email)
            .IsUnique();

        builder
            .Property(b => b.Email)
            .IsRequired()
            .HasColumnType($"varchar({ValidationConstants.User.Shared.EMAIL_MAX_LENGTH})");

        builder
            .Property(b => b.Name)
            .IsRequired()
            .HasColumnType($"varchar({ValidationConstants.User.Shared.NAME_MAX_LENGTH})");

        builder
            .Property(b => b.Password)
            .IsRequired()
            .HasColumnType($"varchar({ValidationConstants.User.Database.PASSWORD_MAX_LENGTH})");

        builder
            .Property(b => b.Surname)
            .IsRequired()
            .HasColumnType($"varchar({ValidationConstants.User.Shared.SURNAME_MAX_LENGTH})");

        builder.ToTable("users");
    }
}
