using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiegeInitiative.Domain.Entities;

namespace SiegeInitiative.Infrastructure.Persistence.Core.Mapping;

public sealed class PlayerMap : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .HasColumnName(nameof(Player.Id))
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.Name)
            .HasColumnName(nameof(Player.Name))
            .HasColumnType("VARCHAR(64)")
            .IsRequired();

        builder.Property(_ => _.Nickname)
            .HasColumnName(nameof(Player.Nickname))
            .HasColumnType("VARCHAR(16)")
            .IsRequired();

        builder.Property(_ => _.Nationality)
            .HasColumnName(nameof(Player.Nationality))
            .HasColumnType("VARCHAR(32)")
            .IsRequired();

        builder.Property(_ => _.Function)
            .HasColumnName(nameof(Player.Function))
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(_ => _.FreeAgent)
            .HasColumnName(nameof(Player.FreeAgent))
            .HasColumnType("BIT")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(_ => _.Team)
            .WithMany(_ => _.Players)
            .HasConstraintName("TeamId")
            .IsRequired();

        builder.Property(_ => _.CreatedAt)
            .HasColumnName(nameof(Player.CreatedAt))
            .HasColumnType("DATETIME2")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(_ => _.UpdatedAt)
            .HasColumnName(nameof(Player.UpdatedAt))
            .HasColumnType("DATETIME2")
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()")
            .IsRequired(false);
    }
}