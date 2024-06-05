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
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property<string>(x => x.UserId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(3)
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Server)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ServerFollowers");
    }
}