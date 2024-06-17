﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetReact.ServerManagementService.DB;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240605082703_NewInitial")]
    partial class NewInitial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.ServerDetails", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(400)")
                        .HasColumnOrder(3);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.ToTable("ServerDetails", (string)null);
                });

            modelBuilder.Entity("Models.ServerFollower", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(1);

                    b.Property<string>("ServerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(2);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasColumnOrder(3);

                    b.HasKey("Id");

                    b.ToTable("ServerFollowers", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}