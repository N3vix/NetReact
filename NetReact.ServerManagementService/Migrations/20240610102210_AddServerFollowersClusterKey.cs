using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddServerFollowersClusterKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerFollowers",
                table: "ServerFollowers");

            migrationBuilder.DropIndex(
                name: "IX_ServerFollowers_ServerId",
                table: "ServerFollowers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ServerFollowers",
                type: "nvarchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "ServerId",
                table: "ServerFollowers",
                type: "nvarchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FollowDate",
                table: "ServerFollowers",
                type: "DATETIME2(2)",
                nullable: false,
                defaultValueSql: "sysdatetime()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(2)",
                oldDefaultValueSql: "sysdatetime()")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServerFollowers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerFollowers",
                table: "ServerFollowers",
                columns: new[] { "ServerId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerFollowers",
                table: "ServerFollowers");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ServerFollowers",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FollowDate",
                table: "ServerFollowers",
                type: "DATETIME2(2)",
                nullable: false,
                defaultValueSql: "sysdatetime()",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2(2)",
                oldDefaultValueSql: "sysdatetime()")
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ServerFollowers",
                type: "nvarchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "ServerId",
                table: "ServerFollowers",
                type: "nvarchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerFollowers",
                table: "ServerFollowers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServerFollowers_ServerId",
                table: "ServerFollowers",
                column: "ServerId");
        }
    }
}
