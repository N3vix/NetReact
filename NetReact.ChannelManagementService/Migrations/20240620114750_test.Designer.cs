﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetReact.ChannelManagementService.DB;

#nullable disable

namespace NetReact.ChannelManagementService.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240620114750_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.ChannelDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(3);

                    b.Property<string>("ServerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(2);

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint")
                        .HasColumnOrder(4);

                    b.HasKey("Id");

                    b.ToTable("ChannelDetails", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
