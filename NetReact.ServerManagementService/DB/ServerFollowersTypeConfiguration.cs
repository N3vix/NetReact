using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace NetReact.ServerManagementService.DB;

public class ServerFollowersTypeConfiguration : IEntityTypeConfiguration<ServerFollower>
{
    public void Configure(EntityTypeBuilder<ServerFollower> builder)
    {
        builder.Property<string>(x => x.Id)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property<string>(x => x.ServerId)
            .IsRequired()
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property<string>(x => x.UserId)
            .IsRequired()
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(3)
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.ToTable("ServerFollowers");
    }
}