using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiegeInitiative.Domain.Entities;

namespace SiegeInitiative.Infrastructure.Persistence.Core.Mapping;

public sealed class TeamMap : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id)
            .HasColumnName(nameof(Team.Id))
            .ValueGeneratedOnAdd();

        builder.Property(_ => _.Name)
            .HasColumnName(nameof(Team.Name))
            .HasColumnType("VARCHAR(32)")
            .IsRequired();

        builder.Property(_ => _.Tag)
            .HasColumnName(nameof(Team.Tag))
            .HasColumnType("VARCHAR(16)")
            .IsRequired(false);

        builder.Property(_ => _.Tier)
            .HasColumnName(nameof(Team.Tier))
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(_ => _.Region)
            .HasColumnName(nameof(Team.Region))
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(_ => _.Nationality)
            .HasColumnName(nameof(Team.Nationality))
            .HasColumnType("VARCHAR(32)")
            .IsRequired(false);

        builder.Property(_ => _.CreatedAt)
            .HasColumnName(nameof(Team.CreatedAt))
            .HasColumnType("DATETIME2")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(_ => _.UpdatedAt)
            .HasColumnName(nameof(Team.UpdatedAt))
            .HasColumnType("DATETIME2")
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()")
            .IsRequired(false);
    }
}