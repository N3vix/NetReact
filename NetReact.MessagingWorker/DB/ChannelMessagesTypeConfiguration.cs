using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace NetReact.MessagingService.DB;

public class ChannelMessagesTypeConfiguration : IEntityTypeConfiguration<ChannelMessage>
{
    public void Configure(EntityTypeBuilder<ChannelMessage> builder)
    {
        builder.Property<string>(x => x.Id)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property<string>(x => x.ChannelId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(2)
            .IsRequired();
        
        builder.Property<string>(x => x.SenderId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property<DateTime>(x => x.Timestamp)
            .HasColumnType("DATETIME2(2)")
            .HasColumnOrder(4)
            .HasDefaultValueSql("sysdatetime()");
        
        builder.Property<string>(x => x.Content)
            .HasColumnType("nvarchar(200)")
            .HasColumnOrder(5)
            .IsRequired();
        
        builder.Property<string>(x => x.Image)
            .HasColumnType("nvarchar(100)")
            .HasColumnOrder(6);
        
        builder.Property<DateTime>(x => x.Timestamp)
            .HasColumnType("DATETIME2(2)")
            .HasColumnOrder(7);

        builder.HasKey(x => x.Id);

        builder.ToTable("ChannelMessages");
    }
}