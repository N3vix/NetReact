using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddServerFollowersFollowDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FollowDate",
                table: "ServerFollowers",
                type: "DATETIME2(2)",
                nullable: false,
                defaultValueSql: "sysdatetime()")
                .Annotation("Relational:ColumnOrder", 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowDate",
                table: "ServerFollowers");
        }
    }
}
