using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationDateAndFluentAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ServerDetails",
                type: "DATETIME2(2)",
                nullable: false,
                defaultValueSql: "sysdatetime()")
                .Annotation("Relational:ColumnOrder", 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ServerDetails");
        }
    }
}
