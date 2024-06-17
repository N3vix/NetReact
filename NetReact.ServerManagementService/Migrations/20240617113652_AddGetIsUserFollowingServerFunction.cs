using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetReact.ServerManagementService.Migrations
{
    public partial class AddGetIsUserFollowingServerFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE FUNCTION [dbo].[GetIsUserFollowing] (@UserId varchar(40), @ServerId varchar(40))
                    RETURNS INT
                AS
                BEGIN
                    RETURN (SELECT COUNT(*)
                		FROM ServerFollowers F
                		WHERE F.ServerId = @ServerId AND F.UserId = @UserId)
                END;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION GetIsUserFollowing");
        }
    }
}
