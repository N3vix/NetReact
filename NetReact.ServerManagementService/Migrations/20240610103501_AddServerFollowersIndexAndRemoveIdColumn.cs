using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddServerFollowersIndexAndRemoveIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServerFollowers");

            migrationBuilder.CreateIndex(
                name: "IX_ServerFollowers_UserId",
                table: "ServerFollowers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerFollowers_UserId",
                table: "ServerFollowers");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ServerFollowers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
