using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace NetReact.ServerManagementService.DB;

public class ServerDetailsTypeConfiguration : IEntityTypeConfiguration<ServerDetails>
{
    public void Configure(EntityTypeBuilder<ServerDetails> builder)
    {
        builder.Property<string>(x => x.Id)
            .HasColumnType("nvarchar(40)")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property<string>(x => x.Name)
            .IsRequired()
            .HasColumnType("nvarchar(100)")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property<string>(x => x.Description)
            .HasColumnType("nvarchar(400)")
            .HasColumnOrder(3);
        
        builder.Property<DateTime>(x => x.CreationDate)
            .HasColumnType("DATETIME2(2)")
            .HasColumnOrder(4)
            .HasDefaultValueSql("sysdatetime()");

        builder.HasKey(x => x.Id);

        builder.ToTable("ServerDetails");
    }
}