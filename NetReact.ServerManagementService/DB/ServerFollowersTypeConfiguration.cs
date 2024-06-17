using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace NetReact.ServerManagementService.DB;

public class ServerFollowersTypeConfiguration : IEntityTypeConfiguration<ServerFollower>
{
    public void Configure(EntityTypeBuilder<ServerFollower> builder)
    {
        builder.Property<string>(x => x.ServerId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property<string>(x => x.UserId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property<DateTime>(x => x.FollowDate)
            .HasColumnType("DATETIME2(2)")
            .HasColumnOrder(3)
            .HasDefaultValueSql("sysdatetime()");

        builder.HasKey(x => new { x.ServerId, x.UserId });
        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Server)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ServerFollowers");
    }
}