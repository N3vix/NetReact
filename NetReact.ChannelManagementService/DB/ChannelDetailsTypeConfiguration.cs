using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace NetReact.ChannelManagementService.DB;

public class ChannelDetailsTypeConfiguration : IEntityTypeConfiguration<ChannelDetails>
{
    public void Configure(EntityTypeBuilder<ChannelDetails> builder)
    {
        builder.Property<string>(x => x.Id)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property<string>(x => x.ServerId)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property<string>(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(100)")
            .HasColumnOrder(3)
            .IsRequired();
        
        builder.Property<ChannelType>(x => x.Type)
            .HasColumnType("tinyint")
            .HasColumnOrder(4)
            .HasConversion<int>()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.ToTable("ChannelDetails");
    }
}