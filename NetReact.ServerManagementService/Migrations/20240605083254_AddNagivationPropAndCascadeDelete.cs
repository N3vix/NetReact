using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    /// <inheritdoc />
    public partial class AddNagivationPropAndCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ServerFollowers_ServerId",
                table: "ServerFollowers",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerFollowers_ServerDetails_ServerId",
                table: "ServerFollowers",
                column: "ServerId",
                principalTable: "ServerDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerFollowers_ServerDetails_ServerId",
                table: "ServerFollowers");

            migrationBuilder.DropIndex(
                name: "IX_ServerFollowers_ServerId",
                table: "ServerFollowers");
        }
    }
}
